# B1UI_GSE.Script 类导航

> 项目路径: `GameDll-src/B1UI_GSE.Script/`
> 总文件数: 2134 个 `.cs` 文件
> 程序集名称: `B1UI.Script`
> 命名空间数量: 22 个

---

## 模块结构

```
B1UI_GSE.Script/
├── Properties/                        # 程序集信息 (AssemblyInfo.cs)
├── (根目录) *.cs                      # 3 个顶层静态工具类
├── b1/                                # [205 文件] 游戏主逻辑 (异步管线/FSM/玩家管理)
│   ├── GSSvc/                         # [12 文件] 游戏服务绑定 (战斗/背包/属性)
│   ├── Online/CSRpc/                  # [2 文件] CS RPC 消息适配器
│   └── Util/                          # [8 文件] 工具类 (HTTP/调试配置/编码)
├── b1x/                               # [1 文件] 调试辅助
├── B1UI/                              # [5 文件] 核心 UI 入口
│   ├── GSSvc/                         # [4 文件] 游戏服务对象 (商店/战斗/GM)
│   ├── Online/                        # [3 文件] 网络计时器/房间服务
│   ├── GSUI/                          # [795 文件] UI 视图层 (VI*视图/DS*数据存储/枚举)
│   └── Script/                        # [30 文件] 关卡过渡/加载系统
│       └── GSUI/Util/                 # [1 文件] UI 音频工具
├── BtlSvr/Script/                     # [2 文件] 战斗服务器 FSM 注册
├── CommB1/                            # [869 文件] 数据合约类型及 Protobuf 包装代码
├── CsB1/                              # [13 文件] RPC 系统/属性/玩家执行器
├── GS/CSNet/                          # [11 文件] 客户端-服务器网络基础设施
├── GSE/                               # UI 框架层
│   ├── GSICore/                       # [10 文件] 核心工具 (委托/数学/本地存档/Tick)
│   │   └── Event/                     # [29 文件] UI 动画/事件系统 (补间/过渡/延时)
│   ├── GSUI/                          # [66 文件] 页面管理框架 (GSUIPage/UiLayer/Action)
│   ├── Mgr/                           # [1 文件] 音频管理器
│   └── Script/                        # [1 文件] 内部实现细节
├── GsOnline/                          # [39 文件] Online session 包装列表
├── GsOnlineFriend/                    # [19 文件] Online 好友系统
└── Online/Friend/                     # [4 文件] 好友接口/工厂
```

---

## 命名空间清单

| 命名空间 | 文件数 | 用途 |
|----------|--------|------|
| *(全局)* | 3 | 静态工具入口 |
| `b1` | 205 | 主游戏逻辑 |
| `b1.GSSvc` | 12 | 游戏服务绑定 |
| `b1.Online.CSRpc` | 2 | CS RPC 适配器 |
| `b1.Util` | 8 | 工具类 |
| `b1x` | 1 | 调试 XDumper |
| `B1UI` | 5 | 核心 UI 入口 |
| `B1UI.GSSvc` | 4 | 游戏服务对象 |
| `B1UI.Online` | 3 | 网络计时器 |
| `B1UI.GSUI` | 795 | UI 视图和数据存储 |
| `B1UI.Script` | 30 | 关卡过渡系统 |
| `B1UI.Script.GSUI.Util` | 1 | UI 音频工具 |
| `BtlSvr.Script` | 2 | 战斗服务器 FSM |
| `CommB1` | 869 | 数据合约与包装 |
| `CsB1` | 13 | RPC 系统 |
| `GS.CSNet` | 11 | 网络基础设施 |
| `GSE.GSICore` | 10 | 核心工具 |
| `GSE.GSICore.Event` | 29 | 动画事件系统 |
| `GSE.GSUI` | 66 | UI 页面框架 |
| `GSE.Mgr` | 1 | 音频管理 |
| `GSE.Script` | 1 | 内部实现 |
| `GsOnline` | 39 | Online session |
| `GsOnlineFriend` | 19 | Online 好友 |
| `Online.Friend` | 4 | 好友接口 |

---

## 类/接口/结构/枚举清单

### 一、根目录 (全局命名空间)

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `GSServerConfigFunUtil` | 静态类 | 服务器配置初始化：解析 JSON 配置并执行 GM 命令和控制台变量 | `GSServerConfigFunUtil.cs` |
| `GSUISettingFunUtil` | 静态类 | UI 设置功能工具：检查锁定状态/条件类型判定 | `GSUISettingFunUtil.cs` |
| `OSS_SettingReport` | 静态类 | OSS 设置上报：退出游戏/设置初始化/设置变更事件上报 | `OSS_SettingReport.cs` |

---

### 二、`b1` 命名空间 — 主游戏逻辑 (205 文件)

#### 2.1 异步管线 (Async Pipelines)

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `AsyncPipelineBase` | 抽象类 | - | 异步管线基类 | `AsyncPipelineBase.cs` |
| `PipelineResult` | 结构 | - | 管线执行结果 | `AsyncPipelineBase.cs` (嵌套) |
| `AsyncPipelineCreateParty` | 类 | `AsyncPipelineBase` | 创建队伍管线 | `AsyncPipelineCreateParty.cs` |
| `AsyncPipelineSearchParty` | 类 | `AsyncPipelineBase` | 搜索队伍管线 | `AsyncPipelineSearchParty.cs` |
| `CreateOrRejoinPartyPipeline` | 类 | `AsyncPipelineBase` | 创建或重新加入队伍管线 | `CreateOrRejoinPartyPipeline.cs` |
| `LeaderBattleReadyPipeline` | 类 | `AsyncPipelineBase` | 队长战斗就绪管线 | `LeaderBattleReadyPipeline.cs` |
| `LeaderCreateBattlePipeline` | 类 | `AsyncPipelineBase` | 队长创建战斗管线 | `LeaderCreateBattlePipeline.cs` |
| `LeaderCreatePartyTaskPipeline` | 类 | `AsyncPipelineBase` | 队长创建队伍任务管线 | `LeaderCreatePartyTaskPipeline.cs` |
| `MemberEnterBattlePipeline` | 类 | `AsyncPipelineBase` | 成员进入战斗管线 | `MemberEnterBattlePipeline.cs` |
| `MemberJoinPartyPipeline` | 类 | `AsyncPipelineBase` | 成员加入队伍管线 | `MemberJoinPartyPipeline.cs` |
| `MemberTaskReadyPipeline` | 类 | `AsyncPipelineBase` | 成员任务就绪管线 | `MemberTaskReadyPipeline.cs` |
| `OnlinePartyCleanPipeline` | 类 | `AsyncPipelineBase` | 在线队伍清理管线 | `OnlinePartyCleanPipeline.cs` |

#### 2.2 异步任务 (Async Tasks)

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `AsyncTaskBase` | 抽象类 | - | 异步任务基类 | `AsyncTaskBase.cs` |
| `AsyncTaskForGSRpc` | 抽象类 | `AsyncTaskBase` | GS RPC 异步任务基类 | `AsyncTaskForGSRpc.cs` |
| `AsyncTaskForSession` | 抽象类 | `AsyncTaskBase` | Session 异步任务基类 | `AsyncTaskForSession.cs` |
| `AsyncTaskMgr` | 静态类 | - | 异步任务管理器 | `AsyncTaskMgr.cs` |
| `AsyncTaskCreateSession` | 类 | `AsyncTaskForSession` | 创建 Session | `AsyncTaskCreateSession.cs` |
| `AsyncTaskExitSession` | 类 | `AsyncTaskForSession` | 退出 Session | `AsyncTaskExitSession.cs` |
| `AsyncTaskGetSession` | 类 | `AsyncTaskForSession` | 获取 Session | `AsyncTaskGetSession.cs` |
| `AsyncTaskJoinSession` | 类 | `AsyncTaskForSession` | 加入 Session | `AsyncTaskJoinSession.cs` |
| `AsyncTaskSearchRoomParty` | 类 | `AsyncTaskForGSRpc` | 搜索房间队伍 | `AsyncTaskSearchRoomParty.cs` |
| `RoomAsyncTaskCreateParty` | 类 | `AsyncTaskForGSRpc` | 房间创建队伍 | `RoomAsyncTaskCreateParty.cs` |
| `RoomAsyncTaskCreatePartyTask` | 类 | `AsyncTaskForGSRpc` | 房间创建队伍任务 | `RoomAsyncTaskCreatePartyTask.cs` |
| `RoomAsyncTaskExitParty` | 类 | `AsyncTaskForGSRpc` | 房间退出队伍 | `RoomAsyncTaskExitParty.cs` |
| `RoomAsyncTaskJoinParty` | 类 | `AsyncTaskForGSRpc` | 房间加入队伍 | `RoomAsyncTaskJoinParty.cs` |
| `RoomAsyncTaskLeaderUpdatePartyTask` | 类 | `AsyncTaskForGSRpc` | 队长更新队伍任务 | `RoomAsyncTaskLeaderUpdatePartyTask.cs` |
| `RoomAsyncTaskMemberUpdate` | 类 | `AsyncTaskForGSRpc` | 成员更新 | `RoomAsyncTaskMemberUpdate.cs` |
| `RoomAsyncTaskQueryRoleParty` | 类 | `AsyncTaskForGSRpc` | 查询角色队伍 | `RoomAsyncTaskQueryRoleParty.cs` |

#### 2.3 数据监听器 (Data Listeners) — 均为 internal

| 类型 | 说明 | 文件 |
|------|------|------|
| `ActorLegacyListListener` | 角色遗物列表监听 | `ActorLegacyListListener.cs` |
| `ActorMeditationListener` | 角色打坐监听 | `ActorMeditationListener.cs` |
| `ActorOwnSpellListListener` | 角色法术列表监听 | `ActorOwnSpellListListener.cs` |
| `ActorTalentListener` | 角色天赋监听 | `ActorTalentListener.cs` |
| `ActorTalentPointListener` | 角色天赋点监听 | `ActorTalentPointListener.cs` |
| `ActorWearSpellListListener` | 角色装备法术列表监听 | `ActorWearSpellListListener.cs` |
| `ActorXpChangeListener` | 角色经验值变化监听 | `ActorXpChangeListener.cs` |
| `AchievementListener` | 成就监听 | `AchievementListener.cs` |
| `BagAttrItemListener` | 背包属性物品监听 | `BagAttrItemListener.cs` |
| `BagEquipChangeListener` | 背包装备变化监听 | `BagEquipChangeListener.cs` |
| `BagItemChangeListener` | 背包物品变化监听 | `BagItemChangeListener.cs` |
| `BagMoneyChangeListener` | 背包金钱变化监听 | `BagMoneyChangeListener.cs` |
| `BagSoulSkillListener` | 背包魂技监听 | `BagSoulSkillListener.cs` |
| `BagWineChangeListener` | 背包酒变化监听 | `BagWineChangeListener.cs` |
| `CollectionCardListener` | 收藏卡牌监听 | `CollectionCardListener.cs` |
| `WearAccessoryListener` | 装备饰品监听 | `WearAccessoryListener.cs` |
| `WearEquipListener` | 装备武器监听 | `WearEquipListener.cs` |
| `WearSoulSkillListener` | 装备魂技监听 | `WearSoulSkillListener.cs` |

#### 2.4 加载 FSM 状态 (GI_Loading States) — 均为 public class, 继承 `FSMState_GI_LoadingBase`

| 类型 | 说明 | 文件 |
|------|------|------|
| `FSMState_GI_Loading_AdjustPSOCachePrecompileBatch` | 调整 PSO 缓存预编译批次 | `FSMState_GI_Loading_AdjustPSOCachePrecompileBatch.cs` |
| `FSMState_GI_Loading_CacheArchiveDataForClient` | 缓存存档数据至客户端 | `FSMState_GI_Loading_CacheArchiveDataForClient.cs` |
| `FSMState_GI_Loading_ChangeGameDefaultMap` | 更改游戏默认地图 | `FSMState_GI_Loading_ChangeGameDefaultMap.cs` |
| `FSMState_GI_Loading_CheckArchiveDataIsValid` | 检查存档数据有效性 | `FSMState_GI_Loading_CheckArchiveDataIsValid.cs` |
| `FSMState_GI_Loading_CheckGSSdkServerConfig` | 检查 GS SDK 服务器配置 | `FSMState_GI_Loading_CheckGSSdkServerConfig.cs` |
| `FSMState_GI_Loading_CheckGSSdkUserConfig` | 检查 GS SDK 用户配置 | `FSMState_GI_Loading_CheckGSSdkUserConfig.cs` |
| `FSMState_GI_Loading_CreateNewRoleData` | 创建新角色数据 | `FSMState_GI_Loading_CreateNewRoleData.cs` |
| `FSMState_GI_Loading_FillIsInToilet` | 填充"洗手间"状态 | `FSMState_GI_Loading_FillIsInToilet.cs` |
| `FSMState_GI_Loading_FillLocalBPCRoleData` | 填充本地 BPC 角色数据 | `FSMState_GI_Loading_FillLocalBPCRoleData.cs` |
| `FSMState_GI_Loading_FillLoginRoleData` | 填充登录角色数据 | `FSMState_GI_Loading_FillLoginRoleData.cs` |
| `FSMState_GI_Loading_FirstStartGameSettings` | 首次启动游戏设置 | `FSMState_GI_Loading_FirstStartGameSettings.cs` |
| `FSMState_GI_Loading_GetPreviewSeqPosition` | 获取预览序列位置 | `FSMState_GI_Loading_GetPreviewSeqPosition.cs` |
| `FSMState_GI_Loading_GSGBtlOnBattleDestroy` | GSG 战斗销毁 | `FSMState_GI_Loading_GSGBtlOnBattleDestroy.cs` |
| `FSMState_GI_Loading_GSGBtlOnBattleStart` | GSG 战斗开始 | `FSMState_GI_Loading_GSGBtlOnBattleStart.cs` |
| `FSMState_GI_Loading_GSGEnterBattleLevel` | GSG 进入战斗关卡 | `FSMState_GI_Loading_GSGEnterBattleLevel.cs` |
| `FSMState_GI_Loading_GSGEnterLevel` | GSG 进入关卡 | `FSMState_GI_Loading_GSGEnterLevel.cs` |
| `FSMState_GI_Loading_GSGExitLevel` | GSG 退出关卡 | `FSMState_GI_Loading_GSGExitLevel.cs` |
| `FSMState_GI_Loading_GSGOnBattleDestroy` | GSG 战斗销毁(事件) | `FSMState_GI_Loading_GSGOnBattleDestroy.cs` |
| `FSMState_GI_Loading_GSGOnBattleStart` | GSG 战斗开始(事件) | `FSMState_GI_Loading_GSGOnBattleStart.cs` |
| `FSMState_GI_Loading_GSGRecoverMuseum` | GSG 恢复博物馆 | `FSMState_GI_Loading_GSGRecoverMuseum.cs` |
| `FSMState_GI_Loading_GSGShowBattleUI` | GSG 显示战斗 UI | `FSMState_GI_Loading_GSGShowBattleUI.cs` |
| `FSMState_GI_Loading_GSGShowLoginUI` | GSG 显示登录 UI | `FSMState_GI_Loading_GSGShowLoginUI.cs` |
| `FSMState_GI_Loading_GSGShowWXlogin` | GSG 显示微信登录 | `FSMState_GI_Loading_GSGShowWXlogin.cs` |
| `FSMState_GI_Loading_InBenchMark` | 基准测试中 | `FSMState_GI_Loading_InBenchMark.cs` |
| `FSMState_GI_Loading_InitClientPlayerContainer` | 初始化客户端玩家容器 | `FSMState_GI_Loading_InitClientPlayerContainer.cs` |
| `FSMState_GI_Loading_InitDispLibWorld` | 初始化可视化库世界 | `FSMState_GI_Loading_InitDispLibWorld.cs` |
| `FSMState_GI_Loading_InitLocalPlayerContainer` | 初始化本地玩家容器 | `FSMState_GI_Loading_InitLocalPlayerContainer.cs` |
| `FSMState_GI_Loading_InitLocalRoleDataForPreviewSeq` | 初始化预览序列本地角色数据 | `FSMState_GI_Loading_InitLocalRoleDataForPreviewSeq.cs` |
| `FSMState_GI_Loading_InitNewArchiveData` | 初始化新存档数据 | `FSMState_GI_Loading_InitNewArchiveData.cs` |
| `FSMState_GI_Loading_InitPlayerContainer` | 初始化玩家容器 | `FSMState_GI_Loading_InitPlayerContainer.cs` |
| `FSMState_GI_Loading_LoadChapterViewLevel` | 加载章节视图关卡 | `FSMState_GI_Loading_LoadChapterViewLevel.cs` |
| `FSMState_GI_Loading_LoadCharacterViewLevel` | 加载角色视图关卡 | `FSMState_GI_Loading_LoadCharacterViewLevel.cs` |
| `FSMState_GI_Loading_LoadCommLevel` | 加载通用关卡 | `FSMState_GI_Loading_LoadCommLevel.cs` |
| `FSMState_GI_Loading_LoadingUIFadeAway` | 加载 UI 渐隐 | `FSMState_GI_Loading_LoadingUIFadeAway.cs` |
| `FSMState_GI_Loading_LoadingUIFadeIn` | 加载 UI 渐显 | `FSMState_GI_Loading_LoadingUIFadeIn.cs` |
| `FSMState_GI_Loading_LoadingUILinearTime` | 加载 UI 线性计时 | `FSMState_GI_Loading_LoadingUILinearTime.cs` |
| `FSMState_GI_Loading_LoadingUIWaitUserInput` | 加载 UI 等待用户输入 | `FSMState_GI_Loading_LoadingUIWaitUserInput.cs` |
| `FSMState_GI_Loading_LoginByRoleData` | 用角色数据登录 | `FSMState_GI_Loading_LoginByRoleData.cs` |
| `FSMState_GI_Loading_MarkFirstStartGameFlag` | 标记首次启动标志 | `FSMState_GI_Loading_MarkFirstStartGameFlag.cs` |
| `FSMState_GI_Loading_MarkNewGameplusReady` | 标记新游戏+就绪 | `FSMState_GI_Loading_MarkNewGameplusReady.cs` |
| `FSMState_GI_Loading_OpenLevelDefaultBattle` | 打开默认战斗关卡 | `FSMState_GI_Loading_OpenLevelDefaultBattle.cs` |
| `FSMState_GI_Loading_OpenLevelStartUp` | 打开启动关卡 | `FSMState_GI_Loading_OpenLevelStartUp.cs` |
| `FSMState_GI_Loading_PausePsoCachePrecompile` | 暂停 PSO 缓存预编译 | `FSMState_GI_Loading_PausePsoCachePrecompile.cs` |
| `FSMState_GI_Loading_PlayGoDownloadIncompleteImpl` | PlayGo 下载未完成处理 | `FSMState_GI_Loading_PlayGoDownloadIncompleteImpl.cs` |
| `FSMState_GI_Loading_PlayPreviewSeq` | 播放预览序列 | `FSMState_GI_Loading_PlayPreviewSeq.cs` |
| `FSMState_GI_Loading_PlayerDataInitPreEnterLevel` | 玩家数据初始化预进入关卡 | `FSMState_GI_Loading_PlayerDataInitPreEnterLevel.cs` |
| `FSMState_GI_Loading_PostLogin` | 登录后处理 | `FSMState_GI_Loading_PostLogin.cs` |
| `FSMState_GI_Loading_PreLogin` | 登录前处理 | `FSMState_GI_Loading_PreLogin.cs` |
| `FSMState_GI_Loading_ReadLatestArchive` | 读取最近存档 | `FSMState_GI_Loading_ReadLatestArchive.cs` |
| `FSMState_GI_Loading_RequestTemplateCreateArchiveData` | 请求模板创建存档数据 | `FSMState_GI_Loading_RequestTemplateCreateArchiveData.cs` |
| `FSMState_GI_Loading_RequestTemplateLoadArchiveData` | 请求模板加载存档数据 | `FSMState_GI_Loading_RequestTemplateLoadArchiveData.cs` |
| `FSMState_GI_Loading_RequestTemplatePerformActionsFromArchive` | 请求模板执行存档操作 | `FSMState_GI_Loading_RequestTemplatePerformActionsFromArchive.cs` |
| `FSMState_GI_Loading_RequestTemplatePostSaveArchiveFinish` | 请求模板保存存档完成 | `FSMState_GI_Loading_RequestTemplatePostSaveArchiveFinish.cs` |
| `FSMState_GI_Loading_RequestTemplatePreSaveArchive` | 请求模板预保存存档 | `FSMState_GI_Loading_RequestTemplatePreSaveArchive.cs` |
| `FSMState_GI_Loading_Reset820DemoGameData` | 重置 820 演示游戏数据 | `FSMState_GI_Loading_Reset820DemoGameData.cs` |
| `FSMState_GI_Loading_ResetGameInstanceData` | 重置游戏实例数据 | `FSMState_GI_Loading_ResetGameInstanceData.cs` |
| `FSMState_GI_Loading_ResumePsoCachePrecompile` | 恢复 PSO 缓存预编译 | `FSMState_GI_Loading_ResumePsoCachePrecompile.cs` |
| `FSMState_GI_Loading_SaveArchiveAndWaitFinish` | 保存存档并等待完成 | `FSMState_GI_Loading_SaveArchiveAndWaitFinish.cs` |
| `FSMState_GI_Loading_ServerBattleReady` | 服务器战斗就绪 | `FSMState_GI_Loading_ServerBattleReady.cs` |
| `FSMState_GI_Loading_SetPSOCacheUsageMask` | 设置 PSO 缓存使用掩码 | `FSMState_GI_Loading_SetPSOCacheUsageMask.cs` |
| `FSMState_GI_Loading_ShowAgreementPolicyInStartGame` | 启动游戏时显示协议政策 | `FSMState_GI_Loading_ShowAgreementPolicyInStartGame.cs` |
| `FSMState_GI_Loading_ShowArchiveMarkInStartGame` | 启动游戏时显示存档标记 | `FSMState_GI_Loading_ShowArchiveMarkInStartGame.cs` |
| `FSMState_GI_Loading_ShowGameDisclaimerInStartGame` | 启动游戏时显示免责声明 | `FSMState_GI_Loading_ShowGameDisclaimerInStartGame.cs` |
| `FSMState_GI_Loading_ShowHealthyGamingAdviceInStartGame` | 启动游戏时显示健康游戏提示 | `FSMState_GI_Loading_ShowHealthyGamingAdviceInStartGame.cs` |

#### 2.5 加载 FSM 条件 (GI_Loading Conditions) — 均为 public class, 继承 `FSMConditionBase`

| 类型 | 说明 | 文件 |
|------|------|------|
| `FSMCondition_GI_Loading_DetermineTravelLevelByHandlingArchive` | 通过存档判断过渡类型 | `FSMCondition_GI_Loading_DetermineTravelLevelByHandlingArchiv.cs` |
| `FSMCondition_GI_Loading_HasPlayerLoginBtlSvr` | 玩家是否已登录战斗服务器 | `FSMCondition_GI_Loading_HasPlayerLoginBtlSvr.cs` |
| `FSMCondition_GI_Loading_IsFirstStartGame` | 是否首次启动游戏 | `FSMCondition_GI_Loading_IsFirstStartGame.cs` |
| `FSMCondition_GI_Loading_IsInPreviewSeqContext` | 是否在预览序列上下文中 | `FSMCondition_GI_Loading_IsInPreviewSeqContext.cs` |
| `FSMCondition_GI_Loading_IsNeedCloseLoadingScreen` | 是否需要关闭加载屏幕 | `FSMCondition_GI_Loading_IsNeedCloseLoadingScreen.cs` |
| `FSMCondition_GI_Loading_IsNeedOpenLoadingScreen` | 是否需要打开加载屏幕 | `FSMCondition_GI_Loading_IsNeedOpenLoadingScreen.cs` |
| `FSMCondition_GI_Loading_IsNeedPostLeaveLevel` | 是否需要后离开关卡 | `FSMCondition_GI_Loading_IsNeedPostLeaveLevel.cs` |
| `FSMCondition_GI_Loading_IsNeedPreEnterLevel` | 是否需要预进入关卡 | `FSMCondition_GI_Loading_IsNeedPreEnterLevel.cs` |
| `FSMCondition_GI_Loading_SwitchFillContextArchiveDataType` | 切换填充上下文存档数据类型 | `FSMCondition_GI_Loading_SwitchFillContextArchiveDataType.cs` |
| `FSMCondition_GI_Loading_SwitchSaveArchiveDegree` | 切换存档等级 | `FSMCondition_GI_Loading_SwitchSaveArchiveDegree.cs` |

#### 2.6 加载 FSM 枚举 (GI_Loading Enums)

| 类型 | 说明 | 文件 |
|------|------|------|
| `EGI_Loading_DetermineTravelLevelByHandlingArchive_Result` | 存档判断过渡类型结果 | `EGI_Loading_DetermineTravelLevelByHandlingArchive_Result.cs` |
| `EGI_Loading_HasPlayerLoginBtlSvr_Result` | 登录战斗服务器结果 | `EGI_Loading_HasPlayerLoginBtlSvr_Result.cs` |
| `EGI_Loading_IsFirstStartGame_Result` | 首次启动判断结果 | `EGI_Loading_IsFirstStartGame_Result.cs` |
| `EGI_Loading_IsInPreviewSeqContext_Result` | 预览序列上下文判断结果 | `EGI_Loading_IsInPreviewSeqContext_Result.cs` |
| `EGI_Loading_IsNeedCloseLoadingScreen_Result` | 关闭加载屏幕判断结果 | `EGI_Loading_IsNeedCloseLoadingScreen_Result.cs` |
| `EGI_Loading_IsNeedOpenLoadingScreen_Result` | 打开加载屏幕判断结果 | `EGI_Loading_IsNeedOpenLoadingScreen_Result.cs` |
| `EGI_Loading_IsNeedPostLeaveLevel_Result` | 后离开关卡判断结果 | `EGI_Loading_IsNeedPostLeaveLevel_Result.cs` |
| `EGI_Loading_IsNeedPreEnterLevel_Result` | 预进入关卡判断结果 | `EGI_Loading_IsNeedPreEnterLevel_Result.cs` |
| `EGI_Loading_SwitchFillContextArchiveDataType_Result` | 切换填充类型结果 | `EGI_Loading_SwitchFillContextArchiveDataType_Result.cs` |
| `EGI_Loading_SwitchSaveArchiveDegree_Result` | 切换存档等级结果 | `EGI_Loading_SwitchSaveArchiveDegree_Result.cs` |

#### 2.7 核心游戏对象

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `GamePlayer` | 类 | - | 游戏玩家 | `GamePlayer.cs` |
| `PlayerContainer` | 类 | - | 玩家容器 | `PlayerContainer.cs` |
| `PlayerAttrMgr` | 类 | - | 玩家属性管理器 | `PlayerAttrMgr.cs` |
| `PlayerHelper` | 类 | - | 玩家辅助工具 | `PlayerHelper.cs` |
| `PlayerStatics` | 类 | - | 玩家统计 | `PlayerStatics.cs` |
| `PlayerDataListener` | 类 | - | 玩家数据监听器 | `PlayerDataListener.cs` |
| `PlayerOssUtil` | 静态类 | - | 玩家 OSS (运营支撑) 工具 | `PlayerOssUtil.cs` |
| `PlayerDropUtil` | 静态类 | - | 玩家掉落工具 | `PlayerDropUtil.cs` |
| `PlayerDropParam` | 类 | `IDropParam` | 玩家掉落参数 | `PlayerDropParam.cs` |
| `SimulationDropUtil` | 静态类 | - | 模拟掉落工具 | `SimulationDropUtil.cs` |
| `SimulationDropParam` | 类 | `IDropParam` | 模拟掉落参数 | `SimulationDropParam.cs` |
| `IDropParam` | 接口 | - | 掉落参数接口 | `IDropParam.cs` |
| `DropUtil` | 静态类 | - | 掉落工具 | `DropUtil.cs` |
| `PlayerAsyncEventEntity` | 类 | - | 玩家异步事件实体 | `PlayerAsyncEventEntity.cs` |
| `PlayerAsyncEventMgr` | 类 | - | 玩家异步事件管理器 | `PlayerAsyncEventMgr.cs` |
| `PlayerAsyncEventParam` | 抽象类 | - | 玩家异步事件参数基类 | `PlayerAsyncEventParam.cs` |

#### 2.8 枚举与值类型

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `BaseChangeReason` | 枚举 | 基础变更原因 | `BaseChangeReason.cs` |
| `ChangeFrom` | 枚举 | 变更来源 | `ChangeFrom.cs` |
| `ChangeReason` | 类 | 变更原因容器 | `ChangeReason.cs` |
| `GSNetworkState` | 枚举 | 网络状态 | `GSNetworkState.cs` |
| `GSPartyState` | 枚举 | 队伍状态 | `GSPartyState.cs` |
| `PlayerAsyncEventType` | 枚举 | 玩家异步事件类型 | `PlayerAsyncEventType.cs` |
| `PlayerDataMode` | 枚举 | 玩家数据模式 | `PlayerDataMode.cs` |

#### 2.9 联机/队伍管理

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `BattleToFluxServer` | 类 | `GSDObject` | 战斗 -> Flux 服务器 | `BattleToFluxServer.cs` |
| `BattleToFluxShared` | 类 | `GSDObject` | 战斗 -> Flux 共享 | `BattleToFluxShared.cs` |
| `GSGBtl` | 静态类 | - | GSG 战斗管理 | `GSGBtl.cs` |
| `GSBtlLogUtil` | 类 | - | 战斗日志工具 | `GSBtlLogUtil.cs` |
| `GSOnlineFriendMgr` | 类 | - | 在线好友管理 | `GSOnlineFriendMgr.cs` |
| `GSOnlinePartyMgr` | 类 | - | 在线队伍管理 | `GSOnlinePartyMgr.cs` |
| `GSRpcUnrealChannel` | 类 | - | Unreal RPC 通道 | `GSRpcUnrealChannel.cs` |
| `GSUIBiList<T>` | 类 | - | UI 双向列表泛型 | `GSUIBiList.cs` |
| `GSUIBiProp<T>` | 类 | - | UI 双向属性泛型 | `GSUIBiProp.cs` |
| `OnlinePartyLogic` | 类 | - | 在线队伍逻辑 | `OnlinePartyLogic.cs` |
| `OssB1Util` | 类 | - | OSS B1 工具 | `OssB1Util.cs` |
| `PartyContext` | 类 | - | 队伍上下文 | `PartyContext.cs` |
| `UIAwardSnapShot` | 类 | - | UI 奖励快照 | `UIAwardSnapShot.cs` |
| `UIDataStoreSnapShotData` | 类 | - | UI 数据存储快照数据 | `UIDataStoreSnapShotData.cs` |
| `UIDataStoreSnapShotMgr` | 静态类 | - | UI 数据存储快照管理器 | `UIDataStoreSnapShotMgr.cs` |

---

### 三、`b1.GSSvc` — 游戏服务绑定 (12 文件)

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `BagEquipBlind` | 类 | - | 背包装备绑定 | `BagEquipBlind.cs` |
| `BagItemBlind` | 类 | - | 背包物品绑定 | `BagItemBlind.cs` |
| `BattleInstance` | 类 | - | 战斗实例 | `BattleInstance.cs` |
| `BattleLogic` | 类 | - | 战斗逻辑 | `BattleLogic.cs` |
| `BattleState` | 类 | - | 战斗状态 | `BattleState.cs` |
| `FluxToBattleSvc` | 类 | `GSDObject` | Flux -> 战斗服务 | `FluxToBattleSvc.cs` |
| `GamePlayerManageSvc` | 类 | - | 游戏玩家管理服务 | `GamePlayerManageSvc.cs` |
| `MultiplyPlayerAwardBoxRecord` | 类 | - | 多人奖励箱记录 | `MultiplyPlayerAwardBoxRecord.cs` |
| `RoleAttrBlind` | 类 | - | 角色属性绑定 | `RoleAttrBlind.cs` |
| `SpellBlind` | 类 | - | 法术绑定 | `SpellBlind.cs` |
| `TalentInfoOneBind` | 类 | - | 天赋信息绑定 | `TalentInfoOneBind.cs` |
| `TaskStageInfoBind` | 类 | - | 任务阶段信息绑定 | `TaskStageInfoBind.cs` |

---

### 四、`b1.Online.CSRpc` — CS RPC 适配器 (2 文件)

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `B1CSMsgAdapter` | 类 | `ICSMsgAdapter` | B1 CS 消息适配器 | `B1CSMsgAdapter.cs` |
| `B1CSMsgHeadAdapter` | 类 | `ICSMsgHeadAdapter` | B1 CS 消息头适配器 | `B1CSMsgHeadAdapter.cs` |

---

### 五、`b1.Util` — 工具类 (8 文件)

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `BattleTask` | 静态类 | 战斗任务 | `BattleTask.cs` |
| `BattleTestJob` | 类 | 战斗测试任务 | `BattleTestJob.cs` |
| `GSLocalDebugCfg` | 类 | GS 本地调试配置 | `GSLocalDebugCfg.cs` |
| `GSLocalDebugCfgUtil` | 类 | GS 本地调试配置工具 | `GSLocalDebugCfgUtil.cs` |
| `HttpUtil` | 静态类 | HTTP 工具 | `HttpUtil.cs` |
| `ILPbEncoding` | 类 | IL Protobuf 编码 | `ILPbEncoding.cs` |
| `JsonBattleTaskJob` | 类 | JSON 战斗任务作业 | `JsonBattleTaskJob.cs` |
| `JsonBattleTaskResult` | 类 | JSON 战斗任务结果 | `JsonBattleTaskResult.cs` |

---

### 六、`b1x` 命名空间 (1 文件)

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `XDumper` | 类 | 调试 XDumper | `XDumper.cs` |

---

### 七、`B1UI` 命名空间 — 核心 UI 入口 (5 文件)

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `B1ScriptMain` | 类 | 脚本主入口 | `B1ScriptMain.cs` |
| `GenRoleDataReason` | 枚举 | 角色数据生成原因 | `GenRoleDataReason.cs` |
| `GSG` | 类 | GSG 核心管理器 | `GSG.cs` |
| `GSOnlineMgr` | 类 | GS 在线管理器 | `GSOnlineMgr.cs` |
| `RoleLoginSvc` | 静态类 | 角色登录服务 | `RoleLoginSvc.cs` |

---

### 八、`B1UI.GSSvc` — 游戏服务对象 (4 文件)

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `B1BattleLogicSvc` | 类 | `GSDObject` | B1 战斗逻辑服务 | `B1BattleLogicSvc.cs` |
| `FTransferParam` | 结构 | - | 战斗传输参数 | `B1BattleLogicSvc.cs` (嵌套) |
| `GSDStoreSvc` | 类 | `GSDObject` | GS 数据存储服务 | `GSDStoreSvc.cs` |
| `GSDStoreGameSvc` | 类 | `GSDObject` | GS 游戏存储服务 | `GSDStoreGameSvc.cs` |
| `GSGMSvc` | 类 | `GSDObject` | GS GM 服务 (内含多个嵌套类型: `GMTestStruct`, `DSGMCmdPackOne`, `DSGMCmdPackData`, `GMCmdPackMgr`, `GMCustomTransMgr`, `DSGMCustomTransOne`, `DSGMCustomTransData`, `DSArchiveSnapshootOne`, `DSArchiveSnapshootData`, `ArchiveSnapshotMgr`) | `GSGMSvc.cs` |

---

### 九、`B1UI.Online` — 网络计时器 (3 文件)

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `GameServerTime` | 类 | `GSTime` | 游戏服务器时间 | `GameServerTime.cs` |
| `B1TimerMgr` | 类 | `GSTickTimerMgr` | B1 计时器管理器 | `B1TimerMgr.cs` |
| `RoomNetSvc` | 类 | - | 房间网络服务 | `RoomNetSvc.cs` |

---

### 十、`B1UI.Script` — 关卡过渡/加载系统 (30 文件)

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `ArchiveTravelLevelBase` | 抽象类 | `TravelLevelTemplateBase` | 存档过渡基类 | `ArchiveTravelLevelBase.cs` |
| `BackToMainMenuTravelLevel` | 类 | `GenericTravelLevel`, 实现 `ISaveArchiveTravelLevel` | 返回主菜单过渡 | `BackToMainMenuTravelLevel.cs` |
| `BackToMainMenuByPlayGoTravelLevel` | 密封类 | `BackToMainMenuTravelLevel` | PlayGo 返回主菜单过渡 | `BackToMainMenuByPlayGoTravelLevel.cs` |
| `BackToMainMenuFullBlackTravelLevel` | 类 | `BackToMainMenuTravelLevel` | 全黑返回主菜单过渡 | `BackToMainMenuFullBlackTravelLevel.cs` |
| `BenchMarkTravelLevel` | 密封类 | `TravelLevelTemplateBase` | 基准测试过渡 | `BenchMarkTravelLevel.cs` |
| `CFSMGReg` | 类 | - | 客户端 FSM 注册 | `CFSMGReg.cs` |
| `CreateArchiveTravelLevelBase` | 抽象类 | `ArchiveTravelLevelBase`, 实现 `ICreateArchiveTravelLevel` | 创建存档过渡基类 | `CreateArchiveTravelLevelBase.cs` |
| `CreateArchiveTravelLevel` | 抽象类 | `CreateArchiveTravelLevelBase` | 创建存档过渡 | `CreateArchiveTravelLevel.cs` |
| `DLCCacheMgr` | 静态类 | - | DLC 缓存管理 | `DLCCacheMgr.cs` |
| `ESaveArchiveDegree` | 枚举 | - | 存档等级 | `ESaveArchiveDegree.cs` |
| `GameIntentTravelLevel` | 密封类 | `ReadArchiveTravelLevel` | 游戏意图过渡 | `GameIntentTravelLevel.cs` |
| `GameLevelPassTravelLevel` | 密封类 | `BackToMainMenuTravelLevel` | 关卡通过过渡 | `GameLevelPassTravelLevel.cs` |
| `GenericTravelLevel` | 类 | `TravelLevelTemplateBase` | 通用过渡 | `GenericTravelLevel.cs` |
| `GMTravelLevel` | 密封类 | `CreateArchiveTravelLevelBase` | GM 过渡 | `GMTravelLevel.cs` |
| `GSAddContentMgr` | 静态类 | - | GS 附加内容管理 | `GSAddContentMgr.cs` |
| `ICreateArchiveTravelLevel` | 接口 | - | 创建存档过渡接口 | `ICreateArchiveTravelLevel.cs` |
| `ILoadArchiveTravelLevel` | 接口 | - | 加载存档过渡接口 | `ILoadArchiveTravelLevel.cs` |
| `ISaveArchiveTravelLevel` | 接口 | - | 保存存档过渡接口 | `ISaveArchiveTravelLevel.cs` |
| `LevelTravelTemplateFactoryScript` | 类 | - | 过渡工厂 | `LevelTravelTemplateFactoryScript.cs` |
| `LoadArchiveTravelLevelBase` | 抽象类 | `ArchiveTravelLevelBase`, 实现 `ILoadArchiveTravelLevel` | 加载存档过渡基类 | `LoadArchiveTravelLevelBase.cs` |
| `NianhuiTravelLevel` | 密封类 | `TravelLevelTemplateBase` | 年会展过渡 | `NianhuiTravelLevel.cs` |
| `OnlineTravelLevel` | 密封类 | `GenericTravelLevel` | 在线过渡 | `OnlineTravelLevel.cs` |
| `OnlyOpenLevelTravelLevel` | 类 | `GenericTravelLevel` | 仅打开关卡过渡 | `OnlyOpenLevelTravelLevel.cs` |
| `ReadArchiveTravelLevel` | 类 | `LoadArchiveTravelLevelBase` | 读取存档过渡 | `ReadArchiveTravelLevel.cs` |
| `SeamlessStartNewGameTravelLevel` | 类 | `CreateArchiveTravelLevel` | 无缝开始新游戏过渡 | `SeamlessStartNewGameTravelLevel.cs` |
| `SetConfigFinishTravelLevel` | 密封类 | `OnlyOpenLevelTravelLevel` | 设置完成过渡 | `SetConfigFinishTravelLevel.cs` |
| `StartNewGamePlusTravelLevel` | 密封类 | `ReadArchiveTravelLevel` | 新游戏+ 过渡 | `StartNewGamePlusTravelLevel.cs` |
| `StartNewGameTravelLevel` | 密封类 | `CreateArchiveTravelLevel` | 开始新游戏过渡 | `StartNewGameTravelLevel.cs` |
| `UnknownTravelLevel` | 密封类 | `CreateArchiveTravelLevel` | 未知过渡 | `UnknownTravelLevel.cs` |
| `WXLoginFinishTravelLevel` | 密封类 | `OnlyOpenLevelTravelLevel` | 微信登录完成过渡 | `WXLoginFinishTravelLevel.cs` |

---

### 十一、`B1UI.Script.GSUI.Util` — UI 音频工具 (1 文件)

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `GSUIAudioUtil` | 静态类 | UI 音频工具类 | `GSUIAudioUtil.cs` |

---

### 十二、`B1UI.GSUI` — UI 视图层 (795 文件, 约 982 个类型声明)

这是最大的模块，主要包含三类类型：

#### 12.1 数据存储 (DS*) — UI 数据层
这些类负责 UI 的数据模型，通常继承自 `GSDStore`、`BindDStore`、`DSDetailBase`、`DSItemEntry`、`DSBtnEntry`、`DSAutoSizeItemBase` 等。每个类通常包含若干嵌套的 Action 类（`GSAction` 子类）用于驱动 UI 变化。

典型的数据存储类（约 160+ 个）：

| 类名前缀 | 示例 | 说明 |
|----------|------|------|
| `DS*` | `DSAlchemy`, `DSArchives`, `DSAward`, `DSBagItem`, `DSBagMain`, `DSBackground`, `DSBenchMark`, `DSChapterMovie`, `DSEquipDetail`, `DSShop`, `DSSpellDetail`, `DSTalent`, `DSTask`, `DSStartGame` 等 | UI 数据存储对象，继承自 `GSDStore` 或 `BindDStore` |

#### 12.2 视图层 (VI*) — UI 组件
这些类负责 UI 视图渲染和交互，通常继承自 `GSUIView` 或 `GSUIGadgetMulti`。数量约 300+ 个，以 `VI` 前缀命名并对应 `B1UI.GSUI` 命名空间。

层次结构：
- `GSUIView`（核心 UI 视图基类）
  - `GSUIPage`（页面基类）→ 如 `BUI_ReportBugPanel`
  - `GSUIGadgetMulti`（多功能小组件基类）
    - `VIEntry<ET, DT>` → `VIItemEntry<ET, DT>` → `VIBtnEntry<T>`, `VIBagItem<T>`, `VIAutoSizeItem<ET, DT>`, `VICostItem` 等
    - `VIButtonBaseV2`、`VIAwardItem`、`VIArchiveTab`、`VIBenchMarkInfo` 等

#### 12.3 枚举 (E*) — 独立文件枚举

| 枚举 | 说明 | 文件 |
|------|------|------|
| `EAccordionType` | 手风琴控件类型 | `EAccordionType.cs` |
| `EActiveMode` | 激活模式 | `EActiveMode.cs` |
| `EAlchemyState` | 炼药状态 | `EAlchemyState.cs` |
| `EAttrState` | 属性状态 | `EAttrState.cs` |
| `EBagArea` | 背包区域 | `EBagArea.cs` |
| `EBagShowItemType` | 背包显示物品类型 | `EBagShowItemType.cs` |
| `EBagTab` | 背包标签 | `EBagTab.cs` |
| `EBuildEquipItemStat` | 装备构建状态 | `EBuildEquipItemStat.cs` |
| `EBuildScene` | 构建场景 | `EBuildScene.cs` |
| `CostItemType` | 消耗品类型 | `CostItemType.cs` |
| `EDetailActionType` | 详情动作类型 | `EDetailActionType.cs` |
| `EDetailType` | 详情类型 | `EDetailType.cs` |
| `EEquipDetailScene` | 装备详情场景 | `EEquipDetailScene.cs` |
| `EEquipPageType` | 装备页类型 | `EEquipPageType.cs` |
| `EEquipSlotType` | 装备槽类型 | `EEquipSlotType.cs` |
| `EFarmFocusType` | 农场焦点类型 | `EFarmFocusType.cs` |
| `EFarmNPCConversationType` | 农场 NPC 对话类型 | `EFarmNPCConversationType.cs` |
| `EGourdDetailType` | 葫芦详情类型 | `EGourdDetailType.cs` |
| `EGourdSlotState` | 葫芦槽状态 | `EGourdSlotState.cs` |
| `EGSBinkMediaPlayState` | Bink 媒体播放状态 | `EGSBinkMediaPlayState.cs` |
| `EIconType` | 图标类型 | `EIconType.cs` |
| `EItemType` | 物品类型 | `EItemType.cs` |
| `EKeyStat` | 按键状态 | `EKeyStat.cs` |
| `ELegacyTalentDescState` | 遗物天赋描述状态 | `ELegacyTalentDescState.cs` |
| `ELegacyTalentState` | 遗物天赋状态 | `ELegacyTalentState.cs` |
| `ELoginBtn` | 登录按钮类型 | `ELoginBtn.cs` |
| `EMaxNumType` | 最大数量类型 | `EMaxNumType.cs` |
| `EnDropSpecailShowType` | 掉落特殊显示类型 | `EnDropSpecailShowType.cs` |
| `EnLoginNoticeState` | 登录通知状态 | `EnLoginNoticeState.cs` |
| `EnPageID` | UI 页面 ID 枚举 | `EnPageID.cs` |
| `EndingCreditsStat` | 结局演职员表状态 | `EEndingCreditsStat.cs` |
| `EBackgroundScene` | 背景场景 | `EBackgroundScene.cs` |

> 此外还有大量枚举定义在 DS* 类中作为嵌套类型 (如 `DSBagMain.EItemUseStat`, `DSDetailBase.EMovieStat`, `DSDisplayUIText.EDisplayTabType`, `DSShop.ECheckBuyStat`, `DSStartGame.StartGameBtnType`, `DSSpellDetail.BottomStat` / `EShowType`, `DSTalent.AnimStat` 等)

#### 12.4 结构体

| 结构体 | 说明 | 文件 |
|--------|------|------|
| `BenchMarkRecord` | 基准测试记录 | `BenchMarkRecord.cs` |
| `BenchMarkHistory` | 基准测试历史 | `BenchMarkHistory.cs` |
| `CostItemParam` | 消耗品参数 | `CostItemParam.cs` |
| `CultureInfo` | 文化信息 | `CultureInfo.cs` |

#### 12.5 其他辅助类

| 类 | 类别 | 说明 | 文件 |
|----|------|------|------|
| `CrossPageEventBase` | 抽象类 | 跨页面事件基类 | `CrossPageEventBase.cs` |
| `CrossPageEventBase_Drop` | 类 | 跨页面掉落事件 | `CrossPageEventBase_Drop.cs` |
| `ConfigFixedItem` | 类 | 配置固定项 | `ConfigFixedItem.cs` |
| `AccordionHelper<FDT, SDT>` | 类 | 手风琴控件辅助器 | `AccordionHelper.cs` |
| `AccordionPool<T>` | 类 | 手风琴池 | `AccordionPool.cs` |
| `ActiveStack<T>` | 类 | 活动堆栈泛型 | `ActiveStack.cs` |
| `AutoSizeItem<ET, DT>` | 类 | 自适应尺寸项 | `AutoSizeItem.cs` |
| `B1GSUIMgr` | 类 | B1 GSUI 管理器 (继承 `GSUIMgr`) | `B1GSUIMgr.cs` |
| `B1UIOverlayMgr` | 类 | B1 UI 覆盖层管理器 | `B1UIOverlayMgr.cs` |
| `DisplayUITextContentHelper` 系列 | 类 | 文本内容显示辅助 (含 _AllText, _Area, _Card, _Dialogue, _Equip, _FaBao, _Guide, _Item, _LoadingTips, _Meditation, _Monster, _Setting, _SoulSkill, _Suit, _Talent 等子类) | `DisplayUITextContentHelper*.cs` |
| `DestructEvent` | 类 | 销毁事件 | `DestructEvent.cs` |
| `FMenuHelper` | 类 | 菜单辅助 | `FMenuHelper.cs` |

---

### 十三、`BtlSvr.Script` — 战斗服务器 (2 文件)

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `CFSMGReg` | 类 | 客户端 FSM 注册 | `CFSMGReg.cs` |
| `<PrivateImplementationDetails>` | 密封类 | 内部实现细节 | `-PrivateImplementationDetails-.cs` |

---

### 十四、`CommB1` — 数据合约与 Protobuf 包装 (869 文件)

> 此目录为自动生成的代码，包含约 126 个 DS* 直接数据类，以及约 745 个对应的泛型包装器文件。

#### 14.1 数据类 (DS*)

这些类定义了游戏数据模型的结构，用于在 C# 中表示 Protobuf 消息的强类型包装。共计约 124 个。

| 类型 | 说明 | 文件 |
|------|------|------|
| `DSAccessoryProp` | 饰品属性 | `DSAccessoryProp.cs` |
| `DSAchievementConfig` | 成就配置 | `DSAchievementConfig.cs` |
| `DSAchievementOne` | 单个成就 | `DSAchievementOne.cs` |
| `DSAchievementStat` | 成就统计 | `DSAchievementStat.cs` |
| `DSAchievementStatus` | 成就状态 | `DSAchievementStatus.cs` |
| `DSActorProgress` | 角色进度 | `DSActorProgress.cs` |
| `DSActorWear` | 角色装备 | `DSActorWear.cs` |
| `DSAlchemyNpcCommunicationStatus` | 炼药 NPC 交流状态 | `DSAlchemyNpcCommunicationStatus.cs` |
| `DSArchiveStaticsOne` | 存档统计 | `DSArchiveStaticsOne.cs` |
| `DSAttrItem` | 属性物品 | `DSAttrItem.cs` |
| `DSAwardItem` | 奖励物品 | `DSAwardItem.cs` |
| `DSAwolMsgPlayerChat` | 玩家聊天消息 | `DSAwolMsgPlayerChat.cs` |
| `DSAwolMsgPlayerCommand` | 玩家命令消息 | `DSAwolMsgPlayerCommand.cs` |
| `DSAwolMsgPlayerMail` | 玩家邮件消息 | `DSAwolMsgPlayerMail.cs` |
| `DSAwolMsgPlayerNotify` | 玩家通知消息 | `DSAwolMsgPlayerNotify.cs` |
| `DSAwolMsgServerMail` | 服务器邮件消息 | `DSAwolMsgServerMail.cs` |
| `DSCardPortraitStage` | 卡牌画像阶段 | `DSCardPortraitStage.cs` |
| `DSCardPortraitStatus` | 卡牌画像状态 | `DSCardPortraitStatus.cs` |
| `DSCardStoryStage` | 卡牌故事阶段 | `DSCardStoryStage.cs` |
| `DSCardStoryStatus` | 卡牌故事状态 | `DSCardStoryStatus.cs` |
| `DSChapterData` | 章节数据 | `DSChapterData.cs` |
| `DSChapterStaticsDataOne` | 章节统计数据 | `DSChapterStaticsDataOne.cs` |
| `DSCrop` | 农作物 | `DSCrop.cs` |
| `DSCropOutput` | 农作物产出 | `DSCropOutput.cs` |
| `DSCSAwolMsgList` | CS 消息列表 | `DSCSAwolMsgList.cs` |
| `DSCSAwolMsgOne` | 单条 CS 消息 | `DSCSAwolMsgOne.cs` |
| `DSDropRecord` | 掉落记录 | `DSDropRecord.cs` |
| `DSEffectAttrFloat` | 浮动效果属性 | `DSEffectAttrFloat.cs` |
| `DSEffectAttrList` | 效果属性列表 | `DSEffectAttrList.cs` |
| `DSEquipMantra` | 装备铭文 | `DSEquipMantra.cs` |
| `DSGlobalData` | 全局数据 | `DSGlobalData.cs` |
| `DSItemStat` | 物品统计 | `DSItemStat.cs` |
| `DSKeyMonsterMeetData` | 关键怪物遭遇数据 | `DSKeyMonsterMeetData.cs` |
| `DSLegacyAbility` | 遗物能力 | `DSLegacyAbility.cs` |
| `DSLegacyTalent` | 遗物天赋 | `DSLegacyTalent.cs` |
| `DSLevelStaticsDataOne` | 关卡统计数据 | `DSLevelStaticsDataOne.cs` |
| `DSMailBase` | 邮件基类 | `DSMailBase.cs` |
| `DSMailExt` | 邮件扩展 | `DSMailExt.cs` |
| `DSMailFilter` | 邮件过滤器 | `DSMailFilter.cs` |
| `DSMailOption` | 邮件选项 | `DSMailOption.cs` |
| `DSMailTempParam` | 邮件模板参数 | `DSMailTempParam.cs` |
| `DSMeditationOne` | 打坐数据 | `DSMeditationOne.cs` |
| `DSMemberInfo` | 成员信息 | `DSMemberInfo.cs` |
| `DSMemberRoleData` | 成员角色数据 | `DSMemberRoleData.cs` |
| `DSMonsterCollection` | 怪物收集 | `DSMonsterCollection.cs` |
| `DSMuseumRedPoint` | 博物馆红点 | `DSMuseumRedPoint.cs` |
| `DSNewGameResetStaticsData` | 新游戏重置统计数据 | `DSNewGameResetStaticsData.cs` |
| `DSOutputConfig` | 输出配置 | `DSOutputConfig.cs` |
| `DSPartyData` | 队伍数据 | `DSPartyData.cs` |
| `DSPartyHelp` | 队伍帮助 | `DSPartyHelp.cs` |
| `DSPartyHelpParam` | 队伍帮助参数 | `DSPartyHelpParam.cs` |
| `DSPartyInfo` | 队伍信息 | `DSPartyInfo.cs` |
| `DSPartyMember` | 队伍成员 | `DSPartyMember.cs` |
| `DSPartySetting` | 队伍设置 | `DSPartySetting.cs` |
| `DSPartyTask` | 队伍任务 | `DSPartyTask.cs` |
| `DSPartyTaskParam` | 队伍任务参数 | `DSPartyTaskParam.cs` |
| `DSPastMemoriesOne` | 回忆片段 | `DSPastMemoriesOne.cs` |
| `DSPlayerCommandParam` | 玩家命令参数 | `DSPlayerCommandParam.cs` |
| `DSPlayerMailContent` | 玩家邮件内容 | `DSPlayerMailContent.cs` |
| `DSPS5Activity` | PS5 活动 | `DSPS5Activity.cs` |
| `DSPS5Task` | PS5 任务 | `DSPS5Task.cs` |
| `DSQuestStageOne` | 任务阶段 | `DSQuestStageOne.cs` |
| `DSRoleAchievement` | 角色成就 | `DSRoleAchievement.cs` |
| `DSRoleActivity` | 角色活动 | `DSRoleActivity.cs` |
| `DSRoleActor` | 角色 Actor | `DSRoleActor.cs` |
| `DSRoleBag` | 角色背包 | `DSRoleBag.cs` |
| `DSRoleBagInfo` | 角色背包信息 | `DSRoleBagInfo.cs` |
| `DSRoleBase` | 角色基础数据 | `DSRoleBase.cs` |
| `DSRoleChapter` | 角色章节 | `DSRoleChapter.cs` |
| `DSRoleCollection` | 角色收藏 | `DSRoleCollection.cs` |
| `DSRoleData` | 角色数据 (根) | `DSRoleData.cs` |
| `DSRoleDataAwolNotify` | 角色数据 Awol 通知 | `DSRoleDataAwolNotify.cs` |
| `DSRoleDataClient` | 角色数据客户端 | `DSRoleDataClient.cs` |
| `DSRoleDataCS` | 角色数据 CS | `DSRoleDataCS.cs` |
| `DSRoleDataMail` | 角色数据邮件 | `DSRoleDataMail.cs` |
| `DSRoleDrop` | 角色掉落 | `DSRoleDrop.cs` |
| `DSRoleEquip` | 角色装备 | `DSRoleEquip.cs` |
| `DSRoleGarden` | 角色花园 | `DSRoleGarden.cs` |
| `DSRoleInteraction` | 角色交互 | `DSRoleInteraction.cs` |
| `DSRoleItem` | 角色物品 | `DSRoleItem.cs` |
| `DSRoleMoney` | 角色金钱 | `DSRoleMoney.cs` |
| `DSRoleMuseum` | 角色博物馆 | `DSRoleMuseum.cs` |
| `DSRoleOnline` | 角色在线数据 | `DSRoleOnline.cs` |
| `DSRoleRedPoint` | 角色红点 | `DSRoleRedPoint.cs` |
| `DSRoleShop` | 角色商店 | `DSRoleShop.cs` |
| `DSRoleSoulSkill` | 角色魂技 | `DSRoleSoulSkill.cs` |
| `DSRoleStaticsData` | 角色统计数据 | `DSRoleStaticsData.cs` |
| `DSRoleTask` | 角色任务 | `DSRoleTask.cs` |
| `DSRoleWine` | 角色酒 | `DSRoleWine.cs` |
| `DSSeedConfig` | 种子配置 | `DSSeedConfig.cs` |
| `DSServerMailContent` | 服务器邮件内容 | `DSServerMailContent.cs` |
| `DSServerMailData` | 服务器邮件数据 | `DSServerMailData.cs` |
| `DSShopBuyRecord` | 商店购买记录 | `DSShopBuyRecord.cs` |
| `DSShopItem` | 商店物品 | `DSShopItem.cs` |
| `DSShopOne` | 单个商店 | `DSShopOne.cs` |
| `DSShortcutItem` | 快捷物品 | `DSShortcutItem.cs` |
| `DSSoulSkillDropRecord` | 魂技掉落记录 | `DSSoulSkillDropRecord.cs` |
| `DSSpellItem` | 法术物品 | `DSSpellItem.cs` |
| `DSTalentOne` | 单个天赋 | `DSTalentOne.cs` |
| `DSWearAccessory` | 已装备饰品 | `DSWearAccessory.cs` |
| `DSWearEquip` | 已装备装备 | `DSWearEquip.cs` |
| `DSWearSoulSkill` | 已装备魂技 | `DSWearSoulSkill.cs` |
| `DSWinePartner` | 酒伙伴 | `DSWinePartner.cs` |

#### 14.2 生成的包装器代码 (Comparer / Constructor / Merger / WrapperList)

对于每个 DS* 数据类（以及一些基本类型和协议枚举），自动生成了以下四个包装类：

- **`XxxComparer`** — 比较器，用于比较两个实例是否相等
- **`XxxConstructor`** — 构造器，用于构建/填充 Protobuf 消息
- **`XxxMerger`** — 合并器，用于合并两个版本的变更
- **`XxxWrapperList`** — 包装列表，管理 Protobuf 重复字段的列表包装

此外还有独立的枚举包装列表，如 `AccessoryGradeWrapperList`、`AchievementGradeWrapperList`、`AchievementVersionWrapperList`、`ActivationStateWrapperList`、`ActivityStatusWrapperList`、`ArchiveSourceWrapperList`、`YesNoTypeWrapperList` 等。

**模式描述**：

```csharp
// 举例: 对 DSAccessoryProp 类型
namespace CommB1
{
    public class AccessoryPropComparer  : IComparer<DSAccessoryProp>, IComparer { ... }
    public class AccessoryPropConstructor : ICSConstructor<DSAccessoryProp> { ... }
    public class AccessoryPropMerger : ICSMerger<DSAccessoryProp> { ... }
    public class AccessoryPropWrapperList : ICSWrapperList<DSAccessoryProp> { ... }
}
```

每种包装器遵循相同的接口惯例，约 124 组 × 4 = 496 个包装类 + 约 249 个枚举包装列表 = 合计约 745 个包装类。

---

### 十五、`CsB1` — RPC 系统 (13 文件)

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `GMAttribute` | 类 | `Attribute` | GM 命令属性标记 | `GMAttribute.cs` |
| `CSRpcAsyncRequest` | 类 | - | CS RPC 异步请求 | `CSRpcAsyncRequest.cs` |
| `GSRPCAuthority` | 枚举 | - | GS RPC 授权类型 | `GSRPCAuthority.cs` |
| `GMNoExportAttribute` | 类 | `Attribute` | GM 不导出属性标记 | `GMNoExportAttribute.cs` |
| `GSRPCAttribute` | 类 | `Attribute` | RPC 方法属性标记 | `GSRPCAttribute.cs` |
| `GSRPCEndpoint` | 枚举 | - | RPC 端点类型 | `GSRPCEndpoint.cs` |
| `RoleDataTagCheck` | 类 | - | 角色数据标签检查 | `RoleDataTagCheck.cs` |
| `PlayerGmExecutor` | 类 | - | 玩家 GM 执行器 | `PlayerGmExecutor.cs` |
| `EnNetLoginReason` | 枚举 | - | 网络登录原因 | `EnNetLoginReason.cs` |
| `PlayerRpcBase` | 类 | - | 玩家 RPC 基类 | `PlayerRpcBase.cs` |
| `CSRpc` | 类 | `PlayerRpcBase` | CS RPC 主类 | `CSRpc.cs` |
| `RoleSvc` | 静态类 | - | 角色服务 | `RoleSvc.cs` |

---

### 十六、`GS.CSNet` — 网络基础设施 (11 文件)

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `CSNetMgr` | 类 | - | CS 网络管理器 | `CSNetMgr.cs` |
| `ICSMsgHeadAdapter` | 接口 | - | 消息头适配器接口 | `ICSMsgHeadAdapter.cs` |
| `CSNetEnvironment` | 静态类 | - | CS 网络环境 | `CSNetEnvironment.cs` |
| `ICSMsgAdapter` | 接口 | - | 消息适配器接口 | `ICSMsgAdapter.cs` |
| `CSRpcClient` | 类 | - | CS RPC 客户端 | `CSRpcClient.cs` |
| `NetPbEncoding` | 类 | - | Protobuf 网络编码 | `NetPbEncoding.cs` |

---

### 十七、`GSE.GSICore` — 核心工具 (10 文件)

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `GSIDelegate` | 类 | 通用委托管理 | `GSIDelegate.cs` |
| `GSEngineUtil` | 类 | 引擎工具函数 | `GSEngineUtil.cs` |
| `GSMathUtil` | 类 | 数学工具函数 | `GSMathUtil.cs` |
| `GSLocalSave` | 类 | 本地存档 | `GSLocalSave.cs` |
| `GSStringUtil` | 类 | 字符串工具 | `GSStringUtil.cs` |
| `GSLocalSaveTag` | 类 | 本地存档标签 | `GSLocalSaveTag.cs` |
| `IGSTickable` | 接口 | Tick 接口 | `IGSTickable.cs` |
| `GSTickableStat` | 枚举 | Tick 状态 | `GSTickableStat.cs` |
| `GSTickMgr` | 类 | Tick 管理器 | `GSTickMgr.cs` |
| `IGSShowIn` | 接口 | UI 显示接口 | `IGSShowIn.cs` |

---

### 十八、`GSE.GSICore.Event` — UI 动画/事件系统 (29 文件)

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `GSIEventBase` | 抽象类 | `IGSTickable` | UI 动画事件基类 | `GSIEventBase.cs` |
| `GSIEventMgr` | 类 | - | 动画事件管理器 | `GSIEventMgr.cs` |
| `GSIEventStat` | 枚举 | - | 事件状态 | `GSIEventStat.cs` |
| `CurveParam` | 类 | - | 曲线参数 | `CurveParam.cs` |
| `GSIEventDelay` | 类 | `GSIEventBase` | 延时事件 | `GSIEventDelay.cs` |
| `GSIEventDelayExec` | 类 | `GSIEventBase` | 延时执行事件 | `GSIEventDelayExec.cs` |
| `GSIEventDelayCountExec` | 类 | `GSIEventBase` | 延时计数执行事件 | `GSIEventDelayCountExec.cs` |
| `GSIEventAsyncQueue` | 类 | `GSIEventBase` | 异步队列事件 | `GSIEventAsyncQueue.cs` |
| `GSIEventSequence` | 类 | `GSIEventBase` | 序列事件 | `GSIEventSequence.cs` |
| `GSIEventExecFunc` | 类 | `GSIEventBase` | 执行函数事件 | `GSIEventExecFunc.cs` |
| `GSIEventEaseFunc` | 类 | `GSIEventBase` | 缓动函数事件 | `GSIEventEaseFunc.cs` |
| `GSIEventTweenFloat` | 类 | `GSIEventBase` | 浮点数补间 | `GSIEventTweenFloat.cs` |
| `GSIEventContentSize` | 类 | `GSIEventBase` | 内容尺寸动画 | `GSIEventContentSize.cs` |
| `GSIEventCanvasPanelSlotMove` | 类 | `GSIEventBase` | Canvas 面板槽移动 | `GSIEventCanvasPanelSlotMove.cs` |
| `GSIEventWidgetFade` | 类 | `GSIEventBase` | 控件淡入淡出 | `GSIEventWidgetFade.cs` |
| `GSIEventWidgetMoveTo` | 类 | `GSIEventBase` | 控件移动到 | `GSIEventWidgetMoveTo.cs` |
| `GSIEventWidgetTranslation` | 类 | `GSIEventBase` | 控件平移 | `GSIEventWidgetTranslation.cs` |
| `GSIEventWidgetCurveTranslation` | 类 | `GSIEventBase` | 控件曲线平移 | `GSIEventWidgetCurveTranslation.cs` |
| `GSIEventWidgetQuickBezierTo` | 类 | `GSIEventBase` | 控件快速贝塞尔到 | `GSIEventWidgetQuickBezierTo.cs` |
| `GSIEventWidgetScale` | 类 | `GSIEventBase` | 控件缩放 | `GSIEventWidgetScale.cs` |
| `GSIEventWidgetSize` | 类 | `GSIEventBase` | 控件尺寸变化 | `GSIEventWidgetSize.cs` |
| `GSIEventWidgetTransTo` | 类 | `GSIEventBase` | 控件变换到 | `GSIEventWidgetTransTo.cs` |
| `GSIEventSetVisibility` | 类 | `GSIEventBase` | 设置可见性 | `GSIEventSetVisibility.cs` |
| `GSIEventRemoveUIPage` | 类 | `GSIEventBase` | 移除 UI 页面 | `GSIEventRemoveUIPage.cs` |
| `GSIEventMatScalarParamter` | 类 | `GSIEventBase` | 材质标量参数修改 | `GSIEventMatScalarParamter.cs` |
| `GSSwitchEventBase` | 类 | - | 切换事件基类 | `GSSwitchEventBase.cs` |
| `GSSwitchEvent` | 类 | `GSSwitchEventBase` | 切换事件 v1 | `GSSwitchEvent.cs` |
| `GSSwitchEventV2` | 类 | `GSSwitchEventBase` | 切换事件 v2 | `GSSwitchEventV2.cs` |
| `GSSwitchEventV3` | 类 | `GSSwitchEventBase` | 切换事件 v3 | `GSSwitchEventV3.cs` |

---

### 十九、`GSE.GSUI` — UI 页面框架 (66 文件)

这是 UI 系统的核心框架层，定义了页面管理、Action 模式、视图层次等基础设施。

#### 19.1 核心基类

| 类型 | 类别 | 继承/实现 | 说明 | 文件 |
|------|------|-----------|------|------|
| `GSBObject` | 抽象类 | - | GSB 对象基类 | `GSBObject.cs` |
| `GSDObject` | 抽象类 | `GSBObject` | GS 数据对象 (带自动销毁/释放生命周期) | `GSDObject.cs` |
| `GSDStore` | 抽象类 | `BindDStore` | GS 数据存储基类 | `GSDStore.cs` |
| `BindDStore` | 抽象类 | `IGSMUIDestruct` | 绑定数据存储基类 (带 UI 生命周期绑定) | `BindDStore.cs` |
| `GSAction` | 抽象类 | - | Action 基类 (驱动 UI 状态变化) | `GSAction.cs` |
| `GSUIView` | 抽象类 | `GSDObject` | UI 视图基类 | `GSUIView.cs` |
| `GSUIPage` | 抽象类 | `GSUIView` | UI 页面基类 | `GSUIPage.cs` |
| `GSUIGadgetMulti` | 抽象类 | `GSUIView` | 多功能 UI 小组件基类 | `GSUIGadgetMulti.cs` |
| `GFlux` | 类 | - | Flux 数据流对象 | `GFlux.cs` |
| `GSTime` | 类 | `GSBObject` | 时间管理 | `GSTime.cs` |
| `GSTickTimerMgr` | 类 | `GSBObject` | Tick 计时器管理器 | `GSTickTimerMgr.cs` |

#### 19.2 UI 管理

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `GSUIMgr` | 类 | GSUI 管理器 (核心 UI 管理器) | `GSUIMgr.cs` |
| `GSUIReg` | 类 | GSUI 注册器 (注册页面配置) | `GSUIReg.cs` |
| `GSUIPageCfg` | 类 | UI 页面配置 | `GSUIPageCfg.cs` |
| `GSUIPageGroupCfg` | 类 | UI 页面组配置 | `GSUIPageGroupCfg.cs` |
| `GSUIPageGroupRuleCfg` | 类 | UI 页面组规则配置 | `GSUIPageGroupRuleCfg.cs` |
| `GSUIPageOP` | 类 | UI 页面操作 | `GSUIPageOP.cs` |
| `GSUIUtil` | 静态类 | GSUI 工具函数 | `GSUIUtil.cs` |
| `GSUI` | 静态类 | GSUI 全局接口 | `GSUI.cs` |

#### 19.3 页面系统

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `UiLayer` | 类 | UI 层 | `UiLayer.cs` |
| `UiLayerStack` | 类 | UI 层堆栈 | `UiLayerStack.cs` |
| `UiPageGraph` | 类 | UI 页面图 | `UiPageGraph.cs` |
| `UiPageGraphOpenType` | 枚举 | 页面图打开类型 | `UiPageGraphOpenType.cs` |
| `UiPageShowParam` | 类 | 页面显示参数 | `UiPageShowParam.cs` |
| `UiStateInterface` | 类 | UI 状态接口 | `UiStateInterface.cs` |
| `GSPageList` | 类 | 页面列表 (内含 `EnPageType`, `EnPageAlignmentType`) | `GSPageList.cs` |
| `GSPageScroll` | 类 | 页面滚动 | `GSPageScroll.cs` |
| `DSGPage` | 类 | GSDStore 页面 (内含 `AShowPage`, `AHidePage`, `AFadeOutPage`, `ATempHidePage`, `ABackLayer`, `ACloseAllLayer`, `APreloadUI` 等 Action 类) | `DSGPage.cs` |
| `DSPageOne` | 类 | 单个页面数据 | `DSPageOne.cs` |

#### 19.4 控件相关

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `GSBUIButtonGroup` | 类 | BUI 按钮组 | `GSBUIButtonGroup.cs` |
| `GSUIButtonCheckGroup` | 类 | 勾选按钮组 | `GSUIButtonCheckGroup.cs` |
| `IWithBUIButton` | 接口 | BUI 按钮接口 | `IWithBUIButton.cs` |
| `IWithButtonCheck` | 接口 | 勾选按钮接口 | `IWithButtonCheck.cs` |
| `GSBiProAutoBinder` | 类 | 双向属性自动绑定器 | `GSBiProAutoBinder.cs` |
| `GSRpcBiProp<reqT, resT>` | 类 | RPC 双向属性泛型 | `GSRpcBiProp.cs` |
| `GSAppDispatcher` | 类 | 应用调度器 | `GSAppDispatcher.cs` |
| `GSMouseCursorMgr` | 类 | 鼠标光标管理器 | `GSMouseCursorMgr.cs` |
| `GSGColor` | 类 | 颜色工具 | `GSGColor.cs` |
| `GSSeqUtil` | 类 | 序列工具 | `GSSeqUtil.cs` |
| `GSDStoreUtil` | 类 | 数据存储工具 | `GSDStoreUtil.cs` |
| `GSUILogUtil` | 类 | UI 日志工具 | `GSUILogUtil.cs` |
| `GSUILogHelper` | 静态类 | UI 日志辅助 | `GSUILogHelper.cs` |
| `GSUILogger` | 类 | UI 日志记录器 | `GSUILogger.cs` |
| `GSUIDevConfig` | 结构 | UI 开发配置 | `GSUIDevConfig.cs` |
| `GSUISimAnHelper` | 类 | UI 模拟动画辅助 | `GSUISimAnHelper.cs` |
| `GSSTest` | 类 | 临时测试 | `GSTmpTest.cs` |

#### 19.5 FSM 状态相关

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `GSSimFSMMgr` | 类 | 模拟 FSM 管理器 | `GSSimFSMMgr.cs` |
| `GSSimFSMState` | 抽象类 | 模拟 FSM 状态基类 | `GSSimFSMState.cs` |
| `GSSimFSMStateAnim` | 类 | 模拟 FSM 动画状态 | `GSSimFSMStateAnim.cs` |
| `GenAGPage` | 类 | 通用 AG 页面 | `GenAGPage.cs` |

#### 19.6 枚举

| 枚举 | 说明 | 文件 |
|------|------|------|
| `EnPageOrder` | 页面排序 | `EnPageOrder.cs` |
| `EnPageInGroupRule` | 页面组内规则 | `EnPageInGroupRule.cs` |
| `EnPageStat` | 页面状态 | `EnPageStat.cs` |
| `EnShowInMove` | 移入显示 | `EnShowInMove.cs` |
| `EnUiPageBgmEffectType` | 页面 BGM 效果类型 | `EnUiPageBgmEffectType.cs` |
| `EnUiPageShowType` | 页面显示类型 | `EnUiPageShowType.cs` |
| `EnUiPageHideType` | 页面隐藏类型 | `EnUiPageHideType.cs` |

#### 19.7 其他辅助类

| 类型 | 说明 | 文件 |
|------|------|------|
| `AsyncEventMgr<T>` | 异步事件管理器泛型 | `AsyncEventMgr.cs` |
| `DelayUpdateEvent` | 延迟更新事件 | `DelayUpdateEvent.cs` |
| `BUIBtnUtil` | BUI 按钮工具 | `BUIBtnUtil.cs` |
| `WoodTestConfigDesc` | 木桩测试配置描述 | `WoodTestConfigDesc.cs` |
| `WoodTestConfigValue` | 木桩测试配置值 | `WoodTestConfigValue.cs` |

---

### 二十、`GSE.Mgr` — 音频管理器 (1 文件)

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `AudioMgr` | 静态类 | 音频管理器 (内含 `AudioState`, `SubBGM` 枚举) | `AudioMgr.cs` |

---

### 二十一、`GSE.Script` — 内部实现 (1 文件)

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `<PrivateImplementationDetails>` | 密封类 | 内部实现细节 | `-PrivateImplementationDetails-.cs` |

---

### 二十二、`GsOnline` — Online Session 包装 (39 文件)

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `PrimitiveWrapperList<T>` | 泛型类 | 原始类型包装列表基类 | `PrimitiveWrapperList.cs` |
| `MessageWrapperList<PBT, DST>` | 泛型类 | Protocol Buffer 消息包装列表基类 | `MessageWrapperList.cs` |
| `BoolWrapperList` | 类 | bool 包装列表 | `BoolWrapperList.cs` |
| `Int32WrapperList` | 类 | int 包装列表 | `Int32WrapperList.cs` |
| `Int64WrapperList` | 类 | long 包装列表 | `Int64WrapperList.cs` |
| `Uint32WrapperList` | 类 | uint 包装列表 | `Uint32WrapperList.cs` |
| `Uint64WrapperList` | 类 | ulong 包装列表 | `Uint64WrapperList.cs` |
| `FloatWrapperList` | 类 | float 包装列表 | `FloatWrapperList.cs` |
| `DoubleWrapperList` | 类 | double 包装列表 | `DoubleWrapperList.cs` |
| `StringWrapperList` | 类 | string 包装列表 | `StringWrapperList.cs` |
| `BytesWrapperList` | 类 | ByteString 包装列表 | `BytesWrapperList.cs` |
| `Fixed64WrapperList` | 类 | fixed64 包装列表 | `Fixed64WrapperList.cs` |
| `BattleHostTypeWrapperList` | 类 | BattleHostType 包装列表 | `BattleHostTypeWrapperList.cs` |
| `DataSyncFlagWrapperList` | 类 | DataSyncFlag 包装列表 | `DataSyncFlagWrapperList.cs` |
| `MemberStateWrapperList` | 类 | MemberState 包装列表 | `MemberStateWrapperList.cs` |
| `MemberUpdateTypeWrapperList` | 类 | MemberUpdateType 包装列表 | `MemberUpdateTypeWrapperList.cs` |
| `SessionAdvertiseTypeWrapperList` | 类 | SessionAdvertiseType 包装列表 | `SessionAdvertiseTypeWrapperList.cs` |
| `SessionEventTypeWrapperList` | 类 | SessionEventType 包装列表 | `SessionEventTypeWrapperList.cs` |
| `SessionStateWrapperList` | 类 | SessionState 包装列表 | `SessionStateWrapperList.cs` |
| `SessionCustomSettingWrapperList` | 类 | SessionCustomSetting 包装列表 | `SessionCustomSettingWrapperList.cs` |
| `SessionDataWrapperList` | 类 | SessionData 包装列表 | `SessionDataWrapperList.cs` |
| `SessionSettingWrapperList` | 类 | SessionSetting 包装列表 | `SessionSettingWrapperList.cs` |
| `SessionMemberWrapperList` | 类 | SessionMember 包装列表 | `SessionMemberWrapperList.cs` |
| `DSSessionData` | 类 | Session 数据 | `DSSessionData.cs` |
| `DSSessionMember` | 类 | Session 成员 | `DSSessionMember.cs` |
| `DSSessionSetting` | 类 | Session 设置 | `DSSessionSetting.cs` |
| `DSSessionCustomSetting` | 类 | Session 自定义设置 | `DSSessionCustomSetting.cs` |
| `SessionDataComparer` | 静态类 | Session 数据比较器 | `SessionDataComparer.cs` |
| `SessionDataConstructor` | 静态类 | Session 数据构造器 | `SessionDataConstructor.cs` |
| `SessionDataMerger` | 类 | Session 数据合并器 | `SessionDataMerger.cs` |
| `SessionMemberComparer` | 静态类 | Session 成员比较器 | `SessionMemberComparer.cs` |
| `SessionMemberConstructor` | 静态类 | Session 成员构造器 | `SessionMemberConstructor.cs` |
| `SessionMemberMerger` | 类 | Session 成员合并器 | `SessionMemberMerger.cs` |
| `SessionSettingComparer` | 静态类 | Session 设置比较器 | `SessionSettingComparer.cs` |
| `SessionSettingConstructor` | 静态类 | Session 设置构造器 | `SessionSettingConstructor.cs` |
| `SessionSettingMerger` | 类 | Session 设置合并器 | `SessionSettingMerger.cs` |
| `SessionCustomSettingComparer` | 静态类 | Session 自定义设置比较器 | `SessionCustomSettingComparer.cs` |
| `SessionCustomSettingConstructor` | 静态类 | Session 自定义设置构造器 | `SessionCustomSettingConstructor.cs` |
| `SessionCustomSettingMerger` | 类 | Session 自定义设置合并器 | `SessionCustomSettingMerger.cs` |

---

### 二十三、`GsOnlineFriend` — Online 好友系统 (19 文件)

> 此目录包含类似 `CommB1` 模式的 Protobuf 包装代码，为好友系统数据的 Comparer / Constructor / Merger / WrapperList 自动生成文件，加上以下核心类型：

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `DSFriendMisc` | 类 | 好友杂项数据 | `DSFriendMisc.cs` 等 (自动生成) |
| 和各种 `Friend*Comparer`, `Friend*Constructor`, `Friend*Merger`, `Friend*WrapperList` | 类 | 好友数据包装器 | 多个文件 |

---

### 二十四、`Online.Friend` — 好友接口 (4 文件)

| 类型 | 类别 | 说明 | 文件 |
|------|------|------|------|
| `IGSOnlineFriend` | 接口 | 在线好友接口 | `IGSOnlineFriend.cs` |
| `GSOnlineFriendImpl` | 类 | 在线好友实现 | `GSOnlineFriendImpl.cs` |
| `OnlineFriendFactory` | 静态类 | 在线好友工厂 | `OnlineFriendFactory.cs` |

---

## 核心继承关系总结

```
GSBObject (抽象基类)
├── GSDObject (数据对象, 带销毁生命周期)
│   ├── GSUIView (UI 视图)
│   │   ├── GSUIPage (UI 页面) ← BUI_ReportBugPanel
│   │   ├── GSUIGadgetMulti (多功能小组件)
│   │   │   ├── VIEntry<ET, DT> → VIItemEntry (物品条目) → VIBtnEntry, VIBagItem, VIAutoSizeItem...
│   │   │   └── VIButtonBaseV2, VIAwardItem, VIBenchMarkInfo ...
│   │   └── VIDetailBase, VIAttrBase, VIMarkerBase ...
│   └── GSDStore (数据存储)
│       ├── DSGPage, DSAward, DSArchives, DSAlchemy ...
│       └── DS* (B1UI.GSUI 中的各种数据存储)
├── GSTime (时间)
└── GSTickTimerMgr (计时器)

BindDStore (抽象, 绑定 UI 生命周期)
└── GSDStore

GSAction (抽象)
└── AShowPage, AHidePage, ASetAwardList, ASetAchieveTips ... (驱动 UI 变化的 Action 类)

GSIEventBase (UI 动画事件)
├── GSIEventDelay, GSIEventDelayExec, GSIEventWidgetFade, GSIEventWidgetMoveTo ...
├── GSIEventSequence (序列)
├── GSIEventAsyncQueue (异步队列)
└── GSSwitchEventBase → GSSwitchEvent, GSSwitchEventV2, GSSwitchEventV3

TravelLevelTemplateBase (关卡过渡)
├── GenericTravelLevel (通用)
├── BenchMarkTravelLevel (基准测试)
└── ArchiveTravelLevelBase (存档)
    ├── LoadArchiveTravelLevelBase → ReadArchiveTravelLevel
    └── CreateArchiveTravelLevelBase → CreateArchiveTravelLevel

FSMState_GI_LoadingBase (加载状态) → 约 50 个子类
FSMConditionBase (加载条件) → 约 10 个子类

AsyncPipelineBase (异步管线) → CreateParty, SearchParty, BattleReady...
AsyncTaskBase (异步任务) → CreateSession, JoinSession, RPC...

GSDObject → BattleToFluxServer, BattleToFluxShared, FluxToBattleSvc
```

---

## 数据统计

| 目录 | 文件数 | 估计类/类型数 |
|------|--------|-------------|
| CommB1 | 869 | ~870 |
| B1UI.GSUI | 795 | ~982 (含嵌套) |
| b1 | 205 | ~150 |
| GSE/GSUI | 66 | ~60 |
| GsOnline | 39 | ~39 |
| GSE/GSICore/Event | 29 | ~29 |
| B1UI/Script | 30 | ~30 |
| GsOnlineFriend | 19 | ~19 |
| CsB1 | 13 | ~13 |
| b1/GSSvc | 12 | ~12 |
| GS/CSNet | 11 | ~11 |
| GSE/GSICore | 10 | ~10 |
| b1/Util | 8 | ~8 |
| B1UI | 5 | ~5 |
| B1UI/GSSvc | 4 | ~4 (+嵌套) |
| Online/Friend | 4 | ~4 |
| B1UI/Online | 3 | ~3 |
| b1/Online/CSRpc | 2 | ~2 |
| BtlSvr/Script | 2 | ~2 |
| Others | 6 | ~6 |
| **总计** | **2134** | **~2250+** |
