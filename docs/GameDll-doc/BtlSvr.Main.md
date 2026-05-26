# BtlSvr.Main 类导航

## 总体结构

**程序集名称**: BtlSvr.Main  
**程序集版本**: 1.0.0.0  
**源码规模**: 10834 个 .cs 文件，77 个子目录  
**命名空间体系**: 顶级（全局命名空间）、`b1.*`、`CommB1`、`GSDispLib`、`OssB1`、`HelloUSharp`、`STUN.*`、`System.*`、`UnrealEngine.*`、`Microsoft.*`

**整体定位**:  
此 DLL 是《黑神话：悟空》游戏逻辑的核心 C# 实现，承载于 **USharp** 框架之上，运行在 Unreal Engine 5 环境中。它通过 ILRuntime 实现热更新脚本能力，通过 Google Protobuf 实现网络通信序列化，通过 ECS 架构管理游戏实体。

**核心技术栈**:
- **USharp** — 将 C# 类型绑定到 UE5 原生类型（Actor、Component、GameMode、GameState 等），通过 `[UClass]`、`[UProperty]`、`[UFunction]` 等 Attribute 标记与蓝图互通
- **ILRuntime** — 提供 CLR 交叉绑定适配器（`CrossBindingAdaptor`），使热更新脚本可继承 UE5 原生类型
- **Google Protobuf** — 网络消息的序列化/反序列化（`OssB1` 命名空间）
- **Calliope** — 可视化行为树/状态机编辑器系统（`b1.Editor.Calliope`）
- **GSMUI** — 自研 UI 动画/补间系统
- **STUN** — NAT 穿透库（用于 P2P 联机）
- **ECS** — 轻量级实体组件系统

---

## 命名空间清单

| 命名空间 | 位置 | 规模 |
|---------|------|------|
| *(全局)* | 顶级目录 | 16 文件 |
| `b1` | `b1/` | 5990 文件（含子目录） |
| `b1.EventDelDefine` | `b1/EventDelDefine/` | 2953 个委托定义 |
| `b1.AutoQA` | `b1/AutoQA/` | 311 个自动化测试类 |
| `b1.UI` | `b1/UI/` | 196 |
| `b1.UI.Comm` | `b1/UI/Comm/` | 97 |
| `b1.GSMUICore.Event` | `b1/GSMUICore/Event/` | 23 个 UI 动效类 |
| `b1.GSMUI.GSWidget` | `b1/GSMUI/GSWidget/` | 14 个自定义控件 |
| `b1.BGU.BUAnim` | `b1/BGU/BUAnim/` | 39 个动画蓝图 |
| `b1.BGW` | `b1/BGW/` | 29 个世界管理器 |
| `b1.BGW.EnvQuery` | `b1/BGW/EnvQuery/` | 26 个环境查询 |
| `b1.Render` | `b1/Render/` | 10 |
| `b1.Render.Core` | `b1/Render/Core/` | 3 渲染管线 |
| `b1.Render.TressFX` | `b1/Render/TressFX/` | 11 毛发渲染 |
| `b1.Render.LandscapeBpBrush` | `b1/Render/LandscapeBpBrush/` | 5 地形笔刷 |
| `b1.Util` | `b1/Util/` | 11 工具类 |
| `b1.ECS.Test` | `b1/ECS/Test/` | 4 ECS 测试 |
| `b1.Editor` | `b1/Editor/` | 2 编辑器工具 |
| `b1.Editor.Calliope.Behavior.Nodes` | `b1/Editor/Calliope/Behavior/Nodes/` | 2 行为树节点 |
| `b1.GameMode` | `b1/GameMode/` | 3 GameMode |
| `b1.GameState` | `b1/GameState/` | 1 |
| `b1.GameState.Data` | `b1/GameState/Data/` | 4 游戏状态数据 |
| `b1.GSReplicate` | `b1/GSReplicate/` | 1 网络复制 |
| `b1.GSUI` | `b1/GSUI/` | 1 鼠标输入 |
| `CommB1` | `CommB1/` | 538 个只读数据包装类 |
| `GSDispLib` | `GSDispLib/` | 312 个显示/特效通知类 |
| `OssB1` | `OssB1/` | 132 个 Protobuf 消息类型 |
| `HelloUSharp` | `HelloUSharp/` | 7 个 HelloWorld 示例 |
| `STUN.*` | `STUN/` | 39 个 STUN 协议类 |
| `System.*` | `System/` | 7 系统扩展 |
| `UnrealEngine.Engine` | `UnrealEngine/Engine/` | 1 引擎扩展 |
| `B1UI.GSUI` | `B1UI/GSUI/` | 5 UI 相关 |
| `b1.GSMUI` | `b1/GSMUI/` | 5 GSMUI 基类 |
| `b1.GSMUI.GSMisc` | `b1/GSMUI/GSMisc/` | 1 |
| `b1.GSMUI.GSView` | `b1/GSMUI/GSView/` | 1 |
| `b1.GSMUI.Core` | `b1/GSMUI/Core/` | 1 |
| `ILRuntime.Runtime.Generated` | `ILRuntime/Runtime/Generated/` | 1 CLR 绑定 |
| `GSE` | `GSE/` | 1 服务器状态 |
| `Microsoft.CodeAnalysis` | `Microsoft/CodeAnalysis/` | 1 嵌入式属性 |

---

## 模块结构（按目录）

### 1. 顶级目录 — ILRuntime 适配器与测试工具 (16 文件)

全局命名空间，主要是 ILRuntime 交叉绑定适配器、崩溃测试、以及 FSM 注册。

| 类型名称 | 类型 | 继承/接口 | 说明 | 文件 |
|---------|------|-----------|------|------|
| `AdaptHelper` | 静态类 | - | ILRuntime 适配辅助类，提供 `IMethod` 查找与缓存 | `AdaptHelper.cs` |
| `AdaptHelper.AdaptMethod` | 嵌套类 | - | 方法名+参数数量的适配方法描述 | `AdaptHelper.cs` |
| `MyAdaptor` | 抽象类 | `CrossBindingAdaptorType` | ILRuntime 交叉绑定适配器基类，提供 `Invoke` 机制 | `MyAdaptor.cs` |
| `Adapt_Exception` | 类 | `CrossBindingAdaptor` | Exception 的 ILRuntime 适配器 | `Adapt_Exception.cs` |
| `Adapt_Exception.Adaptor` | 嵌套类 | `Exception, CrossBindingAdaptorType` | Exception 适配实现 | `Adapt_Exception.cs` |
| `Adapt_IMessage` | 类 | `CrossBindingAdaptor` | Google Protobuf IMessage 的 ILRuntime 适配器 | `Adapt_IMessage.cs` |
| `Adapt_IMessage.Adaptor` | 嵌套类 | `MyAdaptor, IMessage` | 使热更脚本可实现 IMessage 接口 | `Adapt_IMessage.cs` |
| `Adapt_UUserWidget` | 类 | `CrossBindingAdaptor` | UMG UUserWidget 的 ILRuntime 适配器 | `Adapt_UUserWidget.cs` |
| `Adapt_UUserWidget.Adaptor` | 嵌套类 | `UUserWidget, CrossBindingAdaptorType` | 使热更脚本可继承 UUserWidget | `Adapt_UUserWidget.cs` |
| `Adapt_CrashTest` | 类 | `CrossBindingAdaptor` | CrashTest 的 ILRuntime 适配器 | `Adapt_CrashTest.cs` |
| `Adapt_CrashTest.Adaptor` | 嵌套类 | `CrashTest, CrossBindingAdaptorType` | 使热更脚本可用 CrashTest | `Adapt_CrashTest.cs` |
| `CFSMGReg` | 类 | - | **FSM 状态与条件注册中心**，集中注册 GI_Global / GI_Loading / PS_Transaction 的所有状态和条件节点 | `CFSMGReg.cs` |
| `SerializedManagedUnrealModuleInfo` | 类 | `ISerializedManagedUnrealModuleInfo` | 序列化后的 UE 模块元信息，包含所有注册类型的 GUID 映射 | `SerializedManagedUnrealModuleInfo.cs` |
| `CrashTest` | 类 | - | 崩溃测试基类 | `CrashTest.cs` |
| `BUC_PerformerControlData` | 类 | - | 表演者控制数据，包含参数列表、阶段列表、阶段索引控制 | `BUC_PerformerControlData.cs` |
| `TweenTxtBlockValue` | 类 | `GSMUIEventBase` | UI TextBlock 数值补间动画组件 | `TweenTxtBlockValue.cs` |
| `TestState_ATPAllBossFPSTest` | 类 | - | FPS 测试状态 | `TestState_ATPAllBossFPSTest.cs` |
| `TestState_CrossLevel_QuickTeleport` | 类 | - | 跨关卡快速传送测试状态 | `TestState_CrossLevel_QuickTeleport.cs` |
| `TestState_RecordFPS2File` | 类 | - | FPS 记录到文件测试状态 | `TestState_RecordFPS2File.cs` |
| `TestState_RecordFPStart` | 类 | - | FPS 记录开始测试状态 | `TestState_RecordFPStart.cs` |
| `CrossLevel_AutoTest_BossFPSTest` | 类 | - | 跨关卡 BOSS FPS 自动化测试 | `CrossLevel_AutoTest_BossFPSTest.cs` |

---

### 2. `b1/` — 核心游戏逻辑命名空间 (5990 文件)

这是最大的模块，包含游戏的核心业务逻辑。以下分类介绍：

#### 2.1 Actor/Component 系统

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `UActorCompBaseCS` | 类 | C# Actor Component 基类 |
| `UActorCompBaseUObj` | 类 | C# Actor Component UObject 基类 |
| `UActorCompContainerCS` | 类 | Actor Component 容器 |
| `UActorEditCompBase` | 类 | Actor 编辑组件基类 |
| `ActorCompBaseShareUtil` | 类 | Actor Component 共享工具 |
| `ActorCompNetRole` | 枚举 | 网络角色（Authority/Simulated/Autonomous） |
| `ActorECSSnapShotData` | 类 | Actor ECS 快照数据 |
| `ActorTransformSnapShotData` | 类 | Actor 变换快照数据 |
| `Type_CheckCompProfileName_Bullet` | 类 | 子弹类型检查组件配置 |
| `Type_CheckCompProfileName_MagicField` | 类 | 魔法场类型检查组件配置 |
| `AlwaysCantMoveActorInit` | 类 | 始终不可移动的 Actor 初始化 |
| `ActorCellPartitionFilter` | 类 | Actor 网格分区过滤器 |
| `UForceCinfigComp` | 类 | 力配置组件 |

#### 2.2 AI 系统 (大量文件)

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `AIDataBase` | 类 | AI 数据基类 |
| `AIData_BasicTimers` | 类 | AI 基础计时器数据 |
| `AIData_ComboInfo` | 类 | AI 连招信息数据 |
| `AIData_FsmContext` | 类 | AI FSM 上下文数据 |
| `AIData_Memory` | 类 | AI 记忆数据 |
| `AIDataPkg` | 类 | AI 数据包 |
| `AIDataUtil` | 类 | AI 数据工具类 |
| `AIElement` | 类 | AI 元素基类 |
| `AIActionElem` | 类 | AI 动作元素 |
| `AIActionProcessState` | 类 | AI 动作处理状态 |
| `AIActionState` | 枚举 | AI 动作状态 |
| `AIActionVertifyResult` | 类 | AI 动作验证结果 |
| `AIFeatureElem` | 类 | AI 特征元素 |
| `AIFeatureState` | 枚举 | AI 特征状态 |
| `AIFeatureTestInfo` | 类 | AI 特征测试信息 |
| `AIFilterProcess` | 类 | AI 过滤处理 |
| `AIFilterResult` | 类 | AI 过滤结果 |
| `AIFuncLibForCS` | 类 | AI C# 函数库 |
| `AIGOAPFuncLibCS` | 类 | AI GOAP（目标导向行动规划）函数库 |
| `AIThinkElem` | 类 | AI 思考元素 |
| `AICharacterData` | 类 | AI 角色数据 |
| `AIRequestBase` | 类 | AI 请求基类 |
| `AIReqDirDamage` | 类 | AI 定向伤害请求 |
| `AIReqMoveSkill` | 类 | AI 移动技能请求 |
| `AIReqMoveToActor` | 类 | AI 移动到目标请求 |
| `AIReqMoveToLoc` | 类 | AI 移动到位置请求 |
| `AISkillDynamicFeature` | 类 | AI 技能动态特征 |
| `AISkillInfo` | 类 | AI 技能信息 |
| `AINodeAction_AdjustTransformBySplineParamInfo` | 类 | AI 沿样条调整变换参数 |
| `AINodeAction_ComboParamInfo` | 类 | AI 连招参数信息 |
| `AINodeAction_EQSRunParamInfo` | 类 | AI EQS 运行参数信息 |
| `AINodeAction_GroupAIMove2EnterBattlePos` | 类 | AI 组移动至战斗位置 |
| `AINodeAction_GroupAIMove2HotZonePointParamInfo` | 类 | AI 组移动至热区参数 |
| `AINodeAction_MoveToParamInfo` | 类 | AI 移动到参数信息 |
| `AINodeAction_SpiderMoveToParamInfo` | 类 | AI 蜘蛛移动到参数信息 |
| `AINodeFinishState` | 枚举 | AI 节点完成状态 |
| `AIPointData` | 类 | AI 点数据 |
| `AIPointTestInfo` | 类 | AI 点测试信息 |

#### 2.3 战斗/单位系统

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `AFAttackableEnemy` | 类 | 可攻击敌人查找 |
| `AFNearestEnemy` | 类 | 最近敌人查找 |
| `AFSkillArea` | 类 | 技能区域查找 |
| `AFStandInSkillArea` | 类 | 技能区域站位 |
| `AffectCaster` | 类 | 影响施法者查找 |
| `AffectTarget` | 类 | 影响目标查找 |
| `UnitAttrHelper` | 类 | 单位属性辅助 |
| `UnitBarInfo` | 类 | 单位血条信息 |
| `UnitCompAddRule` | 类 | 单位组件添加规则 |
| `UnitHatredTargetInfo` | 类 | 单位仇恨目标信息 |
| `UnitLockTargetInfo` | 类 | 单位锁定目标信息 |
| `UnitLockTargetInfoSnapShot` | 类 | 单位锁定目标快照 |
| `UnitTeamForData` | 类 | 单位队伍数据 |
| `UnitTopBarOneBind` | 类 | 单位顶部血条绑定 |
| `UnitZBBInfo` | 类 | 单位 ZBB 信息 |
| `TurnSkillType` | 枚举 | 转身技能类型 |
| `TransDmgStruct` | 结构体 | 传递伤害结构 |
| `TraceMoveMode` | 枚举 | 追踪移动模式 |
| `TransStateInfo` | 类 | 变换状态信息 |
| `TransitionGuard` | 类 | 过渡守卫 |
| `TroDistanceByIntervalConfig` | 类 | 间隔距离配置 |
| `TroDistanceByIntervalHelper` | 类 | 间隔距离辅助 |

#### 2.4 存档系统

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `ArchiveAsyncRequest` | 类 | 异步存档请求 |
| `ArchiveFileUnpacked` | 类 | 解包存档文件 |
| `ArchiveFileUtil` | 类 | 存档文件工具 |
| `ArchiveLock` | 类 | 存档锁 |
| `ArchiveMgrRunningState` | 枚举 | 存档管理器运行状态 |
| `ArchiveOSS` | 类 | OSS 存档 |
| `ArchiveProtoVersionCheck` | 类 | 存档协议版本检查 |
| `ArchiveSaveRequestOne` | 类 | 存档保存请求单例 |

#### 2.5 Performer/表演系统

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `UPerformerActionBase` | 类 | 表演动作基类 |
| `UPerformerAction_AIConversation` | 类 | AI 对话表演动作 |
| `UPerformerAction_AutoTurn2Target` | 类 | 自动转向目标表演动作 |
| `UPerformerAction_FadeIn` | 类 | 淡入表演动作 |
| `UPerformerAction_FadeOut` | 类 | 淡出表演动作 |
| `UPerformerAction_PlayBeginLoopMontage` | 类 | 播放起始循环蒙太奇表演动作 |
| `UPerformerAction_PlayMontage` | 类 | 播放蒙太奇表演动作 |
| `UPerformerAction_SwitchEyeAimOffsetIndex` | 类 | 切换眼部瞄准偏移索引 |
| `UPerformerAction_SwitchHeadAimOffsetIndex` | 类 | 切换头部瞄准偏移索引 |
| `UPerformerParamBase` | 类 | 表演参数基类 |
| `UPerformerParam_Overlap` | 类 | 重叠表演参数 |
| `UPerformerParam_Performer` | 类 | 表演者参数 |
| `UPerformerPhase` | 类 | 表演阶段 |

#### 2.6 动画通知

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `UAnimNotify_GSFootStep` | 类 | 脚步动画通知基类 |
| `UAnimNotify_GSFootStep_FootL` | 类 | 左脚脚步动画通知 |
| `UAnimNotify_GSFootStep_FootR` | 类 | 右脚脚步动画通知 |
| `UAnimNotifyState_GSFootSlide` | 类 | 脚步滑动动画通知状态 |

#### 2.7 FSM 适配基类

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `Adapt_FSMConditionBase` | 类 | FSM 条件的 ILRuntime 适配基类 |
| `Adapt_FSMState_GI_GlobalBase` | 类 | GI_Global FSM 状态适配基类 |
| `Adapt_FSMState_GI_LoadingBase` | 类 | GI_Loading FSM 状态适配基类 |
| `Adapt_TravelLevelTemplateBase` | 类 | 关卡传送模板适配基类 |
| `Adapt_IAutoSizeItem` | 类 | 自动尺寸项适配器 |
| `Adapt_IGSMUIDestruct` | 类 | GSMUI 析构接口适配器 |
| `Adapt_IGSMUITickable` | 类 | GSMUI 可 Tick 接口适配器 |
| `Adapt_BGU_LeakLogUtil` | 类 | BGU 泄漏日志工具适配器 |

#### 2.8 其他 b1 重要类型

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `FSMRuntimeInstance_GI_Global` | 类 (从属) | GI_Global 的 FSM 运行时实例 |
| `FSMState_GI_Global_*` | 多个类 | GI_Global 的各种 FSM 状态（WaitGameStart/MainMenu/InBattle等） |
| `FSMCondition_GI_Global_*` | 多个类 | GI_Global 的各种 FSM 条件 |
| `FSMState_GI_Loading_*` | 多个类 | GI_Loading 的各种 FSM 状态 |
| `FSMCondition_GI_Loading_*` | 多个类 | GI_Loading 的各种 FSM 条件 |
| `FSMState_PS_Transaction_TransactionTask` | 类 | PS_Transaction 的交易任务状态 |
| `FSMCondition_PS_Transaction_TaskCondition` | 类 | PS_Transaction 的任务条件 |
| `UserSettingData` | 类 | 用户设置数据 |
| `UIAssetLoadHelper` | 类 | UI 资源加载辅助 |
| `UIBindData` | 类 | UI 绑定数据 |
| `UIDataTracker` | 类 | UI 数据追踪 |
| `UIEvt_VoidWidget` | 类 | UI 空控件事件 |
| `UIEvt_VoidWidgetInt` | 类 | UI 空控件整型事件 |
| `UIMgrBlockHelper` | 类 | UI 管理器阻塞辅助 |
| `UILRuntimeDelegateWrapperBase` | 类 | ILRuntime 委托包装基类 |
| `UGSInputSettingsPreProcEvent` | 类 | 输入设置预处理事件 |
| `UGSInputWidgetCS` | 类 | 输入控件 |
| `UGSKeyEvent` | 类 | 按键事件 |
| `UGSOverlayCS` | 类 | 叠加层控件 |
| `UGSReplayCSharpFuncLibCS` | 类 | 回放功能 C# 库 |
| `UGSSdkHttpRequestCallbackListener` | 类 | SDK HTTP 请求回调监听 |
| `UGSSuperArmorDescCustomizationHelper` | 类 | 超级护甲描述自定义辅助 |
| `UInputActionEventReceiver` | 类 | 输入动作事件接收器 |
| `UInputPreProcEvent` | 类 | 输入预处理事件 |
| `UBGWDropItemTemplete` | 类 | 掉落物品模板 |
| `UBGWFunctionLibraryCS` | 类 | BGW 功能库 |
| `UBGWImmobilizeConfig` | 类 | 定身配置 |
| `UBGWTestTaskAnim` | 类 | BGW 测试任务动画 |
| `UDSSettingFunctionBinder` | 类 | DS 设置功能绑定器 |
| `UGConfig` | 类 | G 配置 |
| `UPlayerTransactionEventCollection` | 类 | 玩家交易事件集合 |
| `UPostProcessMatInfo` | 类 | 后处理材质信息 |
| `UnrealDBMap` | 类 | 虚幻 DB 映射 |
| `UnTraceStrongPtr` | 结构体 | 不可追踪强指针 |
| `IKRigDefinition` | 类 | IK 骨骼定义 |
| `ZbbCricketData` | 类 | ZBB 蟋蟀数据 |
| `ZBBPreviewConfig` | 类 | ZBB 预览配置 |
| `X2ActionMappingDef` | 类 | X2 动作映射定义 |
| `WeGameAchievementEvent` | 类 | WeGame 成就事件 |
| `WeGameChannelSDK` | 类 | WeGame 渠道 SDK |
| `XSXChannelSDK` | 类 | XSX 渠道 SDK |
| `ActionNameFuncLib` | 类 | 动作名称函数库 |
| `ActionPreExeCache` | 类 | 动作预执行缓存 |
| `ActionProcessBase` | 类 | 动作处理基类 |
| `ActionWarpInfo` | 类 | 动作扭曲信息 |
| `ActionBinding1P` | 类 | 1P 动作绑定 |
| `ActionBinding2P` | 类 | 2P 动作绑定 |
| `AkUnitInfo` | 类 | Wwise 音频单位信息 |
| `AnimationSyncBuffDef` | 类 | 动画同步 Buff 定义 |
| `AbnormalStateAccConfig` | 类 | 异常状态累计配置 |
| `AbnormalStateGlobleParam` | 类 | 异常状态全局参数 |
| `AchievementEvent` | 类 | 成就事件 |
| `ActivityEvent` | 类 | 活动事件 |

---

### 3. `b1/EventDelDefine/` — 委托事件定义 (2953 文件)

**说明**: 这是一个纯委托定义的目录，包含约 2953 个 `delegate` 定义，命名为 `Del_*` 模式。这些委托用于解耦游戏各模块间的事件通信。

**典型模式**:
```csharp
namespace b1.EventDelDefine
{
    public delegate void Del_AAMotionMatchAssetPreloadRequire(FSoftObjectPath Path, Action<int, UObject> CB);
}
```

**代表性类型**:

| 委托名称 | 参数模式 | 说明 |
|---------|---------|------|
| `Del_AAMotionMatchAssetPreloadRequire` | `FSoftObjectPath, Action<int, UObject>` | 动作匹配资源预加载请求 |
| `Del_AbnormalRemoved` | 通用 | 异常状态移除 |
| `Del_ActivateTalent` | 通用 | 激活天赋 |
| `Del_ActionNodeFinish` | 通用 | 动作节点完成 |
| `Del_ActionTimeOut` | 通用 | 动作超时 |
| `Del_AddBuffNotify` | 通用 | 添加 Buff 通知 |
| `Del_AICastBestComboSkill` | 通用 | AI 施放最佳连招技能 |
| `Del_AICastBestComboSkillX2` | 通用 | AI 施放最佳连招技能 X2 |
| `Del_AICastBestSkillByScore` | 通用 | AI 按分数施放最佳技能 |
| `Del_AICatchTarget` | 通用 | AI 捕捉目标 |
| `Del_BuffRemove` | 通用 | Buff 移除 |
| `Del_Damage` | 通用 | 伤害事件 |
| `Del_Death` | 通用 | 死亡事件 |
| `Del_SkillCast` | 通用 | 技能施放事件 |
| `Del_LevelUp` | 通用 | 升级事件 |
| `Del_ItemPickUp` | 通用 | 物品拾取事件 |

**注意**: 该目录是自动生成的，涉及游戏所有模块的事件委托定义。

---

### 4. `b1/AutoQA/` — 自动化测试 (311 文件)

**说明**: 自动测试框架，关卡自动化遍历测试，属于 QA 自动化体系。以每个关卡或功能模块命名。

**命名空间**: `b1.AutoQA`

**代表性类型**:

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `AutoTest_AllUI` | 类 | 全 UI 自动化测试 |
| `AutoTest_BagUI` | 类 | 背包 UI 自动化测试 |
| `AutoTest_BattleTrace` | 类 | 战斗追踪测试 |
| `AutoTest_BattleWithUnit` | 类 | 单位战斗测试 |
| `AutoTest_BianShenCastAllSkill` | 类 | 变身全技能施放测试 |
| `AutoTest_BulletTrace` | 类 | 子弹追踪测试 |
| `AutoTest_BYS_Start` | 类 | 黑风山起点测试 |
| `AutoTest_BYS_Tanglang` | 类 | 黑风山-螳螂测试 |
| `AutoTest_BYS_Xiniu` | 类 | 黑风山-犀牛测试 |
| `AutoTest_BYS_Xuelu` | 类 | 黑风山-雪鹿测试 |
| `AutoTest_DingShenEffectCheckTool` | 类 | 定身效果检查工具 |
| `AutoTest_ENDA_ShiZhongJing` | 类 | 浮屠塔-石中镜测试 |
| `AutoTest_ENDB_Fate` | 类 | 浮屠塔下层-命运测试 |
| `AutoTest_EquipmentUI` | 类 | 装备 UI 测试 |
| `AutoTest_ExportInfo` | 类 | 导出信息测试 |
| `AutoTestHelperLib` | 类 | 自动化测试辅助库 |
| `AutoTest_HFM_Cave` | 类 | 黑风洞测试 |
| `AutoTest_HFM_FuZiShu` | 类 | 黑风林-父子树测试 |
| `AutoTest_HFM_HFDS` | 类 | 黑风林-黑风大仙测试 |
| `AutoTest_HFM_HuangCun` | 类 | 黑风林-黄村测试 |
| `AutoTest_HFM_HuStone` | 类 | 黑风林-虎石测试 |
| `AutoTest_HFM_SandSkiing` | 类 | 黑风林-滑沙测试 |
| `AutoTest_HFM_ShaMenGang` | 类 | 黑风林-沙门岗测试 |
| `AutoTest_HFM_ShiXianFeng` | 类 | 黑风林-石先锋测试 |
| `AutoTest_HFM_Start` | 类 | 黑风林起点测试 |
| `AutoTest_HFS_Bamboo` | 类 | 黄风岭-竹林测试 |
| `AutoTest_HFS_FirstBattle` | 类 | 黄风岭-首战测试 |
| `AutoTest_HFS_Forest` | 类 | 黄风岭-森林测试 |
| `AutoTest_HFS_GoUp` | 类 | 黄风岭-上山测试 |

---

### 5. `CommB1/` — 通信层数据包装 (538 文件)

**说明**: 游戏各种数据表的只读包装类，将 `ArchiveB1` 命名空间的 protobuf 数据封装为只读访问层。每个文件命名模式为 `ReadOnly*`。

**命名空间**: `CommB1`

**代表性类型**:

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `ReadOnlyAccessoryProp` | 类 | 饰品属性只读包装 |
| `ReadOnlyAccessoryPropList` | 类 | 饰品属性列表 |
| `ReadOnlyAchievementConfig` | 类 | 成就配置只读包装 |
| `ReadOnlyAchievementOne` | 类 | 单条成就只读 |
| `ReadOnlyAchievementStat` | 类 | 成就统计 |
| `ReadOnlyAchievementStatus` | 类 | 成就状态 |
| `ReadOnlyActorProgress` | 类 | Actor 进度只读 |
| `ReadOnlyActorWear` | 类 | Actor 装备只读 |
| `ReadOnlyAwardItem` | 类 | 奖励物品只读 |
| `ReadOnlyAttrItem` | 类 | 属性条目只读 |
| `ReadOnlyBagItem` | 类 | 背包物品只读 |
| `ReadOnlyBattleItem` | 类 | 战斗物品只读 |
| `ReadOnlyBossInfo` | 类 | BOSS 信息只读 |
| `ReadOnlyBuffConfig` | 类 | Buff 配置只读 |
| `ReadOnlyBulletConfig` | 类 | 子弹配置只读 |
| `ReadOnlyChapterInfo` | 类 | 章节信息只读 |
| `ReadOnlyChatMessage` | 类 | 聊天消息只读 |
| `ReadOnlyCheckpointInfo` | 类 | 存档点信息只读 |
| `ReadOnlyComboSkill` | 类 | 连招技能只读 |
| `ReadOnlyDamageData` | 类 | 伤害数据只读 |
| `ReadOnlyDialogue` | 类 | 对话只读 |
| `ReadOnlyDropItem` | 类 | 掉落物品只读 |
| `ReadOnlyEquipment` | 类 | 装备只读 |
| `ReadOnlyExpData` | 类 | 经验数据只读 |
| `ReadOnlyGameSetting` | 类 | 游戏设置只读 |
| `ReadOnlyGourdConfig` | 类 | 葫芦配置只读 |
| `ReadOnlyItem` | 类 | 物品只读 |
| `ReadOnlyLevelInfo` | 类 | 关卡信息只读 |
| `ReadOnlyMapInfo` | 类 | 地图信息只读 |
| `ReadOnlyMonsterInfo` | 类 | 怪物信息只读 |
| `ReadOnlyNPCData` | 类 | NPC 数据只读 |
| `ReadOnlyPlayerState` | 类 | 玩家状态只读 |
| `ReadOnlyQuestData` | 类 | 任务数据只读 |
| `ReadOnlySkillData` | 类 | 技能数据只读 |
| `ReadOnlyShopItem` | 类 | 商店物品只读 |
| `ReadOnlySpiritData` | 类 | 精魄数据只读 |
| `ReadOnlyTransformData` | 类 | 变身数据只读 |
| `ReadOnlyTalentData` | 类 | 天赋数据只读 |
| `ReadOnlyVitalityData` | 类 | 气血数据只读 |

**注意**: 该目录是自动生成的，包含约 538 个只读包装类，每个包装类对应一种数据表。

---

### 6. `GSDispLib/` — 显示/特效库 (312 文件)

**说明**: 显示特效库，包含动画通知（BAN/BANS 系列）、分散计算（DBC）效果、Niagara 粒子特效、相机震动、材质修改等显示相关的类型。

**命名空间**: `GSDispLib`

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `ANNiagaraData` | 类 | Niagara 粒子数据（NotifyID、组件、生命周期） |
| `AssetExportMode` | 枚举 | 资源导出模式 |
| `B2D_DispLibFXActorCameraShakeData` | 结构体 | FX Actor 相机震动数据 |
| `BAN_DispLibApplySceneInteractor` | 类 | 场景交互器应用动画通知 |
| `BAN_DispLibChangeUnitMaterial` | 类 | 改变单位材质动画通知 |
| `BAN_DispLibEndDBCEffects` | 类 | 结束 DBC 效果动画通知 |
| `BAN_DispLibModifyMaterial` | 类 | 修改材质动画通知 |
| `BAN_DispLibPlayCameraShake` | 类 | 播放相机震动动画通知 |
| `BAN_DispLibPlayCustomCameraShake` | 类 | 播放自定义相机震动动画通知 |
| `BAN_DispLibPlayDBCDataAsset` | 类 | 播放 DBC 数据资源动画通知 |
| `BAN_DispLibSimpleRibbonTrailsArray` | 类 | 简单飘带轨迹数组动画通知 |
| `BAN_DispLibUnitArtFresnel` | 类 | 单位美术菲涅尔效果动画通知 |
| `BAN_GSPlayNiagaraFX` | 类 | 播放 Niagara FX 动画通知 |
| `BAN_GSPlayNiagaraFX_WithCondition` | 类 | 带条件播放 Niagara FX 动画通知 |
| `BANS_DispLibApplyWindSource` | 类 | 应用风源动画通知状态 |
| `BANS_DispLibTimedModifyMaterial` | 类 | 定时修改材质动画通知状态 |
| `BANS_DispLibTimedModifyMPC` | 类 | 定时修改 MPC 动画通知状态 |
| `BANS_GSTimedPlayNiagaraFX` | 类 | 定时播放 Niagara FX 动画通知状态 |
| `BANS_GSTimedPlayNiagaraFX_WithCondition` | 类 | 带条件定时播放 Niagara FX 动画通知状态 |
| `BGU_DispLibDataUtil` | 静态类 | DBC 数据工具类，包含各种 DBC 数据获取方法 |
| `BGU_DispLibDBCCarrierActor` | 类 | DBC 载体 Actor |
| `BGU_DispLibDBCCarrierActorDataComp` | 类 | DBC 载体 Actor 数据组件 |
| `BGU_DispLibFXActorGSArtFresnelData` | 类 | FX Actor 美术菲涅尔数据 |
| `BGU_DispLibUComponentBase` | 类 | 显示库组件基类 |
| `BGW_DispLibCameraBlockDataAsset` | 类 | 相机阻挡数据资源 |
| `BGW_DispLibCameraEnvFXDataAsset` | 类 | 相机环境特效数据资源 |
| `BGW_DispLibConstDataAsset` | 类 | 显示库常量数据资源 |
| `BGW_DispLibDBCEditorDebugConfigDataAsset` | 类 | DBC 编辑器调试配置资源 |
| `BGW_DispLibFNameCacheDataAsset` | 类 | FName 缓存数据资源 |
| `BGW_DispLibGameDB` | 类 | 显示库游戏 DB |

**约定**:
- `BAN_*` = Bone Animation Notify (动画通知，单帧)
- `BANS_*` = Bone Animation Notify State (动画通知状态，持续)
- `BGU_*` = 游戏实用类
- `BGW_*` = 游戏世界配置资源

---

### 7. `OssB1/` — Protobuf 网络消息类型定义 (132 文件)

**说明**: Google Protobuf 生成的消息类型，用于客户端与服务器的网络通信。

**命名空间**: `OssB1`

**代表性类型**:

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `Accessory` | 类 (proto message) | 饰品消息 |
| `AccessorySlot` | 类 (proto message) | 饰品插槽消息 |
| `Attack` | 类 (proto message) | 攻击消息 |
| `BattleEndType` | 枚举 (proto enum) | 战斗结束类型 |
| `BattleEndTypeSyncWrapper` | 类 | 战斗结束类型同步包装 |
| `BattleFinishType` | 枚举 | 战斗完成类型 |
| `BattleMonster` | 类 | 战斗怪物数据 |
| `BattlePlayer` | 类 | 战斗玩家数据 |
| `ChangedMappableKey` | 类 | 已更改的可映射按键 |
| `ChapterPlayTime` | 类 | 章节游戏时间 |
| `CollectionStage` | 枚举 | 收集阶段 |
| `CollectionType` | 枚举 | 收集类型 |
| `CommValueChange` | 类 | 通用数值变化 |
| `CommValueChangeType` | 枚举 | 数值变化类型 |
| `DamageType` | 枚举 | 伤害类型 |
| `Defence` | 类 | 防御数据 |
| `Demo820LevelInfo` | 类 | 820 演示关卡信息 |
| `DeviceInfo` | 类 | 设备信息 |
| `DieType` | 枚举 | 死亡类型 |
| `EquipPosition` | 枚举 | 装备位置 |
| `GameKeyMapping` | 类 | 游戏键位映射 |
| `GSEquipment` | 类 | GS 装备数据 |
| `GSItem` | 类 | GS 物品数据 |
| `GSTalentInfo` | 类 | 天赋信息 |
| `GSTalentPage` | 类 | 天赋页面 |
| `HeroBaseAttr` | 类 | 英雄基础属性 |
| `HeroFightAttr` | 类 | 英雄战斗属性 |
| `InputMappingPreset` | 类 | 输入映射预设 |
| `ItemPosition` | 类 | 物品位置 |
| `LevelInfo` | 类 | 关卡信息 |
| `LoginInfo` | 类 | 登录信息 |
| `MapInfo` | 类 | 地图信息 |
| `MonsterRefreshInfo` | 类 | 怪物刷新信息 |
| `PlayerBaseInfo` | 类 | 玩家基础信息 |
| `PlayerLoginInfo` | 类 | 玩家登录信息 |
| `PositionAndRotation` | 类 | 位置旋转信息 |
| `QuestData` | 类 | 任务数据 |
| `SkillInfo` | 类 | 技能信息 |
| `StoreData` | 类 | 存储数据 |
| `TransformInfo` | 类 | 变身信息 |
| `VectorInfo` | 类 | 向量信息 |
| `WareHouseInfo` | 类 | 仓库信息 |

---

### 8. `b1/BGU/BUAnim/` — 动画蓝图 (39 文件)

**说明**: BGU 动画蓝图系统，负责角色动画的逻辑控制，包含姿态控制、IK、动作匹配等。

**命名空间**: `b1.BGU`

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `AbpHelperUtil` | 类 | 动画蓝图辅助工具 |
| `BGAnimDragon` | 类 | 龙类动画 |
| `BGAnimSpider` | 类 | 蜘蛛类动画 |
| `BUAnimationAnalyzer` | 类 | 动画分析器 |
| `BUAnimDataAsset8Dir` | 类 | 8 方向动画数据资源 |
| `BUAnimHumanoidCS` | 类 | 人形动画基类 |
| `BUAnimHumanoidCS_AdvancedMonsterLocomotion` | 类 | 高级怪物移动动画 |
| `BUAnimHumanoidCS_AnimCurveBodyBlend` | 类 | 动画曲线身体混合 |
| `BUAnimHumanoidCS_AnimCurveBodySeparation` | 类 | 动画曲线身体分离 |
| `BUAnimHumanoidCS_AttackIK` | 类 | 攻击 IK |
| `BUAnimHumanoidCS_BoneAim` | 类 | 骨骼瞄准 |
| `BUAnimHumanoidCS_CloudLocomotion` | 类 | 云移动动画 |
| `BUAnimHumanoidCS_FlyControl` | 类 | 飞行控制动画 |
| `BUAnimHumanoidCS_FootIK` | 类 | 脚步 IK |
| `BUAnimHumanoidCS_HandIK` | 类 | 手部 IK |
| `BUAnimHumanoidCS_LeftArmSeparation` | 类 | 左臂分离动画 |
| `BUAnimHumanoidCS_LinkedInstanceBase` | 类 | 链接实例基类 |
| `BUAnimHumanoidCS_MMRetarget` | 类 | 运动匹配重定向 |
| `BUAnimHumanoidCS_MonsterLocomotion` | 类 | 怪物移动动画 |
| `BUAnimHumanoidCS_MotionMatching` | 类 | 运动匹配 |
| `BUAnimHumanoidCS_Move` | 类 | 移动动画 |
| `BUAnimHumanoidCS_PlayerLocomotion` | 类 | 玩家移动动画 |
| `BUAnimHumanoidCS_QuadrupedIK` | 类 | 四足 IK |
| `BUAnimHumanoidCS_QuadrupedLocomotion` | 类 | 四足移动动画 |
| `BUAnimHumanoidCS_RightArmSeparation` | 类 | 右臂分离动画 |
| `BUAnimHumanoidCS_Simple4Dir` | 类 | 简单 4 方向动画 |
| `BUAnimHumanoidCS_SpecialMove` | 类 | 特殊移动动画 |
| `BUAnimHumanoidCS_UpperBodySeparation` | 类 | 上半身分离动画 |
| `BUAnimInsect` | 类 | 昆虫类动画 |
| `BUAnimInstanceBase` | 类 | 动画实例基类 |

---

### 9. `b1/BGW/` — 游戏世界管理器 (29 + 26 文件)

**说明**: BGW（Black Myth Game World）管理器系列，负责资源加载、相机适配、关卡流送等世界级管理功能。

**命名空间**: `b1.BGW`

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `BGWAssetLoaderRequestCS` | 类 | 资源加载请求 |
| `BGW_CameraAdapterMgr` | 类 | 相机适配管理器 |
| `BGW_CharacterViewMgr` | 类 | 角色视图管理器 |
| `BGW_DispInteractMgr` | 类 | 显示交互管理器 |
| `BGW_DynamicSDFMgr` | 类 | 动态 SDF 管理器 |
| `BGW_LevelStreamingManger` | 类 | 关卡流送管理器 |
| `BGW_PaintWorldMgr` | 类 | 世界绘制管理器 |
| `BGW_PreloadAssetMgr` | 类 | 预加载资产管理器 |
| `BGW_QuickCookGenerator` | 类 | 快速 Cook 生成器 |
| `CacheAssetReference` | 类 | 缓存资源引用 |
| `CharacterViewType` | 枚举 | 角色视图类型 |
| `DSDF_Solver` | 类 | 动态 SDF 求解器 |
| `EAssetPriority` | 枚举 | 资源优先级 |
| `ELoadResourceType` | 枚举 | 资源加载类型 |
| `EPreloadAssetSourceType` | 枚举 | 预加载资源来源类型 |
| `EPreloadPlayerAbilityType` | 枚举 | 预加载玩家能力类型 |
| `ESoakingCamera` | 枚举 | 浸泡相机类型 |
| `ETamerPreloadLevel` | 枚举 | Tamer 预加载级别 |
| `EUIResourceLoadType` | 枚举 | UI 资源加载类型 |
| `EUnitPreloadLevel` | 枚举 | 单位预加载级别 |
| `GSInCharacterViewCVarHelper` | 类 | 角色视图 CVar 辅助 |
| `ICallbackValidator` | 接口 | 回调验证器 |
| `NEW_SDFMgr` | 类 | 新 SDF 管理器 |
| `ObjectsLoadedCallBack` | 委托 | 对象加载回调 |
| `PreloadAssetHelper` | 类 | 预加载资源辅助 |
| `PreloadLevelConfig` | 类 | 预加载关卡配置 |
| `SDFMethod` | 枚举 | SDF 方法 |
| `SteepActorsData` | 类 | 陡峭 Actor 数据 |
| `UAsyncLoadAssetHolder` | 类 | 异步加载资源持有者 |

**b1/BGW/EnvQuery** — 环境查询系统 (26 文件)

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `FEnvOverlapByObjectTypeData` | 结构体 | 环境重叠查询数据 |
| `GSEQC_CachedEnterBattlePoint` | 类 | 缓存进入战斗点上下文 |
| `GSEQC_CachedGroupAIHotZonePoint` | 类 | 缓存组 AI 热区点上下文 |
| `GSEQC_CachedSceneItem` | 类 | 缓存场景物品上下文 |
| `GSEQC_CaptainContext` | 类 | 队长上下文 |
| `GSEQC_PlayerContext` | 类 | 玩家上下文 |
| `GSEQC_ProjectileContext` | 类 | 投射物上下文 |
| `GSEQC_QATargetLocationContext` | 类 | QA 目标位置上下文 |
| `GSEQC_QuerierNavProjectLocation` | 类 | 查询者导航投影位置 |
| `GSEQC_SkillBaseTargetContext` | 类 | 技能基础目标上下文 |
| `GSEQC_SummonContext` | 类 | 召唤上下文 |
| `GSEQC_TargetContext` | 类 | 目标上下文 |
| `GSEQC_TeamContext` | 类 | 队伍上下文 |
| `GSEQG_ActorsByTag` | 类 | 按标签查询 Actor |
| `GSEQG_CertainPointGenerator` | 类 | 确定点生成器 |
| `GSEQG_CircleAroundProjectile` | 类 | 投射物周围圆形查询 |
| `GSEQG_LandingPointGenerator` | 类 | 落点生成器 |
| `GSEQG_NeutralAnimalSpawnPoints` | 类 | 中立动物刷新点查询 |
| `GSEQG_PointsOnSphere` | 类 | 球面上的点查询 |
| `GSEQG_SphericalLineTracePointGenerator` | 类 | 球形线迹点生成器 |
| `GSEQG_SummonSpawnPointGenerator` | 类 | 召唤生成点生成器 |
| `GSEQG_WanderPointGenerator` | 类 | 游荡点生成器 |
| `GSEQT_CheckAngle` | 类 | 角度检查测试 |
| `GSEQT_OverlapByObjectType` | 类 | 按对象类型重叠测试 |
| `GSEQT_STByResID` | 类 | 按资源 ID 测试 |
| `GSEQT_STPriority` | 类 | 技能目标优先级测试 |

---

### 10. `b1/UI/` — UI 系统 (196 + 97 文件)

**说明**: 游戏 UI 层，包含大量自定义控件、UI 逻辑和 UI 管理器。

**命名空间**: `b1.UI`

**代表性 UI 控件** (`b1/UI/`):

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `AnimationInfo` | 类 | 动画信息 |
| `AnimationInfoWithRef` | 类 | 带引用动画信息 |
| `AnimationRestoreInfo` | 类 | 动画恢复信息 |
| `AnimFinishConfig` | 类 | 动画完成配置 |
| `AnimKeyStateBlendCfg` | 类 | 动画关键帧状态混合配置 |
| `AttrDisplayMode` | 枚举 | 属性显示模式 |
| `AttrSizeInGrid` | 类 | 网格中的属性尺寸 |
| `B1ActorTag` | 类 | Actor 标签 |
| `B1GSUIActorMgr` | 类 | GSUI Actor 管理器 |
| `BI_AbnormalStateAccBarCS` | 类 | 异常状态累积条 |
| `BI_AbnormalStateAccBoxCS` | 类 | 异常状态累积框 |
| `BI_AbnormalStateItemCS` | 类 | 异常状态项 |
| `BI_DebugTextInGrid` | 类 | 网格中的调试文本 |
| `BI_DropAdvanceCS` | 类 | 掉落进阶提示 |
| `BI_DropExpProgCS` | 类 | 掉落经验进度条 |
| `BI_DropExpProgV2CS` | 类 | 掉落经验进度条 V2 |
| `BI_DropItemCS` | 类 | 掉落物品提示 |
| `BI_DropManualCS` | 类 | 掉落手册提示 |
| `BI_DropMiddleItemCS` | 类 | 掉落中间物品提示 |
| `BI_DropMuseumCS` | 类 | 掉落博物馆提示 |
| `BI_DropSpecialTipsCS` | 类 | 掉落特殊提示 |
| `BI_DropSpiritCS` | 类 | 掉落精魄提示 |
| `BI_GourdCS` | 类 | 葫芦 UI |
| `BI_GourdSlotCS` | 类 | 葫芦插槽 UI |
| `BI_HpProgBarCS` | 类 | 血量进度条 |
| `BI_LockEnemyCS` | 类 | 锁定敌人 UI |
| `BI_NavigationCS` | 类 | 导航 UI |
| `BI_PlayerBarCS` | 类 | 玩家状态条 |
| `BI_PlayerStateBoxCS` | 类 | 玩家状态框 |
| `BI_PlayerStateItemCS` | 类 | 玩家状态项 |

**b1/UI/Comm** — 通用 UI 控件 (97 文件):

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `AlwaysHideSetting` | 类 | 总是隐藏设置 |
| `AlwaysShowSetting` | 类 | 总是显示设置 |
| `BarOffset` | 结构体 | 条偏移 |
| `BaskstabUIInfo` | 类 | 背刺 UI 信息 |
| `BUI_BarBase` | 类 | 条状 UI 基类 |
| `BUI_BarCSharp` | 类 | C# 实现的条状 UI |
| `BUI_BarFloat` | 类 | 浮动条 UI |
| `BUI_BarMatBase` | 类 | 材质条基类 |
| `BUI_BarTimeCount` | 类 | 计时条 UI |
| `BUI_BossBar` | 类 | Boss 血条 |
| `BUI_Button` | 类 | 按钮基类 |
| `BUI_ButtonCompare` | 类 | 比较按钮 |
| `BUI_ButtonNone` | 类 | 空按钮 |
| `BUI_ButtonSpecialNone` | 类 | 特殊空按钮 |
| `BUI_ButtonSpellItemV2` | 类 | 法术物品按钮 V2 |
| `BUI_ButtonTalentItemV2` | 类 | 天赋物品按钮 V2 |
| `BUI_ChapterRoam` | 类 | 章节漫游 UI |
| `BUI_Cursor` | 类 | 鼠标光标 |
| `BUI_CursorBase` | 类 | 鼠标光标基类 |
| `BUI_CursorMap` | 类 | 地图鼠标光标 |

**b1/UI/GMCommand** — GM 命令 UI:

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `BUI_GM_CalliopePanel` | 类 | Calliope 调试面板 |
| `BUI_GM_HatredAndTargetPanel` | 类 | 仇恨与目标调试面板 |

**b1/UI/GSPage** — 页面系统:

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `IPoolWidget` | 接口 | 对象池控件接口 |
| `IWidgetPoolManager` | 接口 | 控件池管理器接口 |

**b1/UI/Example**:

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `UI_CSharpBasic` | 类 | C# UI 基础示例 |

---

### 11. `b1/GSMUICore/Event/` — UI 动效系统 (23 文件)

**说明**: 自研 UI 动画系统核心，提供补间动画、延迟执行、序列等机制。

**命名空间**: `b1.GSMUICore.Event`

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `GSMEventAsyncQueue` | 类 | 异步事件队列 |
| `GSMEventWidgetQuickBezierTo` | 类 | 控件贝塞尔快速移动事件 |
| `GSMMathUtil` | 类 | GSM 数学工具 |
| `GSMSeqUtil` | 类 | 序列工具 |
| `GSMUIDelay` | 类 | UI 延迟 |
| `GSMUIDelayExec` | 类 | 延迟执行 |
| `GSMUIEventBase` | 类 | UI 事件基类 |
| `GSMUIEventExecFunc` | 类 | 事件执行函数 |
| `GSMUIEventSequence` | 类 | 事件序列 |
| `GSMUIEventStat` | 枚举 | 事件状态 |
| `GSMUITickableStat` | 枚举 | Tick 状态 |
| `GSMUITweenBarLength` | 类 | 条长度补间 |
| `GSMUITweenBarMatLineMarkScale` | 类 | 条材质线标记缩放补间 |
| `GSMUITweenBarMatPercent` | 类 | 条材质百分比补间 |
| `GSMUITweenBlink` | 类 | 闪烁补间 |
| `GSMUITweenFade` | 类 | 淡入淡出补间 |
| `GSMUITweenFloat` | 类 | 浮点数补间 |
| `GSMUITweenSetMatParam_Scalar` | 类 | 设置材质标量参数补间 |
| `GSMUITweenSetMPCParam` | 类 | 设置 MPC 参数补间 |
| `GSMUITweenWidgetMoveTo` | 类 | 控件移动补间 |

---

### 12. `b1/GSMUI/GSWidget/` — GSMUI 自定义控件 (14 文件)

**命名空间**: `b1.GSMUI`

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `EDefaulValueType` | 枚举 | 默认值类型 |
| `EIndicatorType` | 枚举 | 指示器类型 |
| `EWarnState` | 枚举 | 警告状态 |
| `EWarnStateBlock` | 枚举 | 警告状态块 |
| `FFreqConfig` | 结构体 | 频率配置 |
| `GSButton` | 类 | GS 按钮 |
| `GSButtonCheck` | 类 | GS 复选按钮 |
| `GSGridConScreenAdapter` | 类 | 网格容器屏幕适配器 |
| `GSProcBar` | 类 | 进度条 |
| `GSProcBarV4` | 类 | 进度条 V4 |
| `GSRetainerBox` | 类 | 保留盒控件 |
| `GSRoundRectCS` | 类 | 圆角矩形 |
| `IProcBar` | 接口 | 进度条接口 |
| `ProcBarSizeHelper` | 类 | 进度条尺寸辅助 |

**b1/GSMUI** 根文件:

| `GSMUITickMgr` | 类 | UI Tick 管理器 |
| `IGSMShowIn` | 接口 | GSM 显示接口 |
| `IGSMUIDestruct` | 接口 | GSM UI 析构接口 |
| `IGSMUITickable` | 接口 | GSM UI Tick 接口 |
| `IGSSetCommParams` | 接口 | 设置通用参数接口 |
| `GSMUIUtil` | 类 | GSM UI 工具类 |

**b1/GSMUI/Core**:

| 类型 | 说明 |
|------|------|
| GSMUI 核心实现（1 文件） |

**b1/GSMUI/GSMisc**:

| `GSTestRun` | 类 | 测试运行 |
| `BUI_GridPanel` | 类 | 网格面板 |

**b1/GSMUI/GSView**:

| `BUI_GridPanel` | 类 | 网格面板视图 |

---

### 13. `b1/BGU/` — BGU 子系统

**b1/BGU/BUActor**:

| 类型 | 说明 |
|------|------|
| BGU Actor 基类（2 文件） |

**b1/BGU/BUActor/BUFXActor/BGUFXActorS**:

| 类型 | 说明 |
|------|------|
| BGU FX Actor 服务器端（1 文件） |

**b1/BGU/ActorComp** (3 文件):

| 类型 | 说明 |
|------|------|
| BGU Actor 组件（3 文件） |

**b1/BGU/BUS** (2 文件):

| 类型 | 说明 |
|------|------|
| BGU 服务类（2 文件） |

**b1/BGU/AI** (2 文件):

| `BAIT_MoveToSceneItemAndCastSkill` | 类 | AI 移动到场景物品并施放技能 |
| `EMoveToSceneItemAndCastSkillState` | 枚举 | 移动到场景物品并施放技能的状态 |

**b1/BGU/Util**:

| 类型 | 说明 |
|------|------|
| BGU 工具类（1 文件） |

---

### 14. `b1/GameMode/` — 游戏模式 (3 文件)

**命名空间**: `b1.GameMode`

| 类型名称 | 类型 | 继承 | 说明 |
|---------|------|------|------|
| `BGG_GameModeB1` | 类 | `BGG_GameMode` | 单人游戏模式，设置 GameStateClass 为 BGGGameStateB1 |
| `BGG_GameModeB1Net` | 类 | `BGG_GameMode` | 网络游戏模式 |
| `BGG_GameModeStartUp` | 类 | `BGG_GameMode` | 启动游戏模式 |

---

### 15. `b1/GameState/` — 游戏状态 (1 + 4 文件)

**命名空间**: `b1.GameState`

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `BGGGameStateB1` | 类 | 游戏主状态，含音频数据、武器管理器 |

**b1/GameState/Data/:**

| `BGC_AudioData` | 类 | 音频数据 |
| `BGC_WeaponManagerData` | 类 | 武器管理器数据 |
| `FAudioEmitter` | 结构体 | 音频发射器 |
| `IBGC_WeaponManagerData` | 接口 | 武器管理器数据接口 |

---

### 16. `b1/Render/` — 渲染系统 (10 + 19 文件)

**b1/Render** (根):

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `CustomShadowComp` | 类 | 自定义阴影组件 |
| `EFluidSimDimension` | 枚举 | 流体模拟维度 |
| `FluidHeightmapFogComponent` | 类 | 流体高度图雾组件 |
| `FluidHeightmapSettings` | 类 | 流体高度图设置 |
| `FluidSimulationComponent` | 类 | 流体模拟组件 |
| `FluidSimulationInteractor` | 类 | 流体模拟交互器 |
| `FluidSimulationRes` | 类 | 流体模拟资源 |
| `FluidSimulationSettings` | 类 | 流体模拟设置 |
| `GPUSplineMesh` | 类 | GPU 样条网格 |
| `RenderTargetDebugger` | 类 | 渲染目标调试器 |

**b1/Render/Core** — 渲染管线核心:

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `CommandBuffer` | 类 | 命令缓冲区 |
| `ComputeBuffer` | 类 | 计算缓冲区 |
| `ComputeShader` | 类 | 计算着色器 |

**b1/Render/TressFX** — 毛发渲染系统:

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `CommandBuffer` | 类 | TressFX 命令缓冲 |
| `FakeShadowMapComponent` | 类 | 伪阴影贴图组件 |
| `FTressFXVertexFactoryShaderParameters` | 类 | TressFX 顶点工厂着色器参数 |
| `RDGBuilder` | 类 | RDG 构建器 |
| `RHICreateComputeFence` | 类 | RHI 计算围栏 |
| `TressFXComponent` | 类 | TressFX 组件 |
| `TressFXCpp` | 类 | TressFX C++ 互操作 |
| `TressFXFunctionPtrs` | 类 | TressFX 函数指针 |
| `TressFXRenderPipeline` | 类 | TressFX 渲染管线 |
| `TressFXSceneProxy` | 类 | TressFX 场景代理 |
| `TressFXVertexFactory` | 类 | TressFX 顶点工厂 |

**b1/Render/LandscapeBpBrush** — 地形笔刷:

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `BGLandmassActor` | 类 | BG 陆块 Actor |
| `LandscapeBpBrushTest` | 类 | 地形笔刷测试 |
| `LandscapeLayerBrush` | 类 | 地形图层笔刷 |
| `LandscapeRoadBrush` | 类 | 地形道路笔刷 |
| `LandscapeRoadSpline` | 类 | 地形道路样条 |

---

### 17. `b1/ECS/Test/` — ECS 测试 (4 文件)

**命名空间**: `b1.ECS.Test`

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `BS_Api` | 类 | ECS 批量系统 API（桩代码），含 Get_UnitReader/Get_WorldReader/OP_MoveUpdate/OP_CastSkill/OP_SweepCheckTriggerHit 等方法 |
| `BS_MessageQueue` | 类 | 批量系统消息队列 |
| `BS_UnitReader` | 类 | 批量系统单位读取器 |
| `NativeStream` | 类 | 原生流 |

---

### 18. `b1/GSReplicate/` — 网络复制系统 (1 文件)

**命名空间**: `b1.GSReplicate`

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `GSReplicateStruct` | 类 | 网络复制结构定义，包含所有需要同步的数据结构（字典、列表等）的生成配置 |

---

### 19. `b1/GSUI/` — 输入控制 (1 文件)

**命名空间**: `b1.GSUI`

| 类型名称 | 类型 | 继承 | 说明 |
|---------|------|------|------|
| `MouseInputControlActor` | 类 | `AActor` | 鼠标输入控制 Actor，处理鼠标追踪输入和 UI 交互 |

---

### 20. `b1/Editor/` — 编辑器工具 (2 + 2 文件)

**命名空间**: `b1.Editor`

| 类型名称 | 类型 | 继承 | 说明 |
|---------|------|------|------|
| `BED_LevelConfUtil` | 类 | `UBlueprintFunctionLibrary` | 关卡配置工具库，含关卡流送体积、地形体积等编辑器工具 |
| `GlobalAudioMgr` | 类 | - | 全局音频管理器 |

**b1/Editor/Calliope/Behavior/Nodes/**:

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `BED_BehaviorNode_AiConversation` | 类 | Calliope AI 对话行为节点 |
| `BED_BehaviorNode_PlayPigsyStory` | 类 | Calliope 猪八戒剧情行为节点 |

---

### 21. `b1/FUnctionLibUtil/` — 函数库工具 (1 文件)

**命名空间**: `b1.FUnctionLibUtil`

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `FunctionLibUtil` | 静态内部类 | 函数库工具，含单位销毁检查等通用工具方法 |

---

### 22. `b1/Util/` — 工具类 (11 + 10 + 7 文件)

**命名空间**: `b1.Util`

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `CDNUtil` | 类 | CDN 工具 |
| `GSEMiscUtil` | 类 | GSE 杂项工具 |
| `GSE_P4HelperCreator` | 类 | Perforce 辅助创建器 |
| `GSEP4Util` | 类 | Perforce 工具 |
| `GSEP4VersionInfo` | 类 | Perforce 版本信息 |
| `GSEPerfTimeUtil` | 类 | 性能时间工具 |
| `GSETimeUtil` | 类 | 时间工具 |
| `GSPbUtil` | 类 | Protobuf 工具 |
| `IGSE_P4Helper` | 接口 | Perforce 辅助接口 |
| `MyTestHttpListener` | 类 | HTTP 监听器测试 |
| `USharpPerfTest` | 类 | USharp 性能测试 |

**b1/Util/PerfTest/** — 性能测试:

| `IL2CPPStructPersistTest` | IL2CPP 结构持久化测试 |
| `IL2CPPUnitTest` | IL2CPP 单元测试 |
| `IL2CPPUnitTestFuncLib` | IL2CPP 单元测试函数库 |
| `IL2CPPUnitTestStruct1` / `Inner` / `InnerInner` / `Outer` | IL2CPP 测试结构 |
| `ReplicationTest` | 复制测试 |
| `TStrongObjectPtrTestNew` / `TStrongObjectPtrTestOld` | 强指针测试 |
| `WindowsWorkaroundUtils` | Windows 兼容工具 |

**b1/Util/Workaround/**:

| `workaround` 相关 | 兼容性处理 |

---

### 23. `B1UI/GSUI/` — UI 公用组件 (5 文件)

**命名空间**: `B1UI.GSUI`

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `BI_UnitBarListCS` | 类 | 单位血条列表 |
| `BUI_InputTipsOne` | 类 | 输入提示单项 |
| `EUIWordID` | 枚举 | UI 文字 ID |
| `GSUIResPathUtil` | 类 | UI 资源路径工具 |
| `UIConversationCfg` | 类 | UI 对话配置 |

---

### 24. `b1/AnimNotify/AnimNotifyState/` — 动画通知状态 (1 文件)

**说明**: 动画通知状态模块（1 个文件）

---

### 25. `HelloUSharp/` — USharp 示例 (7 文件)

**命名空间**: `HelloUSharp`

| 类型名称 | 类型 | 继承 | 说明 |
|---------|------|------|------|
| `HelloWorldActor` | 类 | `AActor` | USharp 入门示例 Actor，含 Int 属性、CallMe 函数、ReceiveBeginPlay 重载 |
| `HelloUFromUSharp` | 类 | `AActor` | USharp 双向通信示例 Actor，含多类型属性（int/string/delegate/component/array/subclass/struct） |
| `HelloUSharpDelegate` | 类 | 多播委托 | 示例多播委托（在蓝图中可绑定） |
| `HelloUStructTest` | 结构体 | - | 示例结构体 |
| `HelloUTestComp` | 类 | `UActorComponent` | 示例 Actor 组件 |
| `OldMKSpawnTest` | 类 | `AActor` | 旧版 MK 生成测试 |
| `TestSaveGameModule` | 类 | `USaveGame` | 存档测试模块（含 Name/UserIdx/TestObj 属性） |

---

### 26. `GSE/GSSdk/` — 服务器 SDK (1 文件)

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `GSSdkServerDirState` | 枚举 | GS SDK 服务器目录状态 |

---

### 27. `STUN/` — NAT 穿透库 (39 文件)

**说明**: 完整的 STUN（Session Traversal Utilities for NAT）协议 C# 实现，用于 P2P 联机。

| 类型名称 | 类型 | 说明 |
|---------|------|------|
| `StunServer` | 类 | STUN 服务器 |
| `HostnameEndpoint` | 类 | 主机名端点 |

**STUN/Client/**:

| `IStunClient` | 接口 | STUN 客户端接口 |
| `StunClient3489` | 类 | RFC 3489 STUN 客户端 |
| `StunClient5389UDP` | 类 | RFC 5389 STUN 客户端（UDP） |

**STUN/Enums/** (11 文件):

| `AttributeType` | 枚举 | 属性类型 |
| `BindingTestResult` | 枚举 | 绑定测试结果 |
| `Class` | 枚举 | 消息类 |
| `FilteringBehavior` | 枚举 | 过滤行为 |
| `IpFamily` | 枚举 | IP 地址族 |
| `MappingBehavior` | 枚举 | 映射行为 |
| `Method` | 枚举 | 方法 |
| `NatType` | 枚举 | NAT 类型 |
| `ProxyType` | 枚举 | 代理类型 |
| `StunMessageType` | 枚举 | 消息类型 |
| `TransportType` | 枚举 | 传输类型 |

**STUN/Messages/**:

| `StunAttribute` | 类 | STUN 属性 |
| `StunMessage5389` | 类 | RFC 5389 消息 |
| `StunResponse` | 类 | STUN 响应 |

**STUN/Messages/StunAttributeValues/** (13 文件):

| `ChangeRequest` / `ErrorCode` / `MappedAddress` / `MessageIntegrity` / `Realm` / `Nonce` / `ResponseOrigin` / `OtherAddress` / `XorMappedAddress` / `XorOnly` / `Software` / `Username` / `UnknownAttribute` |
| STUN 属性值类型 |

**STUN/Proxy/**:

| STUN 代理相关类型（3 文件） |

**STUN/StunResult/**:

| STUN 结果相关类型（3 文件） |

**STUN/Utils/**:

| STUN 工具类（1 文件） |

---

### 28. `System/` 和 `UnrealEngine/` — 系统扩展

**System/ArrayExtensions/**:

| `ArrayExtensions` | 静态类 | 数组扩展方法：`ForEach` 遍历多维数组 |
| `ArrayTraverse` | 类 | 数组遍历辅助 |

**System/Runtime/CompilerServices/**:

| `IsUnmanagedAttribute` | 类 | 标记非托管类型的属性 |
| `NullableAttribute` | 类 | 可空类型属性 |
| `NullableContextAttribute` | 类 | 可空上下文属性 |

**UnrealEngine/Engine/**:

| `UGameplayStaticsEx` | 类 | UE GameplayStatics 扩展 |

---

### 29. `ILRuntime/Runtime/Generated/` — ILRuntime CLR 绑定

| 类型名称 | 说明 |
|---------|------|
| `CLRBindings` | ILRuntime CLR 类型绑定生成文件，注册所有需要用到的 CLR 类型和方法 |

---

### 30. `Microsoft/CodeAnalysis/` — 嵌入式属性

| `EmbeddedAttribute` | 编译器生成的嵌入式属性 |

---

### 31. `Properties/` — 程序集属性

| `AssemblyInfo.cs` | 程序集信息：版本 1.0.0.0，Guid 60bf569b-3063-4a3b-94b4-e61883a8e747，InternalsVisibleTo("BtlSvr.Script") |

---

## 类/接口/结构/枚举统计汇总

| 类别 | 数量 |
|------|------|
| **类 (class)** | ~8300+（含自动生成） |
| **接口 (interface)** | ~50+ |
| **结构体 (struct)** | ~200+ |
| **枚举 (enum)** | ~300+ |
| **委托 (delegate)** | ~2953+（EventDelDefine） |
| **总计** | ~10834 个 .cs 文件 |

## 架构总结

BtlSvr.Main 作为一个游戏逻辑 DLL，其架构分层如下：

```
UE5 引擎层 (C++)
    |
    |-- USharp 绑定层 ([UClass], [UProperty], [UFunction])
    |
    v
BtlSvr.Main (C# 游戏逻辑层)
    |
    |-- 实体管理: ECS 系统 (b1.ECS)
    |-- 游戏流程: FSM 状态机 (CFSMGReg + b1.* FSM 状态/条件)
    |-- AI 系统: GOAP + 行为树 + 环境查询 (b1.AI*, b1.BGW.EnvQuery)
    |-- 战斗系统: Buff/技能/伤害/单位管理 (b1.*)
    |-- 动画系统: BUAnimHumanoidCS 系列 + 动画通知 (b1.BGU.BUAnim, b1.AnimNotify)
    |-- 渲染系统: TressFX 毛发 + 流体模拟 (b1.Render)
    |-- UI 系统: GSMUI 补间动画 + GSWidget 控件 (b1.GSMUI*, b1.UI*)
    |-- 网络通信: Protobuf(OssB1) + STUN 穿透 (STUN.*)
    |-- 数据层: Archive(存档) + CommB1(只读包装) + OssB1(protobuf消息)
    |-- 测试框架: AutoQA 自动化测试 + PerfTest 性能测试
    |-- 编辑器工具: Calliope 行为树节点 + LevelConf (b1.Editor)
    |
    v
ILRuntime 热更新层 (CrossBindingAdaptor → 热更脚本)
```

该 DLL 的代码规模反映了《黑神话：悟空》作为 AAA 游戏的复杂度——涵盖动画、AI、战斗、渲染、UI、网络、存档等完整的游戏功能模块。
