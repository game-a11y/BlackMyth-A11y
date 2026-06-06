# PalAccess::Speech 输出去重层 —— 架构参考

> 提取自 `ref/PalworldAccessibility/src/Speech.cpp`，面向其他 mod（尤其是屏幕阅读器/语音反馈类 mod）的输出去重设计指南。

---

## 1. 核心问题

游戏 UI 事件是**爆发性的**——单次键盘焦点可能触发 3 个 UE 事件（`BP_OnHovered` + `AnmEvent_Focus` + `AnmEvent_Hover`）；面板构建时子控件级联触发数百个事件。屏幕阅读器驱动必须将这些原始事件流提炼为**语义上有意义的语音输出**。

PalAccess 的 `Speech` 类提供了**通用的、基于文本的去重层**，而各个 Hook 分发器提供了**上下文感知的第二层去重**。

---

## 2. 语音策略分类

| 策略 | 方法 | 中断 | 合并 | 去重 | 典型场景 |
|------|------|------|------|------|---------|
| **Cued（合并型）** | `Speak` | 否 | 是（单槽） | 200ms + 30 条历史 | 状态变化、HUD 读数 |
| **Cued（保序型）** | `Announce` / `Queue` | 否 | 否 | 仅记录标记 | 对话、通知、材料列表 |
| **Interrupting（级联型）** | `FocusUpdate` | 是(*) | 级联期间合并 | 150ms | 菜单导航、焦点移动 |
| **Interrupting（直通型）** | `SpeakNow` | 是 | 否 | 无 | 热键响应、用户直接操作 |

(*) FocusUpdate 在级联活跃期间不中断——此时它行为类似合并型。

### 设计原则

- **合并型**用于"只关心最新值"的信息（音量变化、速度读数）
- **保序型**用于"每条消息都重要"的信息（对话对白、通知）
- **级联型**用于爆发性的导航事件——建造时抑制，导航时立即播报

---

## 3. 合并机制（Coalescing）

```
┌──────────────┐      ┌──────────────┐      ┌──────────────┐
│ Speak("A")   │      │ Speak("B")   │      │ Speak("C")   │
│ Tolk 忙      │      │ Tolk 忙      │      │ Tolk 空闲    │
│ pending = A  │  →   │ pending = B  │  →   │ Output("B")  │
└──────────────┘      └──────────────┘      └──────────────┘
```

单槽 pending：新消息到来时若 Tolk 正忙则**覆盖**旧 pending。只关心最新的。

```cpp
void Cue(const std::wstring& text) {
    if (tolk.IsSpeaking()) {
        m_pending = text;        // 覆盖
        m_hasPending = true;
    } else {
        tolk.Output(text, false);
    }
}
```

`Tick()` 每帧检查：Tolk 空闲且无级联活跃 → 刷新 pending。

---

## 4. 级联门控（Cascade Gating）—— 核心创新

### 问题

打开设置面板时，Palworld 一次性构建所有 5 个标签面板，每个面板的每一行都触发焦点事件。用户应只听到**最后一个**焦点项，而不是全部。

### 时序

```
t=0     NotifyScreenArrival()    ← 级联开始
t=50    FocusUpdate("Graphics")  → cascade, store pending = "Graphics"
t=80    FocusUpdate("Window")    → cascade, overwrite pending = "Window"
t=120   FocusUpdate("Fullscreen")→ cascade, overwrite pending = "Fullscreen"
t=350   FocusUpdate("VSync")     → cascade, overwrite pending = "VSync"
t=380   FocusUpdate("On")        → cascade, overwrite pending = "On"
...
t=750   Tick()                   → cascade 结束, flush pending = "On"
```

### 两条规则

```
级联活跃条件（满足任一）：
  1. sinceArrival < ScreenArrivalGraceMillis (500ms)     ← 初始宽限期
  2. sinceLastCascadeFocus < FocusCascadeDebounceMillis (200ms)  ← 动态延长
```

**一旦焦点事件停止到达超过 200ms，且初始 500ms 宽限期已过**，级联就被视为结束——然后才刷新最后一个 pending。

### 关键洞察

这是一个**不设固定超时**的延迟过滤机制。只要焦点事件持续以 <200ms 间隔到达，级联就保持活跃。这对处理速度不同的 UI 构建序列是健壮的。

---

## 5. 双重去重模式（Pointer + Message）

Speech 层提供基于文本的去重，但 **Hooks 层**需要额外的、基于指针的去重来覆盖 Speech 无法处理的场景：

### 层次 1：指针去重（~400ms）

```
同一 Context 指针 → 多个 UE 事件在 ~10ms 内触发 → 只为该指针播报一次
```

捕捉事件爆发——`AnmEvent_Focus`、`BP_OnHovered`、`AnmEvent_Hover` 三连击共享同一个 Context。

### 层次 2：消息去重（~1.2s）

```
不同 Context 指针 → 但提取出相同文本 → 抑制重复播报
```

用户在清单中箭移，经过多个存放相同物品的槽位时，只播报一次。

### 实现模式

```cpp
void HandleInventorySlotFocus(Context) {
    // Layer 1: pointer dedup
    if (Context == g_last_slot && now - g_last_slot_t < 400ms) return;
    g_last_slot = Context;

    auto msg = BuildMessage(Context);  // e.g. "Red Berries, 1"

    // Layer 2: message dedup (only when name resolved)
    if (!name.empty() && msg == g_last_msg && now - g_last_msg_t < 1200ms) return;
    g_last_msg = msg;

    Speech::FocusUpdate(msg);
}
```

### 何时使用哪一层

| 场景 | 指针去重 | 消息去重 |
|------|---------|---------|
| 同一槽位上的多事件爆发（3 个 UE 事件） | ✓ 必需 | 可选（消息相同） |
| 多个槽位存放相同物品 | ✗ 无法处理 | ✓ 必需 |
| 从另一个槽位回来后重新访问同一槽位 | ✗ 会错误抑制 | ✓ 1200ms 窗口过期后自然重新播报 |

---

## 6. 面板构建去重（Cluster Debounce）

设置面板是最复杂的情况：Palworld 构建 5 个标签面板（Graphics、Audio、Controls、Key bindings、Other）作为兄弟控件。每个都触发 `Construct` + `OnSetup`。

```cpp
// 每个类 600ms：过滤 Construct + OnSetup 双重触发
if (same_class_fired < 600ms ago) return;

// 全局 500ms 集群：过滤兄弟面板同时构建
if (any_panel_announced < 500ms ago) return;

// Okay, announce this one
Speech::Announce("Graphics settings");
```

**效果：** 用户听到一个面板名称（活跃标签页），而不是 5 个。

---

## 7. 综合架构：两个去重层

```
                    原始 UE 事件流
                          │
                          ▼
    ╔══════════════════════════════════════════╗
    ║  Hooks 层（上下文感知去重）              ║
    ║  - 指针去重 (same widget, multi-fire)   ║
    ║  - 消息去重 (different widget, same text)║
    ║  - 集群去重 (panel cluster construction)║
    ║  - 每类去重 (Construct+OnSetup pair)    ║
    ╚══════════════════════════════════════════╝
                          │
                          ▼
    ╔══════════════════════════════════════════╗
    ║  Speech 层（通用文本去重）               ║
    ║  - 精确匹配 + 时间窗口                   ║
    ║  - 最近 N 条缓冲区                       ║
    ║  - 级联门控                             ║
    ║  - 合并/非合并路径                       ║
    ╚══════════════════════════════════════════╝
                          │
                          ▼
                     TolkBridge
```

**核心原则：** Hooks 层理解游戏语义（"这是一个菜单，这些事件属于同一个用户操作"），Speech 层只关心文本流（"这个词刚刚说过，不要重复"）。职责分离使得两个层都保持简洁。

---

## 8. 可调参数指南

| 参数 | 含义 | 建议范围 | 过大的后果 |
|------|------|---------|-----------|
| 指针去重窗口 | 同一控件的事件的爆发等待时间 | 200–800ms | 合法导航被抑制 |
| 消息去重窗口 | 相同文本被视为重复的时间 | 800ms–2.5s | 用户等待太久才会重新播报 |
| 集群去重窗口 | 兄弟面板同时构建的最大间隔 | 300–600ms | 多个面板名称被不必要地播报 |
| 级联初始宽限期 | 面板到达后抑制的固定窗口 | 300–600ms | 面板标题播报延迟 |
| 级联延长 | 每次焦点事件后延长级联的时间 | 100–250ms | 用户导航操作在打开面板后被抑制 |
| 最近缓冲区大小 | Speak 历史去重的容量 | 20–50 | 用户循环浏览后无法重新播报 |
| 精确匹配去重 | 相同文本的硬抑制窗口 | 100–300ms | 合法重复被抑制 |

---

## 9. 移植到新 Mod 的建议

1. **从两种策略开始：** 合并型（`Speak`）用于状态，保序型（`Announce`）用于通知。`FocusUpdate` 可以稍后添加，当导航反馈感觉太慢或太嘈杂时。

2. **先实现 Speech 层，再添加 Hooks 去重：** Speech 的通用去重默认就能处理 ~60% 的冗余。只在你实际观察到重复播报的具体 UI 场景中添加 Hooks 级别的去重。

3. **记录日志以辅助调优：** 在去重抑制时记录 `[dedup] skipped "..." `。部署到实际玩家处，收集日志，根据真实使用场景调整时间窗口。

4. **优先使用 Speech 层去重而非 Hooks 去重，** 除非你明确需要指针去重或集群去重。通用解决方案随着更多游戏状态的处理而扩展；每个 Hook 的解决方案则会累积成维护负担。

5. **级联门控是最具复用价值的模式。** 任何需要从 UI 控件树中提取焦点事件的无障碍 mod 都将面临相同的问题。这个两阶段门控（初始宽限期 + 动态延长）在很大程度上独立于游戏引擎。
