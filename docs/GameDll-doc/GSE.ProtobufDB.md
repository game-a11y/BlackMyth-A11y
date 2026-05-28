# GSE.ProtobufDB 类导航

> 本文档基于源码分析生成，覆盖 `GSE.ProtobufDB` 程序集中的所有类型。
> 该程序集是黑神话悟空游戏数据配置的 protobuf 序列化层，包含 1709 个 .cs 文件（v1.0.21.23831），分布在 14 个子目录中。
> 绝大多数类型是由 protobuf 编译器从 `.proto` 文件自动生成的 C# 消息类。

---

## 模块结构总览

| 目录 | 用途 | 文件数 |
|------|------|--------|
| `b1/` | 枚举信息查询路由，按游戏版本分发 | ~5 |
| `b1/Protobuf/DataAPI/` | protobuf 数据加载管线核心（解析器、文件监控、校验、异常） | ~24 |
| `b1/Protobuf/BG_ParserManager/` | 解析器状态管理、枚举定义 | ~3 |
| `BaseU3/` | U3 版基础类型 protobuf 封装 & B2D（2D战斗）数据描述 | ~60 |
| `BtlB1/` | B1 版战斗系统数据（最大模块，~460 个枚举及配置描述） | ~460 |
| `BtlShare/` | 跨版本共享的战斗系统数据（~200 个类型） | ~200 |
| `BtlU3/` | U3 版 2D 战斗系统数据 | ~140 |
| `BtlX2/` | X2 版 2D 战斗系统数据 | ~140 |
| `GurCalliopeFsm/` | Calliope FSM 图实例数据（全局图、加载图、事务任务） | ~47 |
| `GurCalliopeState/` | Calliope FSM 自定义状态与检测条件 | ~44 |
| `GurGsPersistent/` | 游戏状态持久化数据（存档系统，BGC/BPC/BUC 容器 & 增量同步消息） | ~130 |
| `GurGsReplicate/` | 游戏状态网络复制数据（战斗同步） | ~22 |
| `GurGsStruct/` | 游戏状态共享结构体 & 增量同步消息 | ~68 |
| `X2/Base/` | X2 版基础类型 protobuf 封装 & B2D 数据描述 | ~60 |
| `Properties/` | 程序集元信息 | ~1 |
| 根目录 | 模块注册信息 | ~1 |

---

## 1. 根目录文件

| 完整类型 | 类别 | 说明 | 文件 |
|----------|------|------|------|
| `SerializedManagedUnrealModuleInfo` | class | 实现 `ISerializedManagedUnrealModuleInfo`，返回十六进制编码的模块注册字符串 | `SerializedManagedUnrealModuleInfo.cs` |
| `Properties.AssemblyInfo` | class | 程序集元数据 | `Properties/AssemblyInfo.cs` |

---

## 2. b1 命名空间 -- 枚举信息查询

### b1 根目录

| 完整类型 | 类别 | 说明 | 文件 |
|----------|------|------|------|
| `b1.B1EnumInfo` | static class | 枚举中文名查询，维护 `enumTypeName -> cnName -> int` 字典 | `b1/B1EnumInfo.cs` |
| `b1.EnumInfo` | static class | 按项目名称分发枚举查询（`B1` / `X2` -> `B1EnumInfo`，其它返回 -1） | `b1/EnumInfo.cs` |
| `b1.U3EnumInfo` | static class | U3 项目枚举信息 | `b1/U3EnumInfo.cs` |
| `b1.X2EnumInfo` | static class | X2 项目枚举信息 | `b1/X2EnumInfo.cs` |

### b1.Protobuf.BG_ParserManager

| 完整类型 | 类别 | 说明 | 文件 |
|----------|------|------|------|
| `b1.Protobuf.BG_ParserManager.BG_ParserStatusManager` | class | 解析器状态管理器 | `b1/Protobuf/BG_ParserManager/BG_ParserStatusManager.cs` |
| `b1.Protobuf.BG_ParserManager.FILE_PROCESS_STATUS` | enum | 文件处理状态枚举 | `b1/Protobuf/BG_ParserManager/FILE_PROCESS_STATUS.cs` |
| `b1.Protobuf.BG_ParserManager.ParserManagerLogVerbosity` | enum | 解析器日志详细级别 | `b1/Protobuf/BG_ParserManager/ParserManagerLogVerbosity.cs` |

### b1.Protobuf.DataAPI -- 数据加载管线核心

**泛型数据 API：**

| 完整类型 | 类别 | 说明 | 文件 |
|----------|------|------|------|
| `b1.Protobuf.DataAPI.BG_ProtobufDataAPI<T>` | generic class | protobuf 数据加载主 API，泛型约束 `IMessage, new()`，支持 CSV/数据文件加载 | `BG_ProtobufDataAPI.cs` |
| `b1.Protobuf.DataAPI.BG_ProtobufDataLoader` | class | protobuf 数据加载器配置（数据根目录、CDN 文件开关等） | `BG_ProtobufDataLoader.cs` |
| `b1.Protobuf.DataAPI.BG_CSVParser` | class | CSV 解析器 | `BG_CSVParser.cs` |

**文件监控：**

| 完整类型 | 类别 | 说明 | 文件 |
|----------|------|------|------|
| `b1.Protobuf.DataAPI.BG_FileWatcher` | class | 通用文件监控器 | `BG_FileWatcher.cs` |
| `b1.Protobuf.DataAPI.BG_CSVFileWatcher` | class | CSV 文件监控器 | `BG_CSVFileWatcher.cs` |
| `b1.Protobuf.DataAPI.BG_CSVFileWatcher2` | class | CSV 文件监控器版本 2 | `BG_CSVFileWatcher2.cs` |
| `b1.Protobuf.DataAPI.BG_ScriptDLLFileWatcher` | class | 脚本 DLL 文件监控器 | `BG_ScriptDLLFileWatcher.cs` |

**P4（Perforce）工具集成：**

| 完整类型 | 类别 | 说明 | 文件 |
|----------|------|------|------|
| `b1.Protobuf.DataAPI.BG_TableToolP4HelperCreator` | class | P4 辅助类的工厂 | `BG_TableToolP4HelperCreator.cs` |
| `b1.Protobuf.DataAPI.IBG_TableToolP4Helper` | interface | P4 辅助操作接口 | `IBG_TableToolP4Helper.cs` |

**数据校验与差异比较：**

| 完整类型 | 类别 | 说明 | 文件 |
|----------|------|------|------|
| `b1.Protobuf.DataAPI.TableDataInfo` | class | 表格数据信息 | `TableDataInfo.cs` |
| `b1.Protobuf.DataAPI.TableDataDiffHelper` | class | 表格数据差异辅助 | `TableDataDiffHelper.cs` |
| `b1.Protobuf.DataAPI.TableRuleValidator` | class | 表格规则校验器 | `TableRuleValidator.cs` |

**自定义异常类（均位于 `b1.Protobuf.DataAPI` 命名空间）：**

| 完整类型 | 类别 | 说明 | 文件 |
|----------|------|------|------|
| `DuplicatedIDException` | class | 重复 ID 异常 | `DuplicatedIDException.cs` |
| `DuplicatePropertyException` | class | 重复属性异常 | `DuplicatePropertyException.cs` |
| `EmptyFileException` | class | 空文件异常 | `EmptyFileException.cs` |
| `FailedDataGuardException` | class | 数据保护校验失败异常 | `FailedDataGuardException.cs` |
| `InconsistentTargetTypeException` | class | 目标类型不一致异常 | `InconsistentTargetTypeException.cs` |
| `IncorrectPropertyNumberException` | class | 属性数量不正确异常 | `IncorrectPropertyNumberException.cs` |
| `InvalidNumCommentException` | class | 无效数字注释异常 | `InvalidNumCommentException.cs` |
| `InvalidProtoMessageException` | class | 无效 protobuf 消息异常 | `InvalidProtoMessageException.cs` |
| `InvalidRowException` | class | 无效行异常 | `InvalidRowException.cs` |
| `ModifyRuleVerificationException` | class | 修改规则验证异常 | `ModifyRuleVerificationException.cs` |
| `SubPropertyInconsistentException` | class | 子属性不一致异常 | `SubPropertyInconsistentException.cs` |
| `UnknowPropertyException` | class | 未知属性异常 | `UnknowPropertyException.cs` |
| `UnknowTypeException` | class | 未知类型异常 | `UnknowTypeException.cs` |

---

## 3. BaseU3 和 X2/Base 命名空间 -- 基础类型封装 & B2D 数据描述

这两个目录结构完全一致。BaseU3 对应于 `BaseU3` 命名空间，X2/Base 对应于 `X2.Base` 命名空间。

### 基础类型 Protobuf 封装（SyncWrapper / TupleWrapper 模式）

通用基础类型的 protobuf 消息封装，用于网络同步。每个类型有三种变体：

| 基础类型 | SyncWrapper（同步包装） | TupleWrapper（元组包装） |
|----------|------------------------|------------------------|
| `bool` | `BoolSyncWrapper` | `BoolTupleWrapper` |
| `Bytes` | `BytesSyncWrapper` | `BytesTupleWrapper` |
| `double` | `DoubleSyncWrapper` | `DoubleTupleWrapper` |
| `Fixed64` | `Fixed64SyncWrapper` | `Fixed64TupleWrapper` |
| `float` | `FloatSyncWrapper` | `FloatTupleWrapper` |
| `int` | `Int32SyncWrapper` | `Int32TupleWrapper` |
| `long` | `Int64SyncWrapper` | `Int64TupleWrapper` |
| `string` | `StringSyncWrapper` | `StringTupleWrapper` |
| `uint` | `Uint32SyncWrapper` | `Uint32TupleWrapper` |
| `ulong` | `Uint64SyncWrapper` | `Uint64TupleWrapper` |
| `DataSyncFlag` | `DataSyncFlagSyncWrapper` | `DataSyncFlagTupleWrapper` |

### B2D 枚举类型（BaseU3 / X2.Base，命名空间一致）

| 枚举类型 | 说明 |
|----------|------|
| `AITargetMode` | AI 目标选择模式 |
| `BattleResultReason` | 战斗结果原因 |
| `BattleType` | 战斗类型 |
| `BattleVictoryConditions` | 战斗胜利条件 |
| `CardRaceType` | 卡牌种族类型 |
| `CardType` | 卡牌类型 |
| `ColorType` | 颜色类型 |
| `PlayerCtrlType` | 玩家控制类型 |
| `RelicType` | 遗物类型 |
| `ResType` | 资源类型 |
| `UnitSpawnConfig` | 单位生成配置 |

每个枚举类型均配备 `SyncWrapper` 和 `TupleWrapper` protobuf 消息封装。

### B2D 数据描述类型

| 完整类型 | 类别 | 说明 | 文件 |
|----------|------|------|------|
| `BaseU3.FUStB2DRelicBuffDesc` | class | B2D 遗物 Buff 描述（ID、名称、描述、颜色、卡牌限制等） | `FUStB2DRelicBuffDesc.cs` |
| `BaseU3.FUStB2DUnitSpawnDesc` | class | B2D 单位生成描述 | `FUStB2DUnitSpawnDesc.cs` |
| `BaseU3.FUStB2DVictoryConditionsDesc` | class | B2D 胜利条件描述 | `FUStB2DVictoryConditionsDesc.cs` |
| `BaseU3.TBFUStB2DRelicBuffDesc` | class | 表级封装，包含 `RepeatedField<FUStB2DRelicBuffDesc>` | `TBFUStB2DRelicBuffDesc.cs` |
| `BaseU3.TBFUStB2DUnitSpawnDesc` | class | 表级封装，包含 `RepeatedField<FUStB2DUnitSpawnDesc>` | `TBFUStB2DUnitSpawnDesc.cs` |
| `BaseU3.TBFUStB2DVictoryConditionsDesc` | class | 表级封装，包含 `RepeatedField<FUStB2DVictoryConditionsDesc>` | `TBFUStB2DVictoryConditionsDesc.cs` |

---

## 4. BtlB1 命名空间 -- B1 版战斗系统数据

这是最大的模块，包含了 B1 版本战斗系统的全部枚举、配置描述和表级封装。所有类型均位于 `BtlB1` 命名空间。

### 4.1 枚举类型（约 90 个）

所有枚举均标注 `[UEnum]`、`[BlueprintType]` 和 `[USharpPath]` 属性，映射到 UE 蓝图系统。

| 枚举 | 说明 |
|------|------|
| `EAbnormalDispModifyType` | 异常状态显示修改类型 |
| `EACFactDataOperateType` | AC 事实数据操作类型 |
| `EACInterruptType` | AC 中断类型 |
| `EAiConversationConditionType` | AI 对话条件类型 |
| `EAiConversationEndType` | AI 对话结束类型 |
| `EAiConversationEventType` | AI 对话事件类型 |
| `EAiConversationTargetType` | AI 对话目标类型 |
| `EAITaskActionType` | AI 任务动作类型 |
| `EAITaskActivationEvent` | AI 任务激活事件 |
| `EAITaskCondition` | AI 任务条件 |
| `EAttachNiagaraEventType` | 附加 Niagara 事件类型 |
| `EBuffTickRuleBySSType` | Buff 按简单状态 Tick 规则类型 |
| `EBulletAreaType` | 子弹区域类型 |
| `EBulletWindFieldActionType` | 子弹风场动作类型 |
| `ECamRefType` | 摄像机参考类型 |
| `ECollectionInteractType` | 收集物交互类型 |
| `ECollectionQualityType` | 收集物品质类型 |
| `EConditionRelationType` | 条件关系类型（与/或） |
| `ECustomizedInputType` | 自定义输入类型 |
| `EDefeatUIShowType` | 击败 UI 显示类型 |
| `EDefeatUITriggerType` | 击败 UI 触发类型 |
| `EECSDataInitType` | ECS 数据初始化类型 |
| `EFixFunctionType` | 固定功能类型 |
| `EFollowPartnerSpawnType` | 跟随伙伴生成类型 |
| `EFsmMoveLogicType` | FSM 移动逻辑类型 |
| `EFsmSolverType` | FSM 解算器类型 |
| `EFsmStateLogicTriggerType` | FSM 状态逻辑触发类型 |
| `EGroupAIAttackBias` | 群体 AI 攻击偏向 |
| `EGroupAIMoveType` | 群体 AI 移动类型 |
| `EHitDestructibleDirType` | 可破坏物命中方向类型 |
| `EHitDestructibleStrengthType` | 可破坏物命中力度类型 |
| `EHitItemAudioType` | 命中道具音频类型 |
| `EHitItemFXType` | 命中道具特效类型 |
| `EHitPartRecoverAttrConversionType` | 命中部位回复属性转换类型 |
| `EHitPartRecoverAttrType` | 命中部位回复属性类型 |
| `EHitPartReduceAttrType` | 命中部位削减属性类型 |
| `EHitPartRuleType` | 命中部位规则类型 |
| `EHitPerformAudioEventType` | 命中表现音频事件类型 |
| `EHitPerformAudioMappingCondition` | 命中表现音频映射条件 |
| `EHitPerformFXEventType` | 命中表现特效事件类型 |
| `EInteractAction` | 交互动作 |
| `EInteractCameraEffect` | 交互摄像机效果 |
| `EInteractLockAction` | 交互锁定动作 |
| `EInterActMappingCondition` | 交互映射条件 |
| `ELandFXPlayType` | 落地特效播放类型 |
| `EMagicSkillMapping` | 法术技能映射 |
| `EMagicSkillOperationMode` | 法术操作模式 |
| `EMapSymbolActiveState` | 地图标记激活状态 |
| `EMapSymbolState` | 地图标记状态 |
| `EMapSymbolType` | 地图标记类型 |
| `EMobAttackType` | 怪物攻击类型 |
| `EMobStrengthLevel` | 怪物强度等级 |
| `EModifyMethod` | 修改方法 |
| `EMoveSpeedType` | 移动速度类型 |
| `ENpcSubInteractType` | NPC 子交互类型 |
| `EPELevelInheritedType` | PE 关卡继承类型 |
| `EPerformLockType` | 表现锁定类型 |
| `EPigsyStoryIAndRType` | 猪八戒故事 I&R 类型 |
| `EPlayerTransType` | 玩家变身类型 |
| `EPlayType` | 播放类型 |
| `EProcessUsage` | 处理用途 |
| `EProjectileBeHittedCostAttrType` | 弹道被命中消耗属性类型 |
| `EProjectileCheckShapeType` | 弹道检测形状类型 |
| `EProjectileResetDirType` | 弹道重置方向类型 |
| `EProjectileResetTargetType` | 弹道重置目标类型 |
| `EProjectileScaleCurveXAxisType` | 弹道缩放曲线 X 轴类型 |
| `EProjectileScaleCurveYAxisType` | 弹道缩放曲线 Y 轴类型 |
| `EquipPosition` | 装备位置 |
| `ERemoveAttachedNiagaraRule` | 移除附加 Niagara 规则 |
| `EScarDecalTriggerType` | 疤痕贴花触发类型 |
| `ESceneItemSurfaceType` | 场景道具表面类型 |
| `ESeqClockSource` | 序列时钟源 |
| `ESeqHiddenHeadType` | 序列隐藏头部类型 |
| `ESequenceType` | 序列类型 |
| `ESkillSelectOpType` | 技能选择操作类型 |
| `ESkillSelectReleaseType` | 技能选择释放类型 |
| `ESkillsRefIDType` | 技能参考 ID 类型 |
| `ESkillsRefWhiteListType` | 技能参考白名单类型 |
| `ESkipMode` | 跳过模式 |
| `ESkipType` | 跳过类型 |
| `ESlowTraceSolution` | 慢追踪方案 |
| `ESpellTargetBaseType` | 法术目标基础类型 |
| `ESpellTriggerEffectType` | 法术触发效果类型 |
| `ESummonTargetMethod` | 召唤目标方法 |
| `ESummonUnitLocationType` | 召唤单位位置类型 |
| `ESummonUnitRotationType` | 召唤单位旋转类型 |
| `ETaskStageState` | 任务阶段状态 |
| `EUnitAICrowdQualityLevel` | 单位 AI 人群质量等级 |
| `EUnitAIDetourCrowdType` | 单位 AI 绕行人群类型 |
| `EValOp` | 值操作符 |
| `EValueClampType` | 值钳制类型 |
| `FUStGlobalConfigType` | 全局配置类型 |
| `FUStIronBodyBuffTarget` | 铁甲 Buff 目标 |
| `SpellEffectType` | 法术效果类型 |
| `SpellNameEnum` | 法术名称枚举 |
| `SpellType` | 法术类型 |
| `SuitQuality` | 套装品质 |

每个枚举均配备 `SyncWrapper`（protobuf 消息封装）和 `TupleWrapper`。

### 4.2 数据结构类型（非表级，F-prefix 结构体）

| 完整类型 | 说明 |
|----------|------|
| `CastSkillLockRuleInfo` | 技能施放锁定规则信息 |
| `FAbnormalDispModifyInfo` | 异常状态显示修改信息 |
| `FAbnormalDispModifyInfoFreezeExt` | 异常显示修改冻结扩展 |
| `FACModifyFactData` | AC 修改事实数据 |
| `FAiConversationCondition` | AI 对话条件 |
| `FPlayFXByResID` | 按资源 ID 播放特效 |
| `FProjectileBeHittedCostAttr` | 弹道被命中消耗属性 |
| `FSingleAbnormalDispModifyInfo` | 单条异常显示修改信息 |
| `FSpellEffect` | 法术效果 |
| `NPCInfo` | NPC 信息 |
| `SkillMappingConfig` | 技能映射配置 |
| `TalentDisplayCfg` | 天赋显示配置 |
| `TaskStageGainItemInfo` | 任务阶段获得道具信息 |
| `TaskStageInfo` | 任务阶段信息 |

### 4.3 FUSt 数据描述类型 -- 配置表条目

约 160+ 个类型，命名模式为 `FUSt<领域>Desc`。前缀含义：**F** = 结构体，**USt** = Unreal State Table（虚幻状态表）。每个类实现 `IMessage<FUSt*Desc>`，是 protobuf 自动生成的单行配置条目。

以下按领域分组列出：

**战斗基础：**
| 类型 | 说明 |
|------|------|
| `FUStAttrCopyConfigDesc` | 属性复制配置 |
| `FUStAttrEffectConfig` | 属性效果配置 |
| `FUStAttrEffectDesc` | 属性效果描述 |
| `FUStBeAttackedInfoDesc` | 被攻击信息描述 |
| `FUStBeAttackedStiffLevelMappingDesc` | 被攻击硬直等级映射 |
| `FUStBeAttackedDispInfoDesc` | 被攻击显示信息描述 |
| `FUStBeAttackedFXMapDesc` | 被攻击特效映射 |
| `FUStBeAttackedDispInfoDesc` | 被攻击显示信息描述 |
| `FUStAttackerHitFXMappingDesc` | 攻击者命中特效映射 |
| `FUStAttackerHitAudioEventMappingDesc` | 攻击者命中音频映射 |
| `FUStAttackHitFXMapDesc` | 攻击命中特效映射 |
| `FUStAttackHitAudioInfoDesc` | 攻击命中音频信息 |

**Buff 系统：**
| 类型 | 说明 |
|------|------|
| `FUStBuffGroupDesc` | Buff 组描述 |
| `FUStBuffDispGroupDesc` | Buff 显示组描述 |
| `FUStBuffRuleDesc` | Buff 规则描述 |
| `FUStBuffTickRuleBySimpleStateDesc` | Buff 按简单状态 Tick 规则 |
| `FUStAbnormalCommConfigDesc` | 异常状态通用配置 |
| `FUStAbnormalDispAttackerMapDesc` | 异常显示攻击者映射 |
| `FUStAbnormalDispVictimMapDesc` | 异常显示受害者映射 |
| `FUStAbnormalStateUIBlackListDesc` | 异常状态 UI 黑名单 |

**子弹/弹道系统：**
| 类型 | 说明 |
|------|------|
| `FUStBulletCommDesc` | 子弹通用描述 |
| `FUStBulletExpandDesc` | 子弹扩展描述 |
| `FUStBulletSwitchDesc` | 子弹切换描述 |
| `FUStBulletWindFieldExpandDesc` | 子弹风场扩展描述 |
| `FUStBulletAffectArea` | 子弹影响区域 |
| `FUStSpawnBulletMinMaxValue` | 生成子弹最小最大值 |
| `FUStSpawnBulletsData` | 生成子弹数据 |
| `FUStSpawnBulletSpeed` | 生成子弹速度 |
| `FUStChargeSkillBuffInfo` | 蓄力技能 Buff 信息 |
| `FUStChargeSkillSDesc` | 蓄力技能描述 |
| `FUStChargeSkillSuperArmorInfo` | 蓄力技能霸体信息 |

**弹道（Projectile）：**
| 类型 | 说明 |
|------|------|
| `FUStProjectileBase` | 弹道基础 |
| `FUStProjectileBornDir` | 弹道出生方向 |
| `FUStProjectileBornDirOffset` | 弹道出生方向偏移 |
| `FUStProjectileCommDesc` | 弹道通用描述 |
| `FUStProjectileDispDesc` | 弹道显示描述 |
| `FUStProjectileMoveDesc` | 弹道移动描述 |
| `FUStProjectileMulTargetRule` | 弹道多目标规则 |
| `FUStProjectilePosOffset` | 弹道位置偏移 |
| `FUStEffectiveHitProjectileEffectDesc` | 有效命中弹道效果描述 |

**摄像机：**
| 类型 | 说明 |
|------|------|
| `FUStCameraConversionParamConfigDesc` | 摄像机转换参数配置 |
| `FUStCameraGroupDesc` | 摄像机组描述 |
| `FUStDiagonalCamDesc` | 对角摄像机描述 |
| `FUStStraightCamDesc` | 直线摄像机描述 |
| `FUStEnemyCameraDesc` | 敌人摄像机描述 |
| `FUStGiantLockCameraDesc` | 巨型锁定摄像机描述 |
| `FUStMultiPointLockCameraConfigDesc` | 多点锁定摄像机配置 |
| `FUStPlayerCameraDesc` | 玩家摄像机描述 |

**玩家/角色：**
| 类型 | 说明 |
|------|------|
| `FUStPlayerCommDesc` | 玩家通用描述 |
| `FUStPlayerInputSkillMappingDesc` | 玩家输入技能映射 |
| `FUStPlayerSkillCtrlDesc` | 玩家技能控制描述 |
| `FUStPlayerTransAttrDesc` | 玩家变身属性描述 |
| `FUStPlayerTransUnitConfDesc` | 玩家变身单位配置 |
| `FUStUnitAIDesc` | 单位 AI 描述 |
| `FUStUnitCollisionConfig` | 单位碰撞配置 |
| `FUStUnitCollisionHitMoveDesc` | 单位碰撞命中移动 |
| `FUStUnitDeadDesc` | 单位死亡描述 |
| `FUStUnitDeadOldDesc` | 单位死亡描述（旧版） |
| `FUStUnitDeadSwitchToPhysicDesc` | 单位死亡切换物理描述 |
| `FUStUnitDropDesc` | 单位掉落描述 |
| `FUStUnitFootstepDesc` | 单位脚步声描述 |
| `FUStUnitIntelligenceInfoDesc` | 单位智能信息描述 |
| `FUStUnitPhysicalAnimationDesc` | 单位物理动画描述 |
| `FUStUnitAudioBankMapDesc` | 单位音频库映射 |
| `FUStUnitChangeMaterialByAttrDesc` | 单位按属性切换材质 |
| `FUStUnitSoulCamera` | 单位魂摄像机 |
| `FUStUnitSpecialMoveDesc` | 单位特殊移动描述 |
| `FUStUnitTransCommDesc` | 单位变身通用描述 |
| `FUStUnitTransStageDesc` | 单位变身阶段描述 |

**AI / 行为：**
| 类型 | 说明 |
|------|------|
| `FUStAITaskInfo` | AI 任务信息 |
| `FUStGroupAISDesc` | 群体 AI 描述 |
| `FUStSkillAIDesc` | 技能 AI 描述 |
| `FUStMandatoryAITaskDesc` | 强制 AI 任务描述 |
| `FUStAICrowdDetourLevelConfigDesc` | AI 人群绕行等级配置 |
| `FUStAiConversationContentDesc` | AI 对话内容描述 |
| `FUStAiConversationEventDesc` | AI 对话事件描述 |
| `FUStAiConversationGroupDesc` | AI 对话组描述 |
| `FUStAiInteractionMappingDesc` | AI 交互映射描述 |

**技能：**
| 类型 | 说明 |
|------|------|
| `FUStSkillSMappingDesc` | 技能 S 映射描述 |
| `FUStSkillsRefCheckWhiteListDesc` | 技能参考检查白名单 |
| `FUStOverlyingSkillSDesc` | 重叠技能描述 |
| `FUStChargeSkillSDesc` | 蓄力技能描述 |
| `FUStRollSkillDesc` | 翻滚技能描述 |
| `FUStPassiveSkillDesc` | 被动技能描述 |
| `FUStPhantomRushSkillConfigDesc` | 幻影冲刺技能配置 |
| `FUStImmobilizeSkillConfigDesc` | 定身术技能配置 |
| `FUStSealingSpellSkillConfigDesc` | 禁咒技能配置 |
| `FUStSoulSkillMimicryDesc` | 魂魄技能模仿描述 |
| `FUStCCGCastSkillMappingRuleDesc` | CCG 施法技能映射规则 |
| `FUStChargeSkillBuffInfo` | 蓄力技能 Buff 信息 |
| `FUStChargeSkillSuperArmorInfo` | 蓄力技能霸体信息 |

**配置/全局：**
| 类型 | 说明 |
|------|------|
| `FUStGlobalConfigDesc` | 全局配置描述 |
| `FUStGlobalConfigInfo` | 全局配置信息 |
| `FUStGlobalCannotDeadExtraConfigDesc` | 全局不可死亡额外配置 |
| `FUStLevelCommDesc` | 关卡通用描述 |
| `FUStCBGTemplateDesc` | CBG 模板描述 |
| `FUStSettingClassNameDesc` | 设置类名描述 |
| `FUStSettingDetailDesc` | 设置详情描述 |
| `FUStTalentDisplayDesc` | 天赋显示描述 |
| `FUStTalentLvUpCfgDesc` | 天赋升级配置描述 |

**召唤/伙伴：**
| 类型 | 说明 |
|------|------|
| `FUStSummonCommDesc` | 召唤通用描述 |
| `FUStSummonCopySkillDesc` | 召唤复制技能描述 |
| `FUStFollowPartnerConfigDesc` | 跟随伙伴配置描述 |
| `FUStAssociationUnitInfoSDesc` | 关联单位信息描述 |

**场景/环境：**
| 类型 | 说明 |
|------|------|
| `FUStCollectionEventProbabilityDesc` | 收集事件概率描述 |
| `FUStCollectionSpawnGroupDesc` | 收集生成组描述 |
| `FUStCollectionSpawnInfoDesc` | 收集生成信息描述 |
| `FUStEnvironmentSurfaceEffectDesc` | 环境表面效果描述 |
| `FUStEnvironmentSwitchDesc` | 环境切换描述 |
| `FUStHitSceneItemPerformDesc` | 命中场景道具表现描述 |
| `FUStDynamicObstaclePerformanceDesc` | 动态障碍物表现描述 |
| `FUStStreamingLevelStateDesc` | 流式关卡状态描述 |

**其它：**
| 类型 | 说明 |
|------|------|
| `FUStAudioExtendDesc` | 音频扩展描述 |
| `FUStBossRoomConfigDesc` | Boss 房间配置 |
| `FUStCostAttrBySkillEffectData` | 技能效果消耗属性数据 |
| `FUStCustomStateMachineDesc` | 自定义状态机描述 |
| `FUStDeadSeqUnitConfigDesc` | 死亡序列单位配置 |
| `FUStDefeatSlowTimeConfigDesc` | 击败慢动作配置 |
| `FUStDelayPlayerGainConfig` | 延迟玩家获得配置 |
| `FUStDelayTriggerEffects` | 延迟触发效果 |
| `FUStDetonateConfigDesc` | 引爆配置描述 |
| `FUStDialogueDesc` | 对话描述 |
| `FUStDialogueIDMappingDesc` | 对话 ID 映射 |
| `FUStDropItemDesc` | 掉落道具描述 |
| `FUStElementDmgRatioLevelDesc` | 元素伤害比例等级 |
| `FUStEliteBuffConfigDesc` | 精英 Buff 配置 |
| `FUStExAnimDataDesc` | 额外动画数据描述 |
| `FUStExplosiveInfo` | 爆炸物信息 |
| `FUStFixFunctionDesc` | 固定功能描述 |
| `FUStGuideAssetConfigDesc` | 引导资源配置 |
| `FUStHitVEffectDesc` | 命中 V 特效描述 |
| `FUStInteractCondition` | 交互条件 |
| `FUStInteractionMappingDesc` | 交互映射描述 |
| `FUStInteractiveUnitCommDesc` | 交互单位通用描述 |
| `FUStIronBodyBuffTriggerInfo` | 铁甲 Buff 触发信息 |
| `FUStIronBodyConfigDesc` | 铁甲配置描述 |
| `FUStKeyValue` | 键值对 |
| `FUStKeyValueList` | 键值对列表 |
| `FUStLevelSequenceClearBattleItemConfigDesc` | 关卡序列清理战斗道具配置 |
| `FUStLifeSavingHairConfigDesc` | 救命毫毛配置 |
| `FUStMagicConfInfo` | 法术配置信息 |
| `FUStMagicFieldExpandDesc` | 法术场扩展描述 |
| `FUStMapMobConfigDesc` | 地图怪物配置 |
| `FUStMapSymbolDesc` | 地图标记描述 |
| `FUStMobLevelMappingDesc` | 怪物等级映射 |
| `FUStMontagePathWithWeight` | 蒙太奇路径带权重 |
| `FUStMovementOptStrategyConfigDesc` | 移动优化策略配置 |
| `FUStMovieSequenceDesc` | 过场序列描述 |
| `FUStMPCParamWithCurve` | MPC 参数曲线 |
| `FUStNianhuiAwardDesc` | 年兽奖励描述 |
| `FUStNianhuiNameListDesc` | 年兽名称列表 |
| `FUStNPCBaseInfoDesc` | NPC 基础信息描述 |
| `FUStOnlineScreenMsgConfDesc` | 在线屏幕消息配置 |
| `FUStPartDamagedInfo` | 部位损坏信息 |
| `FUStPartHitAttrRecoverConfig` | 部位命中属性恢复配置 |
| `FUStPartHitExpandDesc` | 部位命中扩展描述 |
| `FUStPartRuleInfoDesc` | 部位规则信息描述 |
| `FUStPhaseMobConfig` | 阶段怪物配置 |
| `FUStPhaseSpawnWaveConfig` | 阶段生成波次配置 |
| `FUStPhysicalHitBoneRuleDesc` | 物理命中骨骼规则 |
| `FUStPigsyStoryIAndRLibraryDesc` | 猪八戒故事 I&R 库描述 |
| `FUStPigsyStoryLibraryDesc` | 猪八戒故事库描述 |
| `FUStPotentialEnergyConfigDesc` | 势能配置描述 |
| `FUStPotentialEnergyLevelDetailConfig` | 势能等级详情配置 |
| `FUStQTEDesc` | QTE 描述 |
| `FUStRangePointSetRule` | 范围点集规则 |
| `FUStRebirthAreaDesc` | 重生区域描述 |
| `FUStRebirthPointDesc` | 重生点描述 |
| `FUStRedQualityInfo` | 红色品质信息 |
| `FUStRetrievedMontageData` | 检索到的蒙太奇数据 |
| `FUStRichTextIconDesc` | 富文本图标描述 |
| `FUStScarInfoDesc` | 疤痕信息描述 |
| `FUStSeqAudioJumpLengthDesc` | 序列音频跳跃长度 |
| `FUStShiningDesc` | 发光描述 |
| `FUStSubtitleDesc` | 字幕描述 |
| `FUStSuitDesc` | 套装描述 |
| `FUStSuitInfo` | 套装信息 |
| `FUStSuperArmorLevelDesc` | 霸体等级描述 |
| `FUStSweepCheckDesc` | 扫描检测描述 |
| `FUStSwitchMagicConfInfo` | 切换法术配置信息 |
| `FUStTamerStrategyConfigDesc` | 驯兽师策略配置 |
| `FUStTaskLineDesc` | 任务线描述 |
| `FUStTaskStageDesc` | 任务阶段描述 |
| `FUStTeamRelationConfigDesc` | 队伍关系配置 |
| `FUStTransActiveStateDesc` | 变身激活状态描述 |
| `FUStTransQiTianDaShengConfigDesc` | 齐天大圣变身配置 |
| `FUStTriggerEffectData` | 触发效果数据 |
| `FUStTROStrategyConfigDesc` | TRO 策略配置 |
| `FUStUIWordDesc` | UI 文字描述 |
| `FUStWeakPerformConfigDesc` | 虚弱表现配置 |
| `FUStEQSSettingDesc` | EQS 设置描述 |
| `FUStAddBuffByIdData` | 按 ID 添加 Buff 数据 |
| `FUStAttachedNiagaraByHitDesc` | 命中附加 Niagara 描述 |
| `FUStAttrCopyConfigDesc` | 属性复制配置 |
| `FUStAttrEffectConfig` | 属性效果配置 |
| `FUStAttrEffectDesc` | 属性效果描述 |
| `FUStDelayTriggerEffects` | 延迟触发效果 |

### 4.4 TBFUSt 表级封装

每个 `FUSt*Desc` 类型对应一个 `TBFUSt*Desc`（Table-Based FUSt），其唯一字段为 `RepeatedField<FUSt*Desc> List`。命名模式：`TBFUSt<同名>Desc`。

---

## 5. BtlShare 命名空间 -- 跨版本共享战斗数据

### 5.1 共享枚举（约 70 个）

| 枚举 | 说明 |
|------|------|
| `EActionTagType` | 动作标签类型 |
| `EAIBasicActionType` | AI 基础动作类型 |
| `EAIElemType` | AI 元素类型 |
| `EAttrCostType` | 属性消耗类型 |
| `EBGPPlayerTag` | BGP 玩家标签 |
| `EBGPTagTrigger` | BGP 标签触发器 |
| `EBGUAttrFloat` | BGU 浮点属性 |
| `EBGUBloodBarShowType` | BGU 血条显示类型 |
| `EBGUBloodBarType` | BGU 血条类型 |
| `EBGUBulletRecoveryMode` | BGU 子弹回收模式 |
| `EBGUBulletSweepCheckType` | BGU 子弹扫描检测类型 |
| `EBGUBulletType` | BGU 子弹类型 |
| `EBGUEnvObjSelector` | BGU 环境对象选择器 |
| `EBGUInteractUnitState` | BGU 交互单位状态 |
| `EBGUMagicFieldGenType` | BGU 法术场生成类型 |
| `EBGUResetType` | BGU 重置类型 |
| `EBuffAndSkillEffectCategory` | Buff 和技能效果分类 |
| `EBuffAndSkillEffectType` | Buff 和技能效果类型 |
| `EBuffEffectTargetSelectType` | Buff 效果目标选择类型 |
| `EBuffEffectTriggerType` | Buff 效果触发类型 |
| `EBuffLayerDispMixType` | Buff 层显示混合类型 |
| `EBuffRangeTargetBase` | Buff 范围目标基础 |
| `EBuffRuleType` | Buff 规则类型 |
| `EBulletOrMagicFieldMoveModeType` | 子弹或法术场移动模式类型 |
| `ECameraType` | 摄像机类型 |
| `EChallengeDifficulty` | 挑战难度 |
| `EChallengeSuccessType` | 挑战成功类型 |
| `EChargeSkillStage` | 蓄力技能阶段 |
| `ECollectionPortraitStage` | 收集物肖像阶段 |
| `ECollectionStage` | 收集阶段 |
| `ECollectionStageRemove` | 收集阶段移除 |
| `ECtrlActionType` | 控制动作类型 |
| `EDeadReason` | 死亡原因 |
| `EDmgRangeType` | 伤害范围类型 |
| `EEffectRangeCenterType` | 效果范围中心类型 |
| `EEffectTargetBase` | 效果目标基础 |
| `EEnhancedTriggerEvent` | 增强触发事件 |
| `EEQSGenerator` | EQS 生成器 |
| `EFeatureInputType` | 功能输入类型 |
| `EFilterType` | 过滤类型 |
| `EGSBuffAndSkillEffectActiveCondition` | GS Buff 技能效果激活条件 |
| `EGSBuffLayerCounterType` | GS Buff 层计数器类型 |
| `EGSPosFitType` | GS 位置适配类型 |
| `EGSQTESyncType` | GS QTE 同步类型 |
| `EGSRoarWeightLevel` | GS 咆哮权重等级 |
| `EGSYesNo` | GS 是否枚举 |
| `EGuideGroupFinishType` | 引导组完成类型 |
| `EGuideGroupTriggerType` | 引导组触发类型 |
| `EGuideGroupType` | 引导组类型 |
| `EGuideNodeFinishType` | 引导节点完成类型 |
| `EGuideType` | 引导类型 |
| `EHitActionDir` | 命中动作方向 |
| `EHitOrientationType` | 命中朝向类型 |
| `EHitSlowResumeType` | 命中慢动作恢复类型 |
| `EHitWeightGearType` | 命中权重档位类型 |
| `EInputActionType` | 输入动作类型 |
| `EInteractType` | 交互类型 |
| `EItemQualityColor` | 道具品质颜色 |
| `ELockCamMode` | 锁定摄像机模式 |
| `EMatchingPosType` | 匹配位置类型 |
| `EPillarFormTerminatorType` | 柱形态终结者类型 |
| `EProjectileObjSpdType` | 弹道对象速度类型 |
| `ERangeType` | 范围类型 |
| `EScreenMsgType` | 屏幕消息类型 |
| `ESettingOPType` | 设置操作类型 |
| `ESkillBaseTarget` | 技能基础目标 |
| `ESkillCooldownType` | 技能冷却类型 |
| `ESkillDamageType` | 技能伤害类型 |
| `ESkillMappingConditionType` | 技能映射条件类型 |
| `ESkillMappingResultRull` | 技能映射结果规则 |
| `ESkillRotateType` | 技能旋转类型 |
| `ESkillType` | 技能类型 |
| `ESmartSelectShapeType` | 智能选择形状类型 |
| `ESmartSelectTargetType` | 智能选择目标类型 |
| `EStoryStageRemove` | 故事阶段移除 |
| `EThinkType` | 思考类型 |
| `EUnitAIAttackType` | 单位 AI 攻击类型 |
| `EUnitBodyType` | 单位身体类型 |
| `EUnitDefeatedType` | 单位击败类型 |
| `EUnitQualityType` | 单位品质类型 |
| `EUnitSquadMemberType` | 单位小队成员类型 |

### 5.2 共享数据结构

| 完整类型 | 说明 |
|----------|------|
| `AKMarkerCulture` | AK 音频标记文化信息（包含名称和标记列表） |
| `AKMarkerInfo` | AK 音频标记信息 |
| `EffectAttrCfg` | 效果属性配置（类型 + 数值） |
| `GuideInputAction` | 引导输入动作 |
| `SkillMappingConfig` | 技能映射配置 |

### 5.3 共享 FUSt 数据描述

| 类型 | 说明 |
|------|------|
| `FUStAIActionDesc` | AI 动作描述 |
| `FUStAIActionFilter` | AI 动作过滤器 |
| `FUStAIFeatureDesc` | AI 功能描述 |
| `FUStAIFeatureFilter` | AI 功能过滤器 |
| `FUStAISkillBasicActionDesc` | AI 技能基础动作描述 |
| `FUStAISkillTagsDesc` | AI 技能标签描述 |
| `FUStAIThinkDesc` | AI 思考描述 |
| `FUStAkEventMarkerDesc` | AK 事件标记描述 |
| `FUStBasicAction` | 基础动作 |
| `FUStBuffDesc` | Buff 描述 |
| `FUStBuffDispDesc` | Buff 显示描述 |
| `FUStBuffEffectActiveCondition` | Buff 效果激活条件 |
| `FUStBuffEffectAttr` | Buff 效果属性 |
| `FUStBuffIconDesc` | Buff 图标描述 |
| `FUStBuffLayerDispConfig` | Buff 层显示配置 |
| `FUStBuffLayerDispDesc` | Buff 层显示描述 |
| `FUStChallengeDesc` | 挑战描述 |
| `FUStEnhancedInputActionDesc` | 增强输入动作描述 |
| `FUStFloatCurveToParam` | 浮点曲线转参数 |
| `FUStFXSetting` | 特效设置 |
| `FUStGuideGroupDesc` | 引导组描述 |
| `FUStGuideNodeDesc` | 引导节点描述 |
| `FUStMagicFieldCommDesc` | 法术场通用描述 |
| `FUStRange` | 范围 |
| `FUStSkillDamageExpandDesc` | 技能伤害扩展描述 |
| `FUStSkillEffectDesc` | 技能效果描述 |
| `FUStSkillSDesc` | 技能 S 描述 |
| `FUStThinkElem` | 思考元素 |
| `FUStUnitBattleInfoExtendDesc` | 单位战斗信息扩展描述 |
| `FUStUnitCommDesc` | 单位通用描述 |
| `FUStUnitEnvMaskConfigDesc` | 单位环境遮罩配置描述 |
| `FUStUnitLevelUpDesc` | 单位升级描述 |
| `FUStUnitPassiveSkillInfoExtendDesc` | 单位被动技能信息扩展描述 |

### 5.4 共享 TBFUSt 表级封装

同名 TBFUSt 前缀封装（`TBFUStBuffDesc` 对应 `FUStBuffDesc` 的列表等）。

---

## 6. BtlU3 和 BtlX2 命名空间 -- 2D 战斗系统

这两个目录结构完全一致，分别对应 `BtlU3` 和 `BtlX2` 命名空间。

### 6.1 B2D 枚举（约 30 个）

| 枚举 | 说明 |
|------|------|
| `EB2DAttrFloat` | B2D 浮点属性 |
| `EB2DAttrInt` | B2D 整数属性 |
| `EB2DBattleState` | B2D 战斗状态 |
| `EB2DBuffAndSkillEffectType` | B2D Buff 和技能效果类型 |
| `EB2DBuffEffectTriggerType` | B2D Buff 效果触发类型 |
| `EB2DBuffHarmType` | B2D Buff 伤害类型 |
| `EB2DBuffRangeTargetBase` | B2D Buff 范围目标基础 |
| `EB2DBulletShape` | B2D 子弹形状 |
| `EB2DBulletType` | B2D 子弹类型 |
| `EB2DChangeMeshScaleType` | B2D 改变网格缩放类型 |
| `EB2DDamageCauseDeadType` | B2D 伤害导致死亡类型 |
| `EB2DDamageType` | B2D 伤害类型 |
| `EB2DDeadReason` | B2D 死亡原因 |
| `EB2DDispPredefineDir` | B2D 显示预定义方向 |
| `EB2DDispRangeType` | B2D 显示范围类型 |
| `EB2DEffectRangeTargetBase` | B2D 效果范围目标基础 |
| `EB2DPosFitType` | B2D 位置适配类型 |
| `EB2DRangeType` | B2D 范围类型 |
| `EB2DSBMapCond` | B2D SB 映射条件 |
| `EB2DSEffectTriggerType` | B2D S 效果触发类型 |
| `EB2DSimpleState` | B2D 简单状态 |
| `EB2DSkillTargetType` | B2D 技能目标类型 |
| `EB2DSkillTriggerEvent` | B2D 技能触发事件 |
| `EB2DSkillType` | B2D 技能类型 |
| `EB2DSpecialState` | B2D 特殊状态 |
| `EB2DStateTrigger` | B2D 状态触发 |
| `EB2DStateWithMontage` | B2D 带蒙太奇的状态 |
| `EB2DTargetFilter` | B2D 目标过滤器 |
| `EB2DUnitState` | B2D 单位状态 |
| `EB2DUnitThreatenType` | B2D 单位威胁类型 |
| `EBehitType` | 被命中类型 |

### 6.2 B2D 数据结构

| 类型 | 说明 |
|------|------|
| `AutoPathPos` | 自动寻路位置 |
| `FUStB2DArchivesData` | B2D 档案数据 |
| `FUStB2DArchivesHeroData` | B2D 英雄档案数据 |
| `FUStB2DArchivesMonsterData` | B2D 怪物档案数据 |
| `FUStB2DAutoPathDesc` | B2D 自动寻路描述 |
| `FUStB2DBuffDesc` | B2D Buff 描述 |
| `FUStB2DBuffDispDesc` | B2D Buff 显示描述 |
| `FUStB2DBuffEffectAttr` | B2D Buff 效果属性 |
| `FUStB2DBuffMapDesc` | B2D Buff 映射描述 |
| `FUStB2DBulletCommDesc` | B2D 子弹通用描述 |
| `FUStB2DComboSkill` | B2D 连击技能 |
| `FUStB2DFXSetting` | B2D 特效设置 |
| `FUStB2DLevelTimeBonusDesc` | B2D 关卡时间奖励描述 |
| `FUStB2DMultiKillEnegyDesc` | B2D 多杀能量描述 |
| `FUStB2DNPCDesc` | B2D NPC 描述 |
| `FUStB2DPatrolPointDesc` | B2D 巡逻点描述 |
| `FUStB2DRange` | B2D 范围 |
| `FUStB2DSBMapCond` | B2D SB 映射条件 |
| `FUStB2DSkillData` | B2D 技能数据 |
| `FUStB2DSkillEffectDesc` | B2D 技能效果描述 |
| `FUStB2DSkillLevelMapDesc` | B2D 技能等级映射描述 |
| `FUStB2DSkillMapDesc` | B2D 技能映射描述 |
| `FUStB2DSkillSDesc` | B2D 技能 S 描述 |
| `FUStB2DSkillShakeData` | B2D 技能震动数据 |
| `FUStB2DSkillStage` | B2D 技能阶段 |
| `FUStB2DSpecialStateShowDesc` | B2D 特殊状态显示描述 |
| `FUStB2DSummonDesc` | B2D 召唤描述 |
| `FUStB2DUnitCommDesc` | B2D 单位通用描述 |
| `FUStB2DUnitDeadDispDesc` | B2D 单位死亡显示描述 |

### 6.3 B2D TBFUSt 表级封装

同名 `TBFUStB2D*` 前缀封装。

---

## 7. GurCalliopeFsm 命名空间 -- Calliope FSM 图实例

FSM（有限状态机）图实例数据，用于游戏全局流程控制和加载流程控制。

### 全局图实例

| 完整类型 | 说明 |
|----------|------|
| `GI_Global_SubG_GI_Global_BenchMark` | 基准测试子图 |
| `GI_Global_SubG_GI_Global_WXLogin` | 微信登录子图 |
| `GI_Global_SubG_GI_Loading_820DemoReSetGameData` | 820 演示重置游戏数据 |
| `GI_Global_SubG_GI_Loading_820DemoStartUp` | 820 演示启动 |
| `GI_Global_SubG_GI_Loading_BackToMainMenu` | 返回主菜单 |
| `GI_Global_SubG_GI_Loading_BackToStandAlone` | 返回单机模式 |
| `GI_Global_SubG_GI_Loading_CheckGSSdkServerConfig` | 检查 GS SDK 服务器配置 |
| `GI_Global_SubG_GI_Loading_GameLevelPass` | 关卡通关 |
| `GI_Global_SubG_GI_Loading_HandleDisConnect` | 处理断线重连 |
| `GI_Global_SubG_GI_Loading_InitWXLogin` | 初始化微信登录 |
| `GI_Global_SubG_GI_Loading_PartyRoomClient` | 派对房间客户端 |
| `GI_Global_SubG_GI_Loading_PartyRoomServer` | 派对房间服务器 |
| `GI_Global_SubG_GI_Loading_PostWXLoginFinish` | 微信登录完成后续 |
| `GI_Global_SubG_GI_Loading_PreEnterMainMenu` | 进入主菜单前 |
| `GI_Global_SubG_GI_Loading_PreviewSequence` | 预览序列 |
| `GI_Global_SubG_GI_Loading_ReplayBattle` | 重放战斗 |
| `GI_Global_SubG_GI_Loading_SaveArchiveAndWaitFinish` | 保存存档并等待完成 |
| `GI_Global_SubG_GI_Loading_ServerLogin` | 服务器登录 |
| `GI_Global_SubG_GI_Loading_SetConfigAndPrecompilePSO` | 设置配置并预编译 PSO |
| `GI_Global_SubG_GI_Loading_StartNewGame` | 开始新游戏 |
| `GI_Global_SubG_GI_Loading_StartNewGamePlus` | 开始新游戏+ |
| `GI_Global_SubG_GI_Loading_StartUp` | 启动 |
| `GI_Global_SubG_GI_Loading_Teleport` | 传送 |
| `GI_Global_SubG_GI_Loading_ToiletClient` | Toilet 客户端 |
| `GI_Global_SubG_GI_Loading_ToiletDedicateServer` | Toilet 专用服务器 |
| `GI_Global_SubG_GI_Loading_ToiletListenServer` | Toilet 监听服务器 |
| `GI_Global_SubG_GI_Loading_ToiletStandAlone` | Toilet 单机模式 |
| `GI_Global_SubG_GI_Loading_TravelLevel` | 关卡旅行 |
| `GI_Global_SubG_GI_Loading_TravelToNextChapter` | 旅行到下一章 |
| `GI_Global_SubG_GI_Loading_UnKnowLevelTravel` | 未知关卡旅行 |

### 加载图实例

| 完整类型 | 说明 |
|----------|------|
| `GI_Loading_ChangeGameDefaultMap` | 更改默认地图 |
| `GI_Loading_IsInMap` | 判断在地图中 |
| `GI_Loading_LoadingUIFadeIn` | 加载 UI 淡入 |
| `GI_Loading_OpenLevelByIdInContext` | 按上下文 ID 打开关卡 |
| `GI_Loading_OpenLoadingScreen` | 打开加载画面 |
| `GI_Loading_RequestFadeAway` | 请求淡出 |
| `GI_Loading_SubG_GI_Loading_BattleLevelTravel` | 战斗关卡旅行子图 |
| `GI_Loading_SubG_GI_Loading_ClientEnvInit` | 客户端环境初始化子图 |
| `GI_Loading_SubG_GI_Loading_GSLogin` | GS 登录子图 |
| `GI_Loading_SubG_GI_Loading_HandleArchiveInTravelLevel` | 处理关卡旅行中的存档 |
| `GI_Loading_SubG_GI_Loading_HideLoadingUI` | 隐藏加载 UI |
| `GI_Loading_SubG_GI_Loading_PostLeaveLevel` | 离开关卡后子图 |
| `GI_Loading_SubG_GI_Loading_PreEnterLevel` | 进入关卡前子图 |
| `GI_Loading_SubG_GI_Loading_ResetGameInstanceDataAndSaveArchi` | 重置游戏实例数据并保存存档 |
| `GI_Loading_SubG_GI_Loading_ReStartGSLogin` | 重新启动 GS 登录 |
| `GI_Loading_SubG_GI_Loading_SaveArchiveAndWaitFinish` | 保存存档并等待完成 |
| `GI_Loading_WaitTick` | 等待 Tick |

### FSM 事务任务

| 完整类型 | 说明 |
|----------|------|
| `PS_Transaction_TransactionTask` | 事务任务 FSM 状态 |

---

## 8. GurCalliopeState 命名空间 -- Calliope FSM 自定义状态与检测条件

### 检测条件（约 30 个具体实现 + 1 个主类）

| 完整类型 | 说明 |
|----------|------|
| `CalliopeCustom_DetectCondition` | 检测条件主类（聚合所有子条件，包含 oneof 选择） |
| `CalliopeCustom_DetectCondition_AbnormalState` | 检测条件：异常状态 |
| `CalliopeCustom_DetectCondition_ActorYawRotation` | 检测条件：Actor 偏航旋转 |
| `CalliopeCustom_DetectCondition_CheckSurfaceType` | 检测条件：检查表面类型 |
| `CalliopeCustom_DetectCondition_CompareBuffLayer` | 检测条件：比较 Buff 层数 |
| `CalliopeCustom_DetectCondition_CompareGamePlusCount` | 检测条件：比较游戏+计数 |
| `CalliopeCustom_DetectCondition_CurrentBeAttackedStiffLevel` | 检测条件：当前被攻击硬直等级 |
| `CalliopeCustom_DetectCondition_CurSkillCostDmgNum` | 检测条件：当前技能消耗伤害数 |
| `CalliopeCustom_DetectCondition_CustomFsmState` | 检测条件：自定义 FSM 状态 |
| `CalliopeCustom_DetectCondition_DistanceFromMaster` | 检测条件：与主人的距离 |
| `CalliopeCustom_DetectCondition_DistanceFromNearestPlayer` | 检测条件：与最近玩家的距离 |
| `CalliopeCustom_DetectCondition_DistanceFromTarget` | 检测条件：与目标的距离 |
| `CalliopeCustom_DetectCondition_DurCastSkill` | 检测条件：技能持续施放中 |
| `CalliopeCustom_DetectCondition_FamilySpecifyUnitAttr` | 检测条件：家族指定单位属性 |
| `CalliopeCustom_DetectCondition_FamilyUnitAliveNum` | 检测条件：家族存活单位数量 |
| `CalliopeCustom_DetectCondition_FsmState` | 检测条件：FSM 状态 |
| `CalliopeCustom_DetectCondition_GlobalCastSkillCount` | 检测条件：全局施放技能计数 |
| `CalliopeCustom_DetectCondition_HasBuff` | 检测条件：是否拥有 Buff |
| `CalliopeCustom_DetectCondition_HasStoryCanTalkInThisLevel` | 检测条件：关卡中是否有可对话故事 |
| `CalliopeCustom_DetectCondition_LastBeAttackedStiffLevel` | 检测条件：上一次被攻击硬直等级 |
| `CalliopeCustom_DetectCondition_PlayerLeisureOverTime` | 检测条件：玩家空闲超时 |
| `CalliopeCustom_DetectCondition_Random` | 检测条件：随机概率 |
| `CalliopeCustom_DetectCondition_SimpleState` | 检测条件：简单状态 |
| `CalliopeCustom_DetectCondition_SkillCanCast` | 检测条件：技能是否可以施放 |
| `CalliopeCustom_DetectCondition_SkillCoolDown` | 检测条件：技能冷却中 |
| `CalliopeCustom_DetectCondition_SocketUnitsDead` | 检测条件：Socket 单位死亡 |
| `CalliopeCustom_DetectCondition_SpecifyResIdUnitsDead` | 检测条件：指定资源 ID 单位死亡 |
| `CalliopeCustom_DetectCondition_StoryInCollingOffPeriod` | 检测条件：故事冷却期 |
| `CalliopeCustom_DetectCondition_TargetInAngleRange` | 检测条件：目标在角度范围内 |
| `CalliopeCustom_DetectCondition_UnitActived` | 检测条件：单位已激活 |
| `CalliopeCustom_DetectCondition_UnitAttr` | 检测条件：单位属性 |
| `CalliopeCustom_DetectCondition_UnitInActived` | 检测条件：单位未激活 |
| `CalliopeCustom_DetectCondition_UnitState` | 检测条件：单位状态 |

### 自定义数据结构

| 完整类型 | 说明 |
|----------|------|
| `CalliopeCustom_FBossPhaseInfo` | Boss 阶段信息（是否拥有阶段表现） |
| `CalliopeCustom_FChildActorActionInfo` | 子 Actor 动作信息 |
| `CalliopeCustom_FTamerFamilyMatchChildInfo` | 驯兽师家族匹配子信息 |

---

## 9. GurGsPersistent 命名空间 -- 游戏状态持久化数据

此为存档系统核心模块，所有类型均实现 `IMessage` protobuf 接口。按前缀分为三大组 + 辅助类型。

### 9.1 BGC（Big Global Container）-- 全局持久容器

| 完整类型 | 说明 |
|----------|------|
| `BGC_CollectionGroupData` | 全局：收集组数据 |
| `BGC_GameStateTestData` | 全局：游戏状态测试数据 |
| `BGC_OnlineAssistData` | 全局：在线辅助数据 |
| `BGC_PigsyStoryData` | 全局：猪八戒故事数据 |
| `BGC_PlayerDeathData` | 全局：玩家死亡数据 |
| `BGC_PlayerGuideData` | 全局：玩家引导数据 |

### 9.2 BPC（Big Player Container）-- 玩家持久容器

| 完整类型 | 说明 |
|----------|------|
| `BPC_MapSymbolData` | 玩家：地图标记数据 |
| `BPC_PlayerAttrData` | 玩家：玩家属性数据 |
| `BPC_PlayerRoleData` | 玩家：玩家角色数据 |
| `BPC_RebirthPointData` | 玩家：重生点数据 |
| `BPC_TransData` | 玩家：变身数据 |

### 9.3 BUC（Big Unit Container）-- 单位持久容器

| 完整类型 | 说明 |
|----------|------|
| `BUC_ActorInitData` | 单位：Actor 初始化数据 |
| `BUC_CollectionData` | 单位：收集数据 |
| `BUC_GamePlusSpawnData` | 单位：游戏+生成数据 |
| `BUC_InteractData` | 单位：交互数据 |
| `BUC_LifeSavingData` | 单位：救命数据 |
| `BUC_ReplicateTestData` | 单位：复制测试数据 |
| `BUC_TaskCollectionData` | 单位：任务收集数据 |

### 9.4 聚合容器

| 完整类型 | 说明 |
|----------|------|
| `PersistentBGCData` | 聚合所有 BGC 数据的顶层容器 |
| `PersistentBPCData` | 聚合所有 BPC 数据的顶层容器 |
| `PersistentBUCData` | 聚合所有 BUC 数据的顶层容器 |
| `PersistentBUCDataWithLevel` | 带关卡信息的 BUC 聚合容器 |
| `PersistentECSData` | ECS 持久数据聚合容器 |

### 9.5 玩家事务数据

| 完整类型 | 说明 |
|----------|------|
| `PlayerTransactionBase` | 玩家事务基类 |
| `PlayerTransactionInteract` | 玩家交互事务 |
| `PlayerTransactionInteract2` | 玩家交互事务 2 |
| `PlayerTransactionTaskBase` | 玩家任务事务基类 |
| `PlayerTransactionTask_RequestInteractObjLock` | 请求交互对象锁定事务 |

### 9.6 玩家辅助数据

| 完整类型 | 说明 |
|----------|------|
| `PlayerFabaoCd` | 法宝冷却数据 |
| `PlayerLifeSavingHairCd` | 救命毫毛冷却数据 |
| `PlayerLifeSavingHairInfo` | 救命毫毛信息 |
| `PlayerMagicSkillCd` | 法术技能冷却数据 |
| `PlayerPersistentAttr` | 玩家持久属性 |
| `KeyMonsterMeetCount` | 关键怪物遭遇次数 |

### 9.7 单位辅助数据

| 完整类型 | 说明 |
|----------|------|
| `BuffInstData` | Buff 实例数据 |
| `ChallengeInfo` | 挑战信息 |
| `CollectionGroupDataInfo` | 收集组数据信息 |
| `FBirthPointInfo` | 出生点信息 |
| `FCrusadeUnitInfo` | 讨伐单位信息 |
| `FRepInnerClass` | 复制内部类 |
| `FRepTestClass` | 复制测试类 |
| `FRotator` | 旋转量 |
| `FVector` | 向量 |
| `GSUnitBookData` | 游戏状态单位手册数据 |
| `UnitHatredTargetInfo` | 单位仇恨目标信息 |
| `UnitLockTargetInfo` | 单位锁定目标信息 |

### 9.8 增量同步消息（DeltaMsg）

**DictDeltaMsg -- 字典增量消息：**

命名模式 `DictDeltaMsg<KeyType>_<ValueType>`：

| 类型 |
|------|
| `DictDeltaMsgEBGUAttrFloat_Float` |
| `DictDeltaMsgEBGUSimpleState_Int` |
| `DictDeltaMsgEPropType_UInt` |
| `DictDeltaMsgEquipPosition_Int` |
| `DictDeltaMsgInt_BindListBindListEntity` |
| `DictDeltaMsgInt_Bool` |
| `DictDeltaMsgInt_BuffInstData` |
| `DictDeltaMsgInt_ChallengeInfo` |
| `DictDeltaMsgInt_CollectionGroupDataInfo` |
| `DictDeltaMsgInt_EChallengeState` |
| `DictDeltaMsgInt_Entity` |
| `DictDeltaMsgInt_GSUnitBookData` |
| `DictDeltaMsgInt_Int` |
| `DictDeltaMsgSpellType_Int` |
| `DictDeltaMsgString_BindListString` |
| `DictDeltaMsgString_Bool` |
| `DictDeltaMsgString_EMapSymbolActiveState` |
| `DictDeltaMsgString_EMapSymbolState` |
| `DictDeltaMsgString_FCrusadeUnitInfo` |
| `DictDeltaMsgString_Int` |
| `DictDeltaMsgString_String` |

**ListDeltaMsg -- 列表增量消息：**

| 类型 |
|------|
| `ListDeltaMsgBindListEntity` |
| `ListDeltaMsgBindListUnitLockTargetInfo` |
| `ListDeltaMsgBool` |
| `ListDeltaMsgEntity` |
| `ListDeltaMsgFloat` |
| `ListDeltaMsgFRepInnerClass` |
| `ListDeltaMsgInt` |
| `ListDeltaMsgPlayerFabaoCd` |
| `ListDeltaMsgPlayerLifeSavingHairCd` |
| `ListDeltaMsgPlayerMagicSkillCd` |
| `ListDeltaMsgPlayerPersistentAttr` |
| `ListDeltaMsgPlayerTransactionBase` |
| `ListDeltaMsgPlayerTransactionTaskBase` |
| `ListDeltaMsgString` |
| `ListDeltaMsgUnitHatredTargetInfo` |
| `ListDeltaMsgUnitLockTargetInfo` |

**ListWNRDeltaMsg -- 不删除元素的列表增量消息（With No Remove）：**

| 类型 |
|------|
| `ListWNRDeltaMsgBindListInt` |
| `ListWNRDeltaMsgEntity` |
| `ListWNRDeltaMsgFloat` |
| `ListWNRDeltaMsgInt` |

---

## 10. GurGsReplicate 命名空间 -- 游戏状态网络复制数据

所有类型为 protobuf 消息，用于战斗数据网络同步。

| 完整类型 | 说明 |
|----------|------|
| `ABPHelperData` | ABP 辅助数据 |
| `AttrContainer` | 属性容器（包含浮点属性列表） |
| `BuffData` | Buff 同步数据 |
| `ChargeSkillData` | 蓄力技能同步数据 |
| `CircusControlData` | 马戏团控制数据 |
| `FallDyingData` | 坠落死亡数据 |
| `GameStateTestData` | 游戏状态测试数据 |
| `InteractData` | 交互同步数据 |
| `LevelAuthorityData` | 关卡权限数据 |
| `LevelBattleData` | 关卡战斗数据 |
| `MontageSyncData` | 蒙太奇同步数据 |
| `ObjActorMovementData` | 对象 Actor 移动数据 |
| `OnlineChallengeData` | 在线挑战数据 |
| `PlayerStateTestData` | 玩家状态测试数据 |
| `PredictionTestData` | 预测测试数据 |
| `ProjectileBasicData` | 弹道基础数据 |
| `RepDataAll` | 聚合所有复制数据的顶层容器 |
| `ReplicateTestData` | 复制测试数据 |
| `RoleBaseData` | 角色基础数据 |
| `SimpleStateData` | 简单状态数据 |
| `TargetInfoData` | 目标信息数据 |
| `TransactionData` | 事务数据 |
| `UnitHatredData` | 单位仇恨数据 |
| `UnitStateData` | 单位状态数据 |

---

## 11. GurGsStruct 命名空间 -- 游戏状态共享结构 & 增量同步消息

### 11.1 基础数据结构

| 完整类型 | 说明 |
|----------|------|
| `FVector` | 三维向量（X, Y, Z） |
| `FRotator` | 旋转量 |
| `FBirthPointInfo` | 出生点信息 |
| `FCrusadeUnitInfo` | 讨伐单位信息 |
| `FRepInnerClass` | 复制内部类 |
| `FRepTestClass` | 复制测试类 |
| `FTestPersistence` | 持久性测试 |

### 11.2 共享数据

| 完整类型 | 说明 |
|----------|------|
| `BuffInstData` | Buff 实例数据 |
| `ChallengeInfo` | 挑战信息 |
| `CollectionGroupDataInfo` | 收集组数据信息 |
| `GSUnitBookData` | 单位手册数据 |
| `PlayerMagicSkillCd` | 玩家法术冷却 |
| `PlayerPersistentAttr` | 玩家持久属性 |
| `PlayerTransactionBase` | 玩家事务基类 |
| `PlayerTransactionTaskBase` | 玩家任务事务基类 |
| `UnitHatredTargetInfo` | 单位仇恨目标信息 |
| `UnitLockTargetInfo` | 单位锁定目标信息 |

### 11.3 OPType 枚举

| 完整类型 | 值 | 说明 |
|----------|----|------|
| `OPType` | Add / Remove / Modify / Clear / SetNull / ChangeRef | 增量操作类型 |

附带 `OPTypeSyncWrapper` 和 `OPTypeTupleWrapper`。

### 11.4 增量同步消息

与 `GurGsPersistent` 共享相同的 `DictDeltaMsg*`、`ListDeltaMsg*`、`ListWNRDeltaMsg*` 类型定义（文件重复但内容一致），以及额外 `ClassDeltaMsg*` 类型：

**ClassDeltaMsg -- 类增量消息：**

| 类型 | 说明 |
|------|------|
| `ClassDeltaMsgBuffInstData` | BuffInstData 类增量消息 |
| `ClassDeltaMsgChallengeInfo` | ChallengeInfo 类增量消息 |
| `ClassDeltaMsgCollectionGroupDataInfo` | CollectionGroupDataInfo 类增量消息 |
| `ClassDeltaMsgFBirthPointInfo` | FBirthPointInfo 类增量消息 |
| `ClassDeltaMsgFCrusadeUnitInfo` | FCrusadeUnitInfo 类增量消息 |
| `ClassDeltaMsgFRepInnerClass` | FRepInnerClass 类增量消息 |
| `ClassDeltaMsgFRepTestClass` | FRepTestClass 类增量消息 |
| `ClassDeltaMsgGSUnitBookData` | GSUnitBookData 类增量消息 |
| `ClassDeltaMsgPlayerMagicSkillCd` | PlayerMagicSkillCd 类增量消息 |
| `ClassDeltaMsgPlayerPersistentAttr` | PlayerPersistentAttr 类增量消息 |
| `ClassDeltaMsgPlayerTransactionBase` | PlayerTransactionBase 类增量消息 |
| `ClassDeltaMsgPlayerTransactionTaskBase` | PlayerTransactionTaskBase 类增量消息 |
| `ClassDeltaMsgUnitHatredTargetInfo` | UnitHatredTargetInfo 类增量消息 |
| `ClassDeltaMsgUnitLockTargetInfo` | UnitLockTargetInfo 类增量消息 |

---

## 12. 命名模式说明

本程序集中的类型遵循以下命名惯例：

### 枚举模式
```
E<领域><特性>                    -- BtlB1 / BtlShare 命名空间的 UE 枚举
EB2D<领域><特性>                 -- BtlU3 / BtlX2 的 2D 战斗枚举
```
- 标注 `[UEnum]`、`[BlueprintType]`、`[USharpPath]` 属性
- 每个枚举附带 `SyncWrapper`（protobuf 消息封装）和 `TupleWrapper`

### 数据描述模式（FUSt）
```
FUSt<领域>Desc                   -- 单行配置条目（protobuf 消息）
TBFUSt<领域>Desc                 -- 表级封装（包含 RepeatedField<FUSt*>）
```
- 所有类均实现 `IMessage<T>`、`IEquatable<T>`、`IDeepCloneable<T>`
- B2D 版本使用后缀 `B2D`：`FUStB2D*Desc` / `TBFUStB2D*Desc`

### 持久化容器模式
```
BGC_<领域>Data                   -- Big Global Container（全局）
BPC_<领域>Data                   -- Big Player Container（玩家）
BUC_<领域>Data                   -- Big Unit Container（单位）
PersistentBGCData                -- BGC 聚合根
PersistentBPCData                -- BPC 聚合根
PersistentBUCData                -- BUC 聚合根
PersistentBUCDataWithLevel       -- 带关卡 BUC
PersistentECSData                -- ECS 持久数据
```

### 增量同步消息模式
```
DictDeltaMsg<Key>_<Value>        -- 字典增量（key-value 对）
ListDeltaMsg<Type>               -- 列表增量
ListWNRDeltaMsg<Type>            -- 列表增量（不删除）
ClassDeltaMsg<Type>              -- 类对象增量
```

### Calliope FSM 模式
```
GI_<图类型>_<节点类型>_<名称>    -- Graph Instance
PS_<状态机>_<状态名>             -- Playable State
CalliopeCustom_DetectCondition[_<条件名>]  -- 检测条件
CalliopeCustom_F<结构名>         -- 自定义结构体
```

---

## 13. 继承与实现关系

几乎所有 protobuf 生成类遵循相同的接口模式：

```
public sealed class <ClassName> : IMessage<ClassName>, IMessage, IEquatable<ClassName>, IDeepCloneable<ClassName>
{
    public static MessageParser<ClassName> Parser { get; }
    public ClassName() { }
    public ClassName(ClassName other) { }
    public ClassName Clone() { }
    // ... 字段属性（get/set）
    // ... 序列化方法（CalculateSize, MergeFrom, WriteTo）
    // ... Equals, GetHashCode, ToString
}
```

少数的例外：

| 类型 | 特点 |
|------|------|
| `BG_ProtobufDataAPI<T>` | 泛型类，非 protobuf 消息 |
| `BG_ProtobufDataLoader` | 普通单例类 |
| `B1EnumInfo` / `EnumInfo` | `static class`，枚举查询 |
| `FILE_PROCESS_STATUS` / `ParserManagerLogVerbosity` / `OPType` | 纯枚举 |
| `IBG_TableToolP4Helper` | interface |
| 异常类 | 继承自 `Exception` |
| `SerializedManagedUnrealModuleInfo` | 实现 `ISerializedManagedUnrealModuleInfo` |
| `EAbnormalDispModifyType` 等 `enum` | `: byte` 枚举 |

---

## 14. 文件大小与行数概览

- 根目录文件数: 1
- `b1/`: 5 文件 + 2 子目录
- `BaseU3/`: 约 60 文件
- `BtlB1/`: 约 460 文件（最大模块）
- `BtlShare/`: 约 200 文件
- `BtlU3/`: 约 140 文件
- `BtlX2/`: 约 140 文件
- `GurCalliopeFsm/`: 47 文件
- `GurCalliopeState/`: 44 文件
- `GurGsPersistent/`: 约 130 文件
- `GurGsReplicate/`: 22 文件
- `GurGsStruct/`: 约 68 文件
- `X2/Base/`: 约 60 文件
- 总计: **1651 个 .cs 文件**
