namespace WkAccess.A11yMod;

/// <summary>
/// UI 焦点追踪 — 挂钩 BUI_Button 的聚焦/失焦事件。
/// 外部可通过 OnFocusEnter / OnFocusLeave 事件订阅。
/// </summary>
public static class UIFocusTracker
{
    public static event Action<int, string, string?>? OnFocusEnter;
    public static event Action<int, string>? OnFocusLeave;

    public static void NotifyEnter(int gsid, string className, string? text) =>
        OnFocusEnter?.Invoke(gsid, className, text);

    public static void NotifyLeave(int gsid, string className) =>
        OnFocusLeave?.Invoke(gsid, className);
}
