# 功能列表与测试要点

## 概述

Mod 分三层实现：
- **C# (WkAccess)** — 场景检测、UI 交互 Hook、按钮文本提取
- **Lua (UE4SS)** — UI 交互 Hook、按钮文本提取（逐步迁移到 C#）
- **C++ (UE4SS 插件)** — Tolk 屏幕朗读引擎

---

## 一、场景检测 (C#, SceneDetector)

### 功能
检测游戏所处的场景阶段，通过 `BGW_EventCollection` 事件 + 定时器轮询。

### 可检测的场景

| 场景 | 判断依据 | 对应日志 |
|---|---|---|
| 游戏启动/加载中 | FSM 状态 / PreLoadMap 事件 | `场景切换: Unknown/Startup → Loading` |
| 主菜单 | `IsInFSMState(MainMenu)` | `场景切换: Loading → MainMenu` |
| 游戏中 | `IsInFSMState(InBattleStandAlone)` | `场景切换: Loading → InGame` |
| 登录界面 | `IsInFSMState(WXLogin)` / 页面检测 | `UI打开: LoginNotice` |
| 着色器编译 | 页面检测 | `UI打开: ShaderCompiling` |

### 可检测的 UI 页面 (94 种 EnPageID)
包括：StartGame(主菜单)、BattleMainCon(战斗HUD)、Setting(设置)、ShrineMain(土地庙)、BagMain(背包)、EquipMain(装备)、Death(死亡)、EndCredits(制作人员)、**所有弹窗/子页面**等。

### 快捷键
`Ctrl + D1` → 打印当前场景状态摘要

### 测试确认
- [ ] 从启动到主菜单，场景切换日志是否正确
- [ ] 进入游戏后是否正确识别 InGame
- [ ] 打开各种菜单时是否有对应的 `UI打开` 日志
- [ ] Ctrl+D1 能否正确显示当前状态

---

## 二、UI 焦点追踪 (C#, UIFocusTracker + Harmony)

### 功能
当鼠标悬浮或键盘/手柄导航到按钮时，读取按钮文字并通过 A11yTolk 输出。

### 工作原理
Harmony 补丁挂钩 `BUI_Button.OnAddedToFocusPath_Implementation`，在按钮获得焦点时触发。通过 `UIScreenTextProvider` 读取按钮显示的文字。

### 已覆盖的 UI 控件

| 控件类型 | 预期输出示例 |
|---|---|
| 主菜单按钮 | `继续游戏`、`新游戏`、`载入游戏`、`设置`、`退出` |
| 设置标签页 | `控制器`、`键盘与鼠标`、`游戏`、`视角`、`语言`、`显示`、`画质`、`声音`、`辅助设置`、`退出游戏` |
| 设置开关 | `开启`、`关闭` |
| 设置下拉项 | 当前选中值 |
| 设置滑块 | 当前数值 |
| 确认对话框 | `确定`、`取消` |
| 土地庙菜单 | 菜单项名称 |
| 技能面板 | 技能名称 |
| 确认对话框按钮 | `确定`、`取消` |

### 日志输出格式
```
[UI.Focus] 聚焦 继续游戏 (GSID=20)
[TTS] 继续游戏 [打断]
[UI.Focus] 失焦 WidgetID=20 (BUI_Button)
[UI.Click] 点击 WidgetID=28
[UI.KeyUp] 按键 WidgetID=70
```

### 测试确认
- [ ] 鼠标悬浮到主菜单各按钮，是否能读出正确的按钮名（继续游戏、新游戏、载入游戏、小曲、设置、退出）
- [ ] 进入设置页，各标签页能否读出
- [ ] 设置页内的复选框/滑块/下拉菜单，能否读取当前值
- [ ] 确认对话框的确定/取消能否读出
- [ ] 土地庙菜单能否读出各选项
- [ ] 键盘/手柄导航时是否能正确读出焦点按钮
- [ ] `[UI.Click]` 是否能记录每一次按钮点击
- [ ] `[UI.KeyUp]` 是否能记录键盘操作

---

## 三、UI 鼠标交互追踪 (C#, UIMouseTracker + Harmony)

### 功能
记录按钮的鼠标点击和键盘按键操作。

### 工作原理
Harmony 补丁挂钩 `BUI_Button.OnMouseButtonDown_Implementation` 和 `BUI_Button.OnKeyUp_Implementation`。

### 测试确认
- [ ] 点击主菜单按钮 → 输出 `[UI.Click]`
- [ ] 键盘/手柄导航时操作按钮 → 输出 `[UI.KeyUp]`

---

## 四、场景检测辅助 (C#, SceneDetector + BGW_EventCollection)

### 可订阅的事件（供未来功能使用）

| 事件 | 触发时机 | 用途 |
|---|---|---|
| `PreLoadMap` | 关卡加载前 | 检测即将进入新场景 |
| `PostLoadMapWithWorld` | 关卡加载完成 | 世界加载结束 |
| `OnCurrentLevelChanged` | 当前关卡 ID 变化 | 关卡切换 |
| `LoadingBeginFadeAway` | 加载画面淡出 | 加载结束 |
| `OpenLevelFinished` | OpenLevel 完成 | 关卡打开 |
| `LeavingMap` | 离开地图 | 离开当前区域 |
| `TeleportFinished` | 传送完成 | 传送结束 |
| `PlayerPostLogin` | 玩家登录 | 玩家就绪 |
| `PlayerControllerBeginPlay` | 玩家控制器启动 | 控制器就绪 |
| `UIActived` | UI 页面打开/关闭 | 检测页面变化 |
| `SetGamePause` | 游戏暂停/恢复 | 暂停状态 |

---

## 五、屏幕朗读 (C++ Tolk 模块 + A11yTolk 占位)

### 当前实现 (C# 占位)
`A11yTolk.Speak(text)` 将朗读文本输出到日志，格式为 `[TTS] 文字内容`。

### 预期实现 (C++ Tolk 模块)
通过 UE4SS C++ 插件调用 Tolk 库，实现真正的 Windows 屏幕朗读器输出（读屏软件如 NVDA、讲述人等）。

### 测试确认
- [ ] 焦点到按钮时，日志是否输出 `[TTS] 按钮文字`
- [ ] `Silence()` 是否输出 `[TTS] [静音]`

---

## 六、快捷键

| 快捷键 | 功能 | 实现 |
|---|---|---|
| `Ctrl + Enter` | 打印玩家 HP 信息 | C# (DebugCommands) |
| `Ctrl + D1` | 打印当前场景状态摘要 | C# (SceneDetector) |
| `Ctrl + F5` | 重新加载 C# Mod | C# Loader 内置 |

---

## 七、已知限制

| 限制 | 说明 |
|---|---|
| 按钮文本提取依赖控件名称 | `GSUIUtil.FindChildWidget` 通过名称搜索，部分嵌套复杂的控件可能无法正确找到文字 |
| 设置项控件名/类型/值区分 | 当前只能读取到值（如"开启"），无法同时读取标签名（如"控制器类型"）和控件类型（"左右单项选择"） |
| 需要 `EnableJit=1` | Harmony 补丁需要 JIT 模式，`b1cs.ini` 中需配置 |
| Tolk 尚未接入 | 当前 `A11yTolk.Speak` 只输出日志，不调用读屏软件 |

---

## 八、快速测试流程

1. 确认 `CSharpLoader/b1cs.ini` 中 `EnableJit=1`
2. 启动游戏，观察控制台窗口日志
3. 依次操作：进入主菜单 → 查看各按钮 → 进入设置页 → 查看各设置项 → 加载存档进入游戏
4. 在控制台日志中检查 `[UI.Focus]`、`[UI.Click]`、`[SceneDetector]` 等输出
5. 按 `Ctrl + D1` 查看当前场景状态摘要
