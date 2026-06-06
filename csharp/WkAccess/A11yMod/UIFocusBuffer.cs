namespace WkAccess.A11yMod;

/// <summary>
/// 焦点事件缓冲层 — 在 UIFocusTracker 和 UIFocusSpeaker 之间过滤重复事件。
/// 当前实现 pointer dedup；后续扩展 cascade gate / message dedup。
/// </summary>
public static class UIFocusBuffer
{
    // ── 可调参数 ──
    const int PointerDedupWindowMs = 150;  // 同 gsid 重复合并窗口

    // ── 输出：经过滤的干净事件 ──
    public static event Action<int, string, string?>? OnFocusOutput;

    // ── 去重状态 ──
    static int _lastGsid = -1;
    static DateTime _lastTime;

    public static void Init()
    {
        UIFocusTracker.OnFocusEnter += Enqueue;
    }

    static void Enqueue(int gsid, string className, string? text)
    {
        // Pointer dedup: 同 gsid 短窗口内重复 → 合并为一次
        if (gsid == _lastGsid
            && (DateTime.Now - _lastTime).TotalMilliseconds < PointerDedupWindowMs)
            return;

        _lastGsid = gsid;
        _lastTime = DateTime.Now;

        // TODO: Cascade gate — 页面切换时自动进入抑制模式
        //   if (sincePageOpen < GracePeriodMs || sinceLastFocus < ExtendWindowMs)
        //       store pending, don't flush yet

        OnFocusOutput?.Invoke(gsid, className, text);
    }
}
