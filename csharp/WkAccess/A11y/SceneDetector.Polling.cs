using System;
using System.Linq;
using System.Threading;
using b1;
using B1UI;
using B1UI.GSUI;
using GSE.GSUI;
using UnrealEngine.Runtime;

namespace WkAccess.A11y.Detection;

public static partial class SceneDetector
{
    static Timer? _fallbackTimer;
    static bool _timerStarted;

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

    static DateTime _lastUiCheck = DateTime.MinValue;
    static DateTime _lastFsmCheck = DateTime.MinValue;
    static DateTime _lastStatusLog = DateTime.MinValue;

    // ── Tick 轮询（由后备定时器驱动） ──

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
            A11yLog.Debug($"[SceneDetector] 📊 状态摘要 | 场景={_currentScene} | Map={_currentMapName} | LevelId={_currentLevelId} | Pages可见={_visiblePages.Count} | 事件={(_eventsSubscribed?"✅":"❌")} | GSG={(_gsgAvailable?"✅":"❌")}");
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
}
