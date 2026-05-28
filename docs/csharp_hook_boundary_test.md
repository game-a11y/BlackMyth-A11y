# C# Hook 边界测试

测试 C# Harmony 在《黑神话：悟空》USharp 框架下的有效范围。

## 测试环境

| 项目 | 值 |
|---|---|
| 游戏版本 | 1.0.21.23831 |
| 构建时间 | 2025-12-30 17:15:29 |
| 游戏分支 | b1_release (P4 87037) |
| 运行模式 | b1-Win64-Shipping / Environment: dev |
| C# Loader | B1CSharpLoader |
| Harmony 版本 | 0Harmony (随 B1CSharpLoader 分发) |
| 测试 Mod | WkAccess / HookTest.cs |

## 测试方法

两种注册方式并行测试：

1. **`[HarmonyPatch]` 属性 + `harmony.PatchAll()`** — 与 BattleLog、PlayerStatus 等社区参考 mod 完全一致
2. **手动 `harmony.Patch()` + `AccessTools`** — 用于更精确的错误处理

每个目标方法注册一个无参的 `Prefix` handler，触发时输出 `[HookTest] #N 标签名`。

## 测试目标

共 12 个目标，按类别分组：

| 类别 | 目标方法 | 注册方式 |
|---|---|---|
| Ctrl (正向对照) | `BGG_GameModeB1.ReceiveTick_Implementation` | 手动 |
| Ctrl | `BGGameStateCS.OnTickDispatchEventCS_Implementation` | 手动 |
| ECS | `BGS_AudioSystem.OnTickWithGroup` | 手动 |
| ECS | `BGS_DSDebugSystem.OnTickWithGroup` | 手动 |
| ECS | `BGS_AudioSystem.OnTickWithGroup` | `[HarmonyPatch]` 属性 |
| Widget | `UUserWidget.Tick_Implementation` | 手动 |
| Invoker | `BUI_Widget.Tick__Invoker` | 手动 |
| Invoker | `BUI_Widget.OnKeyUp__Invoker` | 手动 |
| Invoker | `BUI_Button.OnMouseButtonDown__Invoker` | 手动 |
| Invoker | `BUI_Button.OnAddedToFocusPath__Invoker` | 手动 |
| Invoker | `BUI_Button.OnKeyUp__Invoker` | 手动 |
| UI (B1UI 加载后) | `UIStartGame.OnClickTryChangeValue(int,int)` | 手动 + AssemblyLoad |

未通过测试（方法不存在于运行时）：
- `UUserWidget.OnAddedToFocusPath__Invoker`
- `UIStartGame.OnAddedToFocusPath_Implementation`

## 测试结果

**所有成功注册的 11 个补丁在运行中均未触发。** 无论注册方式（属性/手动）、目标方法类别（ECS/Invoker/Widget/Controller）、调用来源（C#/C++），结果一致。

| 测试阶段 | 注册 | 触发 | 结论 |
|---|---|---|---|
| 主菜单 (Startup_V2_P) | ✅ 全部成功 | ❌ 零触发 | 主菜单不实例化 GameMode/ECS 系统 |
| 进入游戏 (HFS01_PersistentLevel) | ✅ | ❌ 零触发 | 首次测试时 EnableJit 未启用 |

## 关键发现

### 第二轮精确测试（EnableJit=1 确认后）

在 `b1cs.ini` 中确认 `enableJit: 1` 后重新测试，**所有补丁正常工作**：

```
Ref-1 GameInstanceInit              ← 游戏启动时触发
Invoker.Btn.MouseDown               ← 主菜单按钮点击
UI.OnClickTryChangeValue            ← 点击所有主菜单按钮
Ctrl.GameModeTick                   ← 进入游戏后每帧约 1 次
Ref-2 PlayerInit                    ← 玩家角色创建时触发
ECS.Audio                           ← 游戏中每帧触发
Ref-5 DamageMult                    ← 受击时触发
```

| 测试类别 | 结果 |
|---|---|
| `[HarmonyPatch]` 属性 (`PatchAll()`) | ✅ 触发 |
| 手动 `harmony.Patch()` + `AccessTools` | ✅ 触发 |
| `_Implementation` 方法（UFunction） | ✅ 触发 |
| `__Invoker` 静态方法 | ✅ 触发 |
| 非 UFunction 方法 (C# delegate 调用) | ✅ 触发 |

### Harmony 工作条件

**EnableJit 是必要条件。** B1CSharpLoader 0.0.7 版本将 mono 从解释执行模式改为 JIT 模式，使 Harmony 的 IL 注入生效。确认 `b1cs.ini` 中 `EnableJit=true`。

## 可行方案

| 手段 | 状态 | 要求 |
|---|---|---|
| `BGW_EventCollection` 事件 | **可用** | 无 |
| 定时器轮询 | **可用** | 无 |
| `GSG.GSPageOP` 页面查询 | **可用** | 无 |
| USharp API 直接调用 | **可用** | 无 |
| `BGW_GameLifeTimeMgr` FSM 查询 | **可用** | 无 |
| **Harmony 补丁** | **可用** | `b1cs.ini` 中 `EnableJit=true` |
| **`[HarmonyPatch]` 属性** | **可用** | 同上 |
| **手动 `harmony.Patch()`** | **可用** | 同上 |
| **`__Invoker` 静态方法** | **可用** | 同上 |

## 相关代码

- 测试类: `csharp/WkAccess/A11y/HookTest.cs`
- Mod 入口: `csharp/WkAccess/ModMain.cs`
- 社区参考 mod: `csharp/.cyhan/MyBlackMythWukongMods-master/csharp/`
  - BattleLog/Program.cs — 使用 `[HarmonyPatch(typeof(BGUFunctionLibraryCS), nameof(BGUFunctionLibraryCS.LogBattleInfo))]`
  - PlayerStatus/Program.cs — 使用 `[HarmonyPatch(typeof(BGUPlayerCharacterCS), nameof(BGUPlayerCharacterCS.AfterInitAllComp))]`
  - RealDamageNumber/Program.cs — 使用 `[HarmonyPatch(typeof(BUI_MSimNum), nameof(BUI_MSimNum.SetDamageNumParam))]`

## 后续建议

1. 清理 `HookTest.cs` 相关代码（测试已完成）
2. 基于 `BGW_EventCollection` 事件 + 定时器轮询 实现无障碍功能
3. 如需更精细的 UI 交互检测，可尝试钩挂 C++ 层函数（通过 UE4SS 而非 C# Harmony）
