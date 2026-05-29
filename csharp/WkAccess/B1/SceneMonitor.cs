using System;
using System.Linq;
using b1;
using B1UI;
using B1UI.GSUI;
using GSE.GSUI;
using UnrealEngine.Runtime;

namespace WkAccess.B1;

/// <summary>
/// 场景检测驱动：订阅游戏事件、轮询 FSM 状态、驱动 GameState 更新。
/// </summary>
public static partial class SceneMonitor
{
    static BGW_EventCollection? _events;
    static BGW_GameLifeTimeMgr? _lifeTime;
    static bool _eventsSubscribed;
    static bool _gsgAvailable;

    // ── 公开 API ──

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
                A11y.A11yLog.Debug("[SceneMonitor] ✅ BGW_EventCollection 事件订阅完成");
            }
        }
        catch (Exception ex)
        {
            A11y.A11yLog.Warning($"[SceneMonitor] Init 暂不可用: {ex.Message}");
        }
    }

    /// <summary>尝试初始化 GSG（B1UI_GSE.Script 加载后调用）。</summary>
    public static void TryInitGSG()
    {
        if (_gsgAvailable) return;
        try
        {
            var ctx = GSG.Context;
            _gsgAvailable = true;
            A11y.A11yLog.Debug("[SceneMonitor] ✅ GSG (B1UI_GSE.Script) 可用");
        }
        catch
        {
            // GSG 尚未加载
        }
    }

    /// <summary>启动后备轮询定时器（Mod 初始化时调用）。</summary>
    public static void Start()
    {
        StartFallbackTimer();
    }

    /// <summary>清理资源（Mod 卸载时调用）。</summary>
    public static void Stop()
    {
        _fallbackTimer?.Dispose();
        _fallbackTimer = null;
        _timerStarted = false;
        _eventsSubscribed = false;
        UnsubscribeEvents();
        _events = null;
        _lifeTime = null;
        GameState.Clear();
    }

    /// <summary>打印当前场景和 UI 页面摘要（调试命令）。</summary>
    public static void PrintSummary()
    {
        A11y.A11yLog.Warning($"=== 当前场景: {GameState.CurrentScene} ===");
        A11y.A11yLog.Info($"  Map: {GameState.CurrentMapName}, LevelId: {GameState.CurrentLevelId}");
        A11y.A11yLog.Info($"  Events: {(_eventsSubscribed ? "✅" : "❌")}, GSG: {(_gsgAvailable ? "✅" : "❌")}");
        A11y.A11yLog.Info($"  Visible Pages ({GameState.VisiblePages.Count}):");

        foreach (var pageId in GameState.VisiblePages.OrderBy(id => id))
        {
            A11y.A11yLog.Info($"    - ({pageId}) {((EnPageID)pageId)}");
        }

        try
        {
            if (_lifeTime != null)
            {
                var curState = _lifeTime.GlobalFSMInstanceCurState;
                A11y.A11yLog.Info($"  FSM State: {curState}");
                A11y.A11yLog.Info($"  InTravelLevel: {_lifeTime.IsInTravelLevel()}");
                A11y.A11yLog.Info($"  IsMainMenu: {_lifeTime.IsInFSMState(SGI_Global.MainMenu)}");
                A11y.A11yLog.Info($"  IsInGame: {_lifeTime.IsInFSMState(SGI_Global.InBattleStandAlone)}");
            }
        }
        catch (Exception ex)
        {
            A11y.A11yLog.Warning($"  FSM query failed: {ex.Message}");
        }
    }

    // ── 内部 ──

    static void UpdateScene(GameState.GameScene newScene)
    {
        GameState.SetScene(newScene);

        if (newScene is GameState.GameScene.MainMenu or GameState.GameScene.InGame)
        {
            TryInitGSG();
        }
    }

    // ── 事件订阅 ──

    static void SubscribeEvents()
    {
        if (_events == null) return;
        UnsubscribeEvents();

        _events.Evt_PreLoadMap += OnEvtPreLoadMap;
        _events.Evt_PostLoadMapWithWorld += OnEvtPostLoadMapWithWorld;
        _events.Evt_OnSeamlessTravelStart += OnEvtSeamlessTravelStart;
        _events.Evt_PostSeamlessTravel += OnEvtPostSeamlessTravel;
        _events.Evt_OpenLevelFinished += OnEvtOpenLevelFinished;
        _events.Evt_leavingMap += OnEvtLeavingMap;
        _events.Evt_OnCurrentLevelChanged += OnEvtCurrentLevelChanged;

        _events.Evt_LoadingBeginFadeAway += OnEvtLoadingBeginFadeAway;
        _events.Evt_OnLoadingStepFinish += OnEvtLoadingStepFinish;

        _events.Evt_OnTeleportFinished += OnEvtTeleportFinished;

        _events.Evt_OnPlayerPostLogin += OnEvtPlayerPostLogin;
        _events.Evt_PlayerControllerBeginPlay += OnEvtPlayerControllerBeginPlay;
        _events.Evt_PlayerControllerEndPlay += OnEvtPlayerControllerEndPlay;
        _events.Evt_PlayerDelayBeginPlayFinished += OnEvtPlayerDelayBeginPlayFinished;

        _events.Evt_ClearAllGameData += OnEvtClearAllGameData;
        _events.Evt_NextChapterTravelBegin += OnEvtNextChapterTravelBegin;
        _events.Evt_RefreshLevelInfo += OnEvtRefreshLevelInfo;

        _events.Evt_UIActived += OnEvtUIActived;

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
        GameState.SetMapName(mapName);
        A11y.A11yLog.Debug($"[SceneMonitor] 📍 PreLoadMap: {mapName}");
        UpdateScene(GameState.GameScene.Loading);
    }

    static void OnEvtPostLoadMapWithWorld()
        => A11y.A11yLog.Debug("[SceneMonitor] ✅ PostLoadMapWithWorld — 世界加载完成");

    static void OnEvtSeamlessTravelStart(string travelUrl)
        => A11y.A11yLog.Debug($"[SceneMonitor] 🔄 SeamlessTravelStart: {travelUrl}");

    static void OnEvtPostSeamlessTravel()
        => A11y.A11yLog.Debug("[SceneMonitor] ✅ PostSeamlessTravel — 无缝过渡完成");

    static void OnEvtOpenLevelFinished()
        => A11y.A11yLog.Debug("[SceneMonitor] ✅ OpenLevelFinished");

    static void OnEvtLeavingMap()
        => A11y.A11yLog.Debug("[SceneMonitor] 👋 LeavingMap");

    static void OnEvtCurrentLevelChanged(int levelId)
    {
        GameState.SetCurrentLevel(levelId);
        A11y.A11yLog.Debug($"[SceneMonitor] 🏷️ CurrentLevelChanged -> LevelID={levelId}");
    }

    static void OnEvtLoadingBeginFadeAway()
        => A11y.A11yLog.Debug("[SceneMonitor] 🔽 LoadingBeginFadeAway — 加载画面淡出");

    static void OnEvtLoadingStepFinish()
        => A11y.A11yLog.Debug("[SceneMonitor] 👣 LoadingStepFinish");

    static void OnEvtTeleportFinished()
        => A11y.A11yLog.Debug("[SceneMonitor] ⚡ TeleportFinished");

    static void OnEvtPlayerPostLogin()
        => A11y.A11yLog.Debug("[SceneMonitor] 🧑 PlayerPostLogin");

    static void OnEvtPlayerControllerBeginPlay(BGP_PlayerControllerCS controller)
        => A11y.A11yLog.Debug("[SceneMonitor] 🎮 PlayerControllerBeginPlay");

    static void OnEvtPlayerControllerEndPlay()
        => A11y.A11yLog.Debug("[SceneMonitor] 🎮 PlayerControllerEndPlay");

    static void OnEvtPlayerDelayBeginPlayFinished()
        => A11y.A11yLog.Debug("[SceneMonitor] ⏳ PlayerDelayBeginPlayFinished");

    static void OnEvtClearAllGameData()
        => A11y.A11yLog.Debug("[SceneMonitor] 🗑️ ClearAllGameData");

    static void OnEvtNextChapterTravelBegin(int chapterId)
        => A11y.A11yLog.Debug($"[SceneMonitor] 📖 NextChapterTravelBegin: Chapter={chapterId}");

    static void OnEvtRefreshLevelInfo()
        => A11y.A11yLog.Debug("[SceneMonitor] 🔄 RefreshLevelInfo");

    static void OnEvtUIActived(int pageId, bool active)
    {
        if (active)
            A11y.A11yLog.Debug($"[SceneMonitor] 📄 UI打开: {(EnPageID)pageId} (ID={pageId})");
        GameState.SetPageActive(pageId, active);
    }

    static void OnEvtSetGamePause(EPauseEvent pauseEvent, bool isPaused)
        => A11y.A11yLog.Debug($"[SceneMonitor] ⏸️ SetGamePause: {pauseEvent} -> {(isPaused ? "暂停" : "恢复")}");
}
