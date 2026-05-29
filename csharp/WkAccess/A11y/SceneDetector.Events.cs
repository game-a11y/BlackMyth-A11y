using System;
using b1;
using B1UI;
using B1UI.GSUI;
using GSE.GSUI;
using UnrealEngine.Runtime;

namespace WkAccess.A11y.Detection;

public static partial class SceneDetector
{
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
            // A11yLog.Debug($"[SceneDetector] 📄 UI关闭: {name} (ID={pageId})");
        }
        OnUIPageChanged?.Invoke(pageId, active);
    }

    static void OnEvtSetGamePause(EPauseEvent pauseEvent, bool isPaused)
    {
        A11yLog.Info($"[SceneDetector] ⏸️ SetGamePause: {pauseEvent} -> {(isPaused ? "暂停" : "恢复")}");
    }
}
