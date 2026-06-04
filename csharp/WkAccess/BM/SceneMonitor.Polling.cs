using WkAccess.A11y.UE;
using System.Threading;

namespace WkAccess.BM;

public static partial class SceneMonitor
{
    static Timer? _fallbackTimer;
    static bool _timerStarted;

    public static void StartFallbackTimer()
    {
        if (_timerStarted) return;
        _timerStarted = true;
        _fallbackTimer = new Timer(_ =>
        {
            try
            {
                FThreading.RunOnGameThread(() =>
                {
                    if (!_eventsSubscribed)
                    {
                        var world = WkUtils.GetWorld();
                        if (world != null) TryInit(world);
                    }
                    OnTick(1.0f);
                });
            }
            catch (Exception ex)
            {
                A11y.A11yLog.Warning($"[SceneMonitor] 定时器回调异常: {ex.Message}");
            }
        }, null, 5000, 2000);
        A11y.A11yLog.Debug("[SceneMonitor] ⏱ 后备定时器已启动 (间隔 2s)");
    }

    static DateTime _lastUiCheck = DateTime.MinValue;
    static DateTime _lastFsmCheck = DateTime.MinValue;

    public static void OnTick(float deltaTime)
    {
        if (!_eventsSubscribed)
        {
            try
            {
                var world = WkUtils.GetWorld();
                if (world != null) TryInit(world);
            }
            catch (Exception ex)
            {
                A11y.A11yLog.Warning($"[SceneMonitor] OnTick 获取 World 失败: {ex.Message}");
            }
        }

        if (!_eventsSubscribed && _lifeTime == null) return;

        var now = DateTime.UtcNow;

        if ((now - _lastFsmCheck).TotalSeconds >= 1.0)
        {
            _lastFsmCheck = now;
            PollFsmState();
        }

        if ((now - _lastUiCheck).TotalSeconds >= 3.0)
        {
            _lastUiCheck = now;
            PollUiPages();
        }
    }

    static void PollFsmState()
    {
        try
        {
            if (_lifeTime == null) return;

            if (_lifeTime.IsInFSMState(SGI_Global.MainMenu))
                UpdateScene(GameState.GameScene.MainMenu);
            else if (_lifeTime.IsInFSMState(SGI_Global.InBattleStandAlone))
                UpdateScene(GameState.GameScene.InGame);
            else if (_lifeTime.IsInFSMState(SGI_Global.WaitGameStart))
                UpdateScene(GameState.GameScene.Startup);
            else if (_lifeTime.IsInFSMState(SGI_Global.WXLogin))
                UpdateScene(GameState.GameScene.LogIn);
            else if (_lifeTime.IsInTravelLevel())
                UpdateScene(GameState.GameScene.Loading);
        }
        catch (Exception ex)
        {
            A11y.A11yLog.Warning($"[SceneMonitor] PollFsmState 异常: {ex.Message}");
        }
    }

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
            return;
        }

        foreach (var pageId in GameState.VisiblePages.ToList())
        {
            try
            {
                if (!pageOp.PageGraphRawIsUiPageShowIng(pageId))
                    GameState.RemovePageSilent(pageId);
            }
            catch
            {
                // 单个页面查询异常不影响其他页面
            }
        }
    }
}
