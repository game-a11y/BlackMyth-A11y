using System.Linq;
using System.Threading;
using b1;
using B1UI;
using B1UI.GSUI;
using GSE.GSUI;
using UnrealEngine.Runtime;

namespace WkAccess.A11y;

/// <summary>
/// 场景检测器：挂钩场景加载/UI 页面切换，用于无障碍功能可行性测试。
/// 输出所有场景变化和页面切换到控制台日志。
/// </summary>
public static class SceneDetector
{
    // ── 场景状态 ──
    public enum GameScene
    {
        Unknown,
        Startup,          // 游戏启动
        LogIn,            // 登录界面
        MainMenu,         // 主菜单
        Loading,          // 加载中
        InGame,           // 游戏中
        PauseMenu,        // 暂停菜单/功能页面
        ShaderCompiling,  // 着色器编译
    }

    static GameScene _currentScene = GameScene.Unknown;
    static string _currentMapName = "";
    static int _currentLevelId = -1;

    static BGW_EventCollection? _events;
    static BGW_GameLifeTimeMgr? _lifeTime;
    static bool _eventsSubscribed;
    static bool _gsgAvailable;

    // 当前可见 UI 页面集合
    static readonly HashSet<int> _visiblePages = new();
    static DateTime _lastUiCheck = DateTime.MinValue;
    static DateTime _lastFsmCheck = DateTime.MinValue;
    static DateTime _lastStatusLog = DateTime.MinValue;

    // ── 后备定时器（在 Harmony Tick 补丁未触发时提供轮询能力） ──
    static Timer? _fallbackTimer;
    static bool _timerStarted;

    // ── 初始化（由补丁在适当时机调用） ──

    /// <summary>尝试从 GameInstance 上下文初始化事件订阅。</summary>
    public static void TryInit(UObject context)
    {
        if (_eventsSubscribed) return;
        try
        {
            _events = BGW_EventCollection.Get(context);
            _lifeTime = BGW_GameLifeTimeMgr.Get(context);

            if (_events != null)
            {
                SubscribeEvents();
                _eventsSubscribed = true;
                A11yLog.Info("[SceneDetector] ✅ BGW_EventCollection 事件订阅完成");
            }
        }
        catch (Exception ex)
        {
            A11yLog.Warning($"[SceneDetector] Init 暂不可用: {ex.Message}");
        }
    }

    /// <summary>尝试初始化 GSG（B1UI_GSE.Script 加载后调用）。</summary>
    public static void TryInitGSG()
    {
        if (_gsgAvailable) return;
        try
        {
            // 检查 GSG 是否可用（B1UI_GSE.Script 是否已加载）
            var ctx = GSG.Context;
            _gsgAvailable = true;
            A11yLog.Info("[SceneDetector] ✅ GSG (B1UI_GSE.Script) 可用");
        }
        catch
        {
            // GSG 尚未加载
        }
    }

    /// <summary>启动后备定时器，每 2 秒重试事件订阅（当 Harmony 补丁未触发时的兜底方案）。</summary>
    public static void StartFallbackTimer()
    {
        if (_timerStarted) return;
        _timerStarted = true;
        // 延迟 5 秒后开始轮询（让游戏有足够时间初始化）
        _fallbackTimer = new Timer(_ =>
        {
            try
            {
                FThreading.RunOnGameThread(() =>
                {
                    // 重试事件订阅
                    if (!_eventsSubscribed)
                    {
                        var world = WkUtils.GetWorld();
                        if (world != null) TryInit(world);
                    }
                    // 驱动 Tick（限频由 OnTick 内部控制）
                    OnTick(1.0f);
                });
            }
            catch (Exception ex)
            {
                A11yLog.Warning($"[SceneDetector] 定时器回调异常: {ex.Message}");
            }
        }, null, 5000, 2000);
        A11yLog.Info("[SceneDetector] ⏱ 后备定时器已启动 (间隔 2s)");
    }

    // ── 事件订阅 ──
    // 所有事件处理器定义为静态方法，配合 -= 先减后加，确保 Mod 重载不重复订阅

    static void SubscribeEvents()
    {
        if (_events == null) return;

        // 先解除再绑定，防止 Mod 重载导致重复订阅
        UnsubscribeEvents();

        // ---- 关卡/场景生命周期 ----
        _events.Evt_PreLoadMap += OnEvtPreLoadMap;
        _events.Evt_PostLoadMapWithWorld += OnEvtPostLoadMapWithWorld;
        _events.Evt_OnSeamlessTravelStart += OnEvtSeamlessTravelStart;
        _events.Evt_PostSeamlessTravel += OnEvtPostSeamlessTravel;
        _events.Evt_OpenLevelFinished += OnEvtOpenLevelFinished;
        _events.Evt_leavingMap += OnEvtLeavingMap;
        _events.Evt_OnCurrentLevelChanged += OnEvtCurrentLevelChanged;

        // ---- 加载画面 ----
        _events.Evt_LoadingBeginFadeAway += OnEvtLoadingBeginFadeAway;
        _events.Evt_OnLoadingStepFinish += OnEvtLoadingStepFinish;

        // ---- 传送 ----
        _events.Evt_OnTeleportFinished += OnEvtTeleportFinished;

        // ---- 玩家 ----
        _events.Evt_OnPlayerPostLogin += OnEvtPlayerPostLogin;
        _events.Evt_PlayerControllerBeginPlay += OnEvtPlayerControllerBeginPlay;
        _events.Evt_PlayerControllerEndPlay += OnEvtPlayerControllerEndPlay;
        _events.Evt_PlayerDelayBeginPlayFinished += OnEvtPlayerDelayBeginPlayFinished;

        // ---- 重置/清理 ----
        _events.Evt_ClearAllGameData += OnEvtClearAllGameData;
        _events.Evt_NextChapterTravelBegin += OnEvtNextChapterTravelBegin;
        _events.Evt_RefreshLevelInfo += OnEvtRefreshLevelInfo;

        // ---- UI 页面活跃事件 ----
        _events.Evt_UIActived += OnEvtUIActived;

        // ---- 游戏暂停/恢复 ----
        _events.Evt_SetGamePause += OnEvtSetGamePause;
    }

    static void UnsubscribeEvents()
    {
        if (_events == null) return;

        _events.Evt_PreLoadMap -= OnEvtPreLoadMap;
        _events.Evt_PostLoadMapWithWorld -= OnEvtPostLoadMapWithWorld;
        _events.Evt_OnSeamlessTravelStart -= OnEvtSeamlessTravelStart;
        _events.Evt_PostSeamlessTravel -= OnEvtPostSeamlessTravel;
        _events.Evt_OpenLevelFinished -= OnEvtOpenLevelFinished;
        _events.Evt_leavingMap -= OnEvtLeavingMap;
        _events.Evt_OnCurrentLevelChanged -= OnEvtCurrentLevelChanged;
        _events.Evt_LoadingBeginFadeAway -= OnEvtLoadingBeginFadeAway;
        _events.Evt_OnLoadingStepFinish -= OnEvtLoadingStepFinish;
        _events.Evt_OnTeleportFinished -= OnEvtTeleportFinished;
        _events.Evt_OnPlayerPostLogin -= OnEvtPlayerPostLogin;
        _events.Evt_PlayerControllerBeginPlay -= OnEvtPlayerControllerBeginPlay;
        _events.Evt_PlayerControllerEndPlay -= OnEvtPlayerControllerEndPlay;
        _events.Evt_PlayerDelayBeginPlayFinished -= OnEvtPlayerDelayBeginPlayFinished;
        _events.Evt_ClearAllGameData -= OnEvtClearAllGameData;
        _events.Evt_NextChapterTravelBegin -= OnEvtNextChapterTravelBegin;
        _events.Evt_RefreshLevelInfo -= OnEvtRefreshLevelInfo;
        _events.Evt_UIActived -= OnEvtUIActived;
        _events.Evt_SetGamePause -= OnEvtSetGamePause;
    }

    // ── 事件处理器 ──

    static void OnEvtPreLoadMap(string mapName)
    {
        _currentMapName = mapName;
        A11yLog.Info($"[SceneDetector] 📍 PreLoadMap: {mapName}");
        UpdateScene(GameScene.Loading);
    }

    static void OnEvtPostLoadMapWithWorld()
    {
        A11yLog.Info("[SceneDetector] ✅ PostLoadMapWithWorld — 世界加载完成");
    }

    static void OnEvtSeamlessTravelStart(string travelUrl)
    {
        A11yLog.Info($"[SceneDetector] 🔄 SeamlessTravelStart: {travelUrl}");
    }

    static void OnEvtPostSeamlessTravel()
    {
        A11yLog.Info("[SceneDetector] ✅ PostSeamlessTravel — 无缝过渡完成");
    }

    static void OnEvtOpenLevelFinished()
    {
        A11yLog.Info("[SceneDetector] ✅ OpenLevelFinished");
    }

    static void OnEvtLeavingMap()
    {
        A11yLog.Info("[SceneDetector] 👋 LeavingMap");
    }

    static void OnEvtCurrentLevelChanged(int levelId)
    {
        _currentLevelId = levelId;
        A11yLog.Info($"[SceneDetector] 🏷️ CurrentLevelChanged -> LevelID={levelId}");
    }

    static void OnEvtLoadingBeginFadeAway()
    {
        A11yLog.Info("[SceneDetector] 🔽 LoadingBeginFadeAway — 加载画面淡出");
    }

    static void OnEvtLoadingStepFinish()
    {
        A11yLog.Info("[SceneDetector] 👣 LoadingStepFinish");
    }

    static void OnEvtTeleportFinished()
    {
        A11yLog.Info("[SceneDetector] ⚡ TeleportFinished");
    }

    static void OnEvtPlayerPostLogin()
    {
        A11yLog.Info("[SceneDetector] 🧑 PlayerPostLogin");
    }

    static void OnEvtPlayerControllerBeginPlay(BGP_PlayerControllerCS controller)
    {
        A11yLog.Info("[SceneDetector] 🎮 PlayerControllerBeginPlay");
    }

    static void OnEvtPlayerControllerEndPlay()
    {
        A11yLog.Info("[SceneDetector] 🎮 PlayerControllerEndPlay");
    }

    static void OnEvtPlayerDelayBeginPlayFinished()
    {
        A11yLog.Info("[SceneDetector] ⏳ PlayerDelayBeginPlayFinished");
    }

    static void OnEvtClearAllGameData()
    {
        A11yLog.Info("[SceneDetector] 🗑️ ClearAllGameData");
    }

    static void OnEvtNextChapterTravelBegin(int chapterId)
    {
        A11yLog.Info($"[SceneDetector] 📖 NextChapterTravelBegin: Chapter={chapterId}");
    }

    static void OnEvtRefreshLevelInfo()
    {
        A11yLog.Info("[SceneDetector] 🔄 RefreshLevelInfo");
    }

    static void OnEvtUIActived(int pageId, bool active)
    {
        var name = ((EnPageID)pageId).ToString();
        if (active)
        {
            _visiblePages.Add(pageId);
            A11yLog.Info($"[SceneDetector] 📄 UI打开: {name} (ID={pageId})");
        }
        else
        {
            _visiblePages.Remove(pageId);
            A11yLog.Info($"[SceneDetector] 📄 UI关闭: {name} (ID={pageId})");
        }
    }

    static void OnEvtSetGamePause(EPauseEvent pauseEvent, bool isPaused)
    {
        A11yLog.Info($"[SceneDetector] ⏸️ SetGamePause: {pauseEvent} -> {(isPaused ? "暂停" : "恢复")}");
    }

    // ── Tick 轮询（由 GameMode.ReceiveTick 补丁驱动） ──

    public static void OnTick(float deltaTime)
    {
        // 事件未订阅时，尝试用 WkUtils 获取 World 上下文重试
        if (!_eventsSubscribed)
        {
            try
            {
                var world = WkUtils.GetWorld();
                if (world != null) TryInit(world);
            }
            catch (Exception ex)
            {
                A11yLog.Warning($"[SceneDetector] OnTick 获取 World 失败: {ex.Message}");
            }
        }

        if (!_eventsSubscribed && _lifeTime == null) return;

        var now = DateTime.UtcNow;

        // FSM 状态轮询 — 每 1 秒
        if ((now - _lastFsmCheck).TotalSeconds >= 1.0)
        {
            _lastFsmCheck = now;
            PollFsmState();
        }

        // UI 页面轮询 — 每 3 秒（主靠 Evt_UIActived 事件，轮询做补充）
        if ((now - _lastUiCheck).TotalSeconds >= 3.0)
        {
            _lastUiCheck = now;
            PollUiPages();
        }

        // 定期状态日志 — 每 30 秒输出一次摘要
        if ((now - _lastStatusLog).TotalSeconds >= 30.0)
        {
            _lastStatusLog = now;
            A11yLog.Info($"[SceneDetector] 📊 状态摘要 | 场景={_currentScene} | Map={_currentMapName} | LevelId={_currentLevelId} | Pages可见={_visiblePages.Count} | 事件={(_eventsSubscribed?"✅":"❌")} | GSG={(_gsgAvailable?"✅":"❌")}");
        }
    }

    static void PollFsmState()
    {
        try
        {
            if (_lifeTime == null) return;

            if (_lifeTime.IsInFSMState(SGI_Global.MainMenu))
            {
                UpdateScene(GameScene.MainMenu);
            }
            else if (_lifeTime.IsInFSMState(SGI_Global.InBattleStandAlone))
            {
                UpdateScene(GameScene.InGame);
            }
            else if (_lifeTime.IsInFSMState(SGI_Global.WaitGameStart))
            {
                UpdateScene(GameScene.Startup);
            }
            else if (_lifeTime.IsInFSMState(SGI_Global.WXLogin))
            {
                UpdateScene(GameScene.LogIn);
            }
            else if (_lifeTime.IsInTravelLevel())
            {
                UpdateScene(GameScene.Loading);
            }
        }
        catch (Exception ex)
        {
            A11yLog.Warning($"[SceneDetector] PollFsmState 异常: {ex.Message}");
        }
    }

    /// <summary>轮询当前 UI 状态（Evt_UIActived 为主，轮询为辅）。</summary>
    static void PollUiPages()
    {
        if (!_gsgAvailable) return;

        GSUIPageOP? pageOp;
        try
        {
            pageOp = GSG.GSPageOP;
            if (pageOp == null) return;
        }
        catch
        {
            return; // GSG 已加载但 GSPageOP 尚未初始化
        }

        // 仅检查 Evt_UIActived 已记录到的页面（不枚举全部 EnPageID，避免越界风险）
        foreach (var pageId in _visiblePages.ToList())
        {
            try
            {
                if (!pageOp.PageGraphRawIsUiPageShowIng(pageId))
                {
                    _visiblePages.Remove(pageId);
                }
            }
            catch
            {
                // 单个页面查询异常不影响其他页面
            }
        }
    }

    // ── 场景状态管理 ──

    static void UpdateScene(GameScene newScene)
    {
        if (_currentScene == newScene) return;
        var old = _currentScene;
        _currentScene = newScene;
        A11yLog.Warning($"[SceneDetector] 🔔 === 场景切换: {old} -> {newScene} ===");
        A11yLog.Info($"[SceneDetector]     Map={_currentMapName}, LevelId={_currentLevelId}");

        // 检查是否需要标记 GSG 可用（主菜单或游戏中都说明 UI 系统已就绪）
        if (newScene is GameScene.MainMenu or GameScene.InGame)
        {
            TryInitGSG();
        }
    }

    // ── Harmony 补丁辅助方法 ──

    /// <summary>由 GameMode/GameState Tick 补丁调用。</summary>
    public static void OnTickUpdate()
    {
        StartFallbackTimer(); // 确保后备定时器已启动（幂等）
        OnTick(0.33f);
    }

    /// <summary>由 BGW_GameInstance_B1.InitGameInstObj 后置补丁调用。</summary>
    public static void OnGameInstanceInit_Postfix(UObject context)
    {
        A11yLog.Info("[SceneDetector] GameInstance 初始化，尝试订阅事件...");
        TryInit(context);
        StartFallbackTimer();
    }

    /// <summary>由 BGW_ManagedReflectMgr.LoadScriptAssemblyFile 后置补丁调用。</summary>
    public static void OnScriptAssemblyLoaded_Postfix()
    {
        A11yLog.Info("[SceneDetector] 脚本程序集加载完成，尝试初始化 GSG...");
        TryInitGSG();
    }

    /// <summary>由 HandleLeavingMap 前置补丁调用。</summary>
    public static void OnHandleLeavingMap_Prefix()
    {
        A11yLog.Info("[SceneDetector] 👋 HandleLeavingMap — 即将离开当前地图");
    }

    /// <summary>清理资源（Mod 卸载时调用）。</summary>
    public static void Deinit()
    {
        _fallbackTimer?.Dispose();
        _fallbackTimer = null;
        _timerStarted = false;
        _eventsSubscribed = false;
        // 解绑所有事件处理器
        UnsubscribeEvents();
        _events = null;
        _lifeTime = null;
    }

    /// <summary>打印当前所有可见 UI 页面的摘要。</summary>
    public static void PrintCurrentUI()
    {
        A11yLog.Warning($"=== 当前场景: {_currentScene} ===");
        A11yLog.Info($"  Map: {_currentMapName}, LevelId: {_currentLevelId}");
        A11yLog.Info($"  Events: {(_eventsSubscribed ? "✅" : "❌")}, GSG: {(_gsgAvailable ? "✅" : "❌")}");
        A11yLog.Info($"  Visible Pages ({_visiblePages.Count}):");

        foreach (var pageId in _visiblePages.OrderBy(id => id))
        {
            A11yLog.Info($"    - ({pageId}) {((EnPageID)pageId)}");
        }

        // 打印 FSM 状态
        try
        {
            if (_lifeTime != null)
            {
                var curState = _lifeTime.GlobalFSMInstanceCurState;
                A11yLog.Info($"  FSM State: {curState}");
                A11yLog.Info($"  InTravelLevel: {_lifeTime.IsInTravelLevel()}");
                A11yLog.Info($"  IsMainMenu: {_lifeTime.IsInFSMState(SGI_Global.MainMenu)}");
                A11yLog.Info($"  IsInGame: {_lifeTime.IsInFSMState(SGI_Global.InBattleStandAlone)}");
            }
        }
        catch (Exception ex)
        {
            A11yLog.Warning($"  FSM query failed: {ex.Message}");
        }
    }
}
