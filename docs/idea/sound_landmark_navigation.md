# 地图区块无障碍导航 — 声音地标方案

> 为视障玩家提供地图空间感知能力。传统寻路导航实现复杂且容易出错，改为在关键地点设置"声音地标"——玩家到达特定区域时播放标志性音效/TTS 提示，玩家自行通过这些声音线索构建脑海中的空间地图。

## 核心思路

在地图的关键位置（桥梁、BOSS 区域、重要路口、地区入口、祭坛/神庙等）定义"声音地标区域"。当玩家进入该区域时，触发音频提示。不同地标类型使用不同的声音/TTS 内容，玩家通过反复听到这些声音来建立空间记忆。

### 优势

- **实现简单**：不需要寻路算法，只需位置检测 + 音频触发
- **不干扰玩法**：玩家仍自由移动，声音是辅助信息
- **渐进式**：可以先从几个关键地标开始，逐步扩展
- **容错性高**：漏检/误检不会导致玩家卡住
- **心智负担低**：玩家被动接收信息，不需要主动操作

### 劣势与注意事项

- 地标坐标需要针对每个地图手动配置（或从游戏数据导出）
- 玩家需要时间学习各种声音的含义
- 需要区分"首次到达"vs"反复经过"（避免骚扰）
- 开放世界区域可能需要更密集的地标

---

## 可行性分析（基于代码调研）

### 已有的基础设施

| 能力 | 位置 | 状态 |
|---|---|---|
| 获取玩家世界坐标 | `WkUtils.GetControlledPawn()?.GetActorLocation()` → `FVector` | 可直接使用 |
| 获取当前地图名/关卡ID | `GameState.CurrentMapName` / `CurrentLevelId` | 已有，通过 `SceneMonitor` 事件驱动更新 |
| TTS 语音输出 | `A11yTolk.Speak(text, interrupt)` | 接口就绪，当前只打 log |
| 定时轮询 | `SceneMonitor` 使用 `Timer` 每 2 秒轮询 | 模式可复用 |
| 按键绑定 | `Utils.RegisterKeyBind(modifiers, key, action)` | 模式成熟 |
| 调试命令 | `DebugCommands` 静态方法 | 模式成熟 |
| 游戏地图符号数据 | `FUStMapSymbolDesc`（含坐标、类型）、`DSMapSymbol` | 可从游戏 DB 读取 |

### 游戏侧可用的地图/位置系统

游戏有完善的地图数据系统，可以直接复用以减少手动配置：

1. **`FUStMapSymbolDesc`** — 地图符号配置（Protobuf 表），包含：
   - `SymbolPosX/Y/Z` — 世界坐标
   - `Type` (`EMapSymbolType`) — 类型枚举：`Stupa`(佛塔)、`GodTower`、`CaveEntrance`(洞穴入口)、`Challenge`、`StrongHold`、`Npc`、`RebirthPoint`(重生点)、`MirageWorld`
   - `Name` / `LocalizationTag` — 名称/本地化标签
   - `UnlockRadius` / `UnlockHeight` — 解锁范围

2. **`FUStRebirthPointDesc`** — 重生点配置，包含坐标、`GroupAreaID`、`CanTeleport`

3. **`BPS_MapAreaSystem`** — 玩家地图区域追踪系统，提供：
   - `TryGetMapAreaInfoByPlayer()` → `{LevelId, AreaId, MapLayer, Location}`
   - `TryGetMapPositionByPlayer()` → `{MapSpriteId, MapPosition, MapRotation}`

4. **`BPC_GeoInfoData`** — 已探索地理信息数据

5. **`BGUFuncLibMap`** — 地图函数库：`GetCurLevelId()`、`GetMapName()`、`GetAreaId(Actor)`

---

## 架构设计

### 新增文件

```
csharp/WkAccess/
├── A11y/
│   ├── UI/
│   └── Navigation/                    # 新增导航命名空间
│       ├── SoundLandmark.cs           # 单个声音地标的数据定义
│       ├── SoundLandmarkDB.cs         # 地标数据库（按地图索引的地标列表）
│       ├── LandmarkDetector.cs        # 位置检测器（轮询 + 触发判断）
│       └── LandmarkConfig.cs          # 地标配置（坐标来源：手动 or 游戏DB）
```

### 数据模型

```csharp
// SoundLandmark.cs — 单个声音地标
public class SoundLandmark
{
    public string Id;                    // 唯一标识
    public string MapName;               // 所属地图（对应 GameState.CurrentMapName）
    public int LevelId;                  // 所属关卡（可选，更精确）
    public FVector Center;               // 中心点世界坐标
    public float Radius;                 // 触发半径
    public string Label;                 // TTS 播报文本，如 "前方石桥"
    public SoundLandmarkType Type;       // 地标类型（决定音效/TTS 策略）
    public float CooldownSeconds;        // 冷却时间（防止频繁触发）
}

public enum SoundLandmarkType
{
    Bridge,         // 桥梁
    BossArea,       // BOSS 区域
    Intersection,   // 路口
    ZoneEntrance,   // 地区入口
    Shrine,         // 祭坛/神庙
    Cave,           // 洞穴入口
    Npc,            // 重要 NPC
    PointOfInterest // 其他兴趣点
}
```

### 检测流程

```
SceneMonitor 定时器 (每 500ms-1s)
  → 检查 GameState.IsInGame
  → 获取当前地图名 → 查找该地图的地标列表
  → 获取玩家世界坐标 FVector
  → 遍历地标列表，计算距离
  → 距离 < Radius 且 冷却已过 → 触发
      → A11yTolk.Speak(landmark.Label, interrupt: false)
      → 记录触发时间
```

### 触发策略

| 策略 | 描述 |
|---|---|
| **首次触发** | 每个地标只触发一次（玩家经过后不再提示） |
| **冷却触发** | 每次触发后有 N 秒冷却，冷却后可再次触发 |
| **接近梯度** | 远距离偶尔提示方向，近距离详细描述（暂不实现，留作扩展） |

推荐默认使用 **冷却触发**（冷却时间 ~30s），玩家反复经过同一地点时可再次确认位置。

### 调试支持

- 新增快捷键 `Ctrl + D2` → `DebugCommands.PrintLandmarkInfo` — 打印当前玩家坐标、所在地图、附近地标及距离
- 新增快捷键 `Ctrl + D3` → 在地标数据库中热重载配置（开发期使用）
- 日志输出：进入/离开地标区域时写入 `A11yLog.Info`

---

## 阶段规划

### Phase 1：基础框架（最小可行）

1. 创建 `SoundLandmark` 数据类 + `SoundLandmarkType` 枚举
2. 创建 `LandmarkDetector` — 定时轮询 + 距离检测 + 冷却管理
3. 创建 `LandmarkDB` — 硬编码 2-3 个测试地标（在当前可访问的地图上选点）
4. 在 `ModMain.Init()` 中启动检测器
5. 新增调试命令 `PrintLandmarkInfo` + 快捷键

### Phase 2：数据扩充

1. 从游戏 Protobuf DB 读取 `FUStMapSymbolDesc` 自动生成地标
2. 将 `EMapSymbolType` 映射到 `SoundLandmarkType`
3. 支持 JSON 配置文件补充手动标注的地标
4. 区分"重要地标"和"普通地标"（重要地标的触发策略不同）

### Phase 3：音频增强

1. 接入真实 TTS（替换 `A11yTolk` 的 log 实现）
2. 不同地标类型使用不同提示音效前缀
3. 空间音频（3D 定位音效，如声音从地标方向传来）— 需评估 UE4 音频 API 可行性

### Phase 4：高级特性（远期）

1. 方向提示："前方有一座桥" — 需要基于玩家朝向计算
2. 距离梯度："你正在接近一座桥"vs"桥上"
3. 路径片段：连续经过多个地标时播报路径摘要
4. 与地图 UI 的语音描述配合

---

## 待讨论的问题

1. **坐标来源优先级**：优先从游戏 DB（`FUStMapSymbolDesc`）自动生成，还是优先手动配置？游戏 DB 数据可能包含很多"不重要"的符号，需要筛选。
2. **检测频率**：500ms vs 1s vs 2s？频率越高越精准但性能开销越大。
3. **地标粒度**：一个中等大小的地图大概需要多少个声音地标？（估计 20-50 个关键点）
4. **TTS vs 音效**：是用 TTS 播报文字（如"石桥"），还是用不同的非语言音效代表不同类型？TTS 信息量大但侵入性强，音效轻量但需要学习。
5. **与现有读屏的关系**：`A11yTolk.Speak()` 已被 UI 焦点追踪使用（`interrupt: true`），地标播报应使用 `interrupt: false`（队列播报），避免打断 UI 操作。

---

## 验证方式

1. 启动游戏，进入任意可探索地图
2. 按下 `Ctrl + D2`，确认日志输出当前坐标和附近地标
3. 走向一个测试地标的坐标范围，确认触发 TTS 播报
4. 再次经过同一地标，确认冷却机制生效
5. 切换地图，确认地标列表正确切换
