# Lua (UE4SS) vs C# (WkAccess) 模组功能对比分析

> 日期：2026-05-30

## 架构差异

| | Lua (UE4SS) | C# (WkAccess) |
|---|---|---|
| **挂钩方式** | `RegisterHook` + `NotifyOnNewObject` | Harmony Patch + `AccessTools.FieldRefAccess` |
| **文本提取** | 直接 `WidgetTree:GetChildAt(n)` 硬遍历 | `GSUIUtil.FindChildWidget` + 反射 |
| **TTS 朗读** | 调用 C++ `A11yTolk:Speak()` — **实际可用** | `A11yTolk.Speak()` 仅写日志 — **占位未实现** |
| **物品名解析** | 读材质纹理名（不可靠） | 读 `GameDBRuntime` / `GSG` 游戏数据（可靠） |
| **代码组织** | 扁平 `require()` + 全局表 | 命名空间、接口、Harmony 补丁 |

---

## 功能矩阵

### C# 有、Lua 无

| 功能 | 文件 | 说明 |
|---|---|---|
| 场景状态机 | `SceneMonitor.cs` | 订阅 18 个游戏事件 + FSM 轮询，检测 主菜单/游戏中/加载/启动/登录/暂停/着色器编译 |
| 可交互物品检测 | `InteractMonitor.cs` | Hook `TickForInteractiveActor`，检测最佳交互目标，播报类别+动作 |
| 装备名数据解析 | `B1WidgetResolvers.cs` | 从 `GameDBRuntime` 和 `GSG` 读取真实物品名，绕过 UI 时序问题 |
| 文件日志系统 | `A11yLog.cs` | UTF-8、5MB 自动截断、彩色分级、会话标记 |
| 完整构建信息 | `DebugCommands.cs` | Git commit、分支、P4 版本、引擎构建信息 |
| UI 页面缓存 | `B1PageCache.cs` | 缓存 6 个全局唯一页面引用（`EquipMain`、`BagMain`、`TalentMain`、`LearnTalent`、`TravelNotesMain`、`RoleMain`） |
| CleanEquipName 清洗 | `B1WidgetResolvers.cs` | 占位符过滤（`名字名字`）+ `ToFText` 本地化 + 标签剥离 |
| 快捷物品名解析 | `B1WidgetResolvers.cs` | 从 `GSG.GamePlayer.Actor.Wear.ShortcutsList` 读取物品名 |
| 暂停检测 | `SceneMonitor.cs` | `Evt_SetGamePause` 事件 |
| 玩家控制器生命周期 | `SceneMonitor.cs` | BeginPlay / EndPlay / PostLogin / DelayBeginPlayFinished |
| UI 页面轮询兜底 | `SceneMonitor.Polling.cs` | 每 3 秒定时器验证活跃页面 |

### Lua 有、C# 无

| 功能 | Lua 文件 | 严重程度 |
|---|---|---|
| **真实 TTS 朗读** | 通过 C++ `A11yTolk:Speak()` | **阻断** — 无此则完全不可替代 |
| `BI_AccordionChildBtn_Echo_C`（小曲菜单） | `HomeScreen.lua:199` | 中 — 主菜单音乐页 |
| `BI_TravelNotesMain_Tab_C`（游记标签） | `TravelNotes.lua` | 中 — 影神图/妙诀标签切换 |
| `BI_TravelNotesMain_ListBar_C`（游记列表项） | `TravelNotes.lua` | 低 — Lua 中也标注了 DevNote |
| 性能测试报告（F12） | `BmTools.lua` | 低 — 仅开发调试用 |
| 无障碍提示（A11yNote） | `SettingMenu.lua`（多处） | 中 — 对盲人用户的额外操作指导 |
| 滑块最大值朗读 | `SettingMenu.lua:251-261` | 低 — C# 只读当前值 |
| `BI_SettingIconItem_C` 特殊分支 | `SettingMenu.lua:300-306` | 低 — 键盘布局/亮度调整快捷入口 |
| `BI_SettingKeyItem_C` 按键名读取尝试 | `SettingMenu.lua:364-382` | 低 — 两者均未完成 |
| `BUI_InitSetting_C` 首次启动处理 | `SettingMenu.lua` | 低 — 首次设置引导 |
| 纹理回退物品检测 | `EquipItem.lua:16-40` | 低 — C# 数据驱动方案更优 |
| `DumpInfo()` 控件树探索工具 | `WkKeyBind.lua:24-72` | 低 — 开发工具，已注释 |
| `CallCppModTest()` | `WkKeyBind.lua:76-85` | 低 — 开发工具，已注释 |

### 两者持平

| 功能 | Lua 位置 | C# 位置 |
|---|---|---|
| UI 文本提取器注册表 | `WkGlobals.lua` 中 `GetTextFuncMap` | `UIScreenTextProvider.cs` 中 `_providers` |
| 焦点进入事件 Hook | `BUI_Button:OnAddedToFocusPath` | `H_FocusEnter.Postfix` + `UIFocusTracker` |
| 焦点离开事件 | （未挂钩） | `H_FocusLeave.Postfix` + `UIFocusTracker` |
| 鼠标点击日志 | `BUI_Button:OnMouseButtonDown` | `H_MouseDown.Postfix` |
| 按键绑定 | `RegisterKeyBind` | `Utils.RegisterKeyBind` |
| 确认对话框（BI_ReconfirmBtn_C） | `WkUIHook.lua:77-99` | `B1WidgetExtractors.cs` 中 `Extract_ReconfirmBtn` |
| 设置菜单 8 种控件类型 | `SettingMenu.lua` | `B1WidgetExtractors.cs` |
| 修行/技能提取器（KB/GP 系列） | `Ability.lua` + LeftRoot 导航 | `B1WidgetExtractors.cs` + LeftRoot 导航 |
| 土地庙菜单 | `Tudi.lua` | `Extract_ShrineMenu` |
| 游戏版本信息 | `WkUtils.PrintGameVersion()` | `DebugCommands.PrintGameBuildInfo()` |
| UI 页面信息 | `WkUtils.PrintUIPage()` | `SceneMonitor.PrintSummary()` |
| 玩家信息查询 | （无） | `DebugCommands.PrintPlayerInfo()` |
| 控件构造追踪 | `NotifyOnNewObject` + `WkGlobals.UIGlobals` | `B1PageCache._pageCache` |
| 行囊物品 | `InventoryItem.lua` | `Extract_InventoryItem` |
| 披挂装备槽 | `EquipItem.lua` | `Extract_EquipItem` |
| 珍玩槽位 | `EquipItem.lua` | `Extract_GearItem` |
| 随身之物槽位 | `EquipItem.lua` | `Extract_QuickItem` |
| 交互图标 | `InteractIcon.lua`（仅 TODO） | `Extract_Interact` |
| 技能面板标题按钮 | `Ability.lua:115-149`（部分完成） | `Extract_SpellPanelTitle` |
| 天赋技能项 | `Ability.lua:169-172` | `Extract_TalentItem` |

---

## 控件文本提取器详细对比

| 控件类型 | Lua | C# | 备注 |
|---|---|---|---|
| `BI_StartGame_C` | WidgetTree 遍历 | WidgetTree 遍历 | 持平 |
| `BI_FirstStartBtn_C` | WidgetTree 遍历 | `FindAnyText` | 持平 |
| `BI_ArchivesBtnV2_C` | WidgetTree 遍历（详细） | WidgetTree 遍历（简洁） | Lua 读取更多字段 |
| `BI_AccordionChildBtn_Echo_C` | 复用 `BI_StartGame_C` | **缺失** | — |
| `BI_SettingTab_C` | 有 | 有 | 持平 |
| `BI_SettingFixedItem_C` | 有 + A11yNote | 有 | Lua 有无障碍提示 |
| `BI_SettingMenuItem_C` | 有 + A11yNote | 有 | Lua 有无障碍提示 |
| `BI_ModeBtnItem_C` | 有 | 有 | 持平 |
| `BI_SettingSliderItem_C` | 有（值/最大值） | 有（仅值） | Lua 读取最大值 |
| `BI_SettingIconItem_C` | 有 + 特殊分支 | 有 | Lua 有 2 个特殊分支 |
| `BI_SettingMainBtn_C` | 有 | 有 | 持平 |
| `BI_SettingMenuBtn_C` | （通过 MainBtn） | 有 | 持平 |
| `BI_SettingKeyItem_C` | 有（固定 "W"） | 有（标记 `[??]`） | 两者均未完成 |
| `BI_ShrineMenuParent_C` | 复用 `BI_StartGame_C` | 通用 `FindAnyText` | 结果等价 |
| `BI_ShrineMenuChild_C` | 复用 `BI_StartGame_C` | 通用 `FindAnyText` | 结果等价 |
| `BI_AbilityIcon_KB_Basic_C` | LeftRoot 导航 | LeftRoot 导航 | 持平 |
| `BI_AbilityIcon_KB_Advance_C` | LeftRoot 导航 | LeftRoot 导航 | 持平 |
| `BI_AbilityIcon_GP_Basic_C` | 返回"根基" | 返回"根基" | 持平 |
| `BI_AbilityIcon_GP_Advance_C` | 返回"棍法" | 返回"棍法" | 持平 |
| `BI_SpellPanelTitle_Btn_C` | 部分完成（WIP） | 通用 `FindAnyText` | Lua 尝试更详细的描述 |
| `BI_TalentItem_*` | "根基技能" | "根基技能 Lv.{limit}" | C# 读取等级限制 |
| `BI_InventoryItem_C` | 纹理检测 | 通用查找 | 两者均未解析物品名 |
| `BI_EquipItem_Slot_C` | 纹理检测 | **数据驱动（GameDB）** | C# 更优 |
| `BI_GearItem_Slot_C` | 纹理检测 | **数据驱动（GameDB）** | C# 更优 |
| `BI_QuickItem_C` | 纹理检测 | **数据驱动（GSG）** | C# 更优 |
| `BI_InteractIcon` | **仅 TODO** | 有 | C# 已实现 |
| `BI_ReconfirmBtn_C` | 有 | 有 | 持平 |
| `BI_TravelNotesMain_Tab_C` | 有 | **缺失** | — |
| `BI_TravelNotesMain_ListBar_C` | 有（DevNote） | **缺失** | — |

---

## 结论：C# 目前不能替代 Lua

### 三个阻断性缺失

1. **TTS 未接入。** `A11yTolk.Speak()` 仅写日志，不朗读。C++ 插件通过 Tolk 库提供真实 TTS，Lua 直接调用。C# 需要 P/Invoke 调用 Tolk.dll 或通过 C++ 插件桥接。这是最大阻碍——没有 TTS 就没有屏幕阅读器。

2. **缺失 3 个控件提取器：**
   - `BI_AccordionChildBtn_Echo_C` — 主菜单小曲（音乐）按钮
   - `BI_TravelNotesMain_Tab_C` — 游记标签切换（影神图/妙诀）
   - `BI_TravelNotesMain_ListBar_C` — 游记二级列表项

3. **缺失无障碍提示语。** Lua 在多个设置处理器中有 `A11yNote` 字符串，向盲人用户解释特定操作方式（如"手柄按 A 确定，键盘按 E 确定"、"输入类型用于切换键位布局，目前不支持自定义手柄键位"）。这些是实际使用中积累的经验，C# 完全没有。

### C# 优于 Lua 之处

- **场景检测** — 完整状态机 + 18 个游戏事件 + 轮询兜底；Lua 完全没有场景感知
- **交互物品检测** — Lua 的 `InteractIcon.lua` 全是 TODO；C# 已完整实现
- **装备名称解析** — C# 从 GameDB 读取真实名称；Lua 读纹理材质名（本质上是 hack）
- **架构** — Harmony 补丁 + 事件驱动比扁平的 `RegisterHook` 更模块化、更易测试
- **日志** — 完整文件日志，含分级、截断、UTF-8 支持

### 建议替代路线

1. **TTS 接入**（P0）— P/Invoke 调用 Tolk.dll 或通过 C++ 插件桥接
2. **补全 3 个缺失提取器**（P1）— 工作量小，参照 `B1WidgetExtractors.cs` 中已有模式
3. **移植 A11yNote 提示语**（P1）— 从 Lua 设置处理器迁移无障碍指导文本
4. **补全滑块最大值**（P2）— `Extract_SettingSliderItem` 中 1-2 行改动
5. **可选移植 BmTools**（P3）— 性能测试报告朗读；可延后

完成步骤 1-4 后，C# 即可全面替代 Lua，并在场景感知和物品名称解析上超越 Lua 的能力。
