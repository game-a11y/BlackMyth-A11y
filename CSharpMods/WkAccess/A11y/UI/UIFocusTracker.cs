using HarmonyLib;

namespace WkAccess.A11y.UI;

/// <summary>
/// UI 焦点追踪 — 挂钩 BUI_Button 的聚焦/失焦事件。
/// 外部可通过 OnFocusEnter / OnFocusLeave 事件订阅。
/// </summary>
public static class UIFocusTracker
{
    /// <summary>按钮聚焦时触发。(GSID, RootWidget 类型名)</summary>
    public static event Action<int, string>? OnFocusEnter;

    /// <summary>按钮失焦时触发。(GSID, RootWidget 类型名)</summary>
    public static event Action<int, string>? OnFocusLeave;

    internal static void NotifyEnter(int gsid, string className) =>
        OnFocusEnter?.Invoke(gsid, className);

    internal static void NotifyLeave(int gsid, string className) =>
        OnFocusLeave?.Invoke(gsid, className);
}

[HarmonyPatch(typeof(BUI_Button), "OnAddedToFocusPath_Implementation")]
static class H_FocusEnter
{
    static void Postfix(BUI_Button __instance)
    {
        var gsid = __instance.GetGSID();
        if (gsid < 0) return;
        var cn = __instance.GetType().Name;
        var text = UIScreenTextProvider.Extract(__instance as UnrealEngine.UMG.UUserWidget);
        if (!string.IsNullOrEmpty(text))
        {
            A11yLog.Info($"[UI.Focus] 聚焦 {text} (GSID={gsid})");
            A11yTolk.Speak(text!, true);
        }
        else
            A11yLog.Info($"[UI.Focus] 聚焦 WidgetID={gsid} ({cn})");
        UIFocusTracker.NotifyEnter(gsid, cn);
    }
}

[HarmonyPatch(typeof(BUI_Widget), "OnRemovedFromFocusPath_Implementation")]
static class H_FocusLeave
{
    static void Postfix(BUI_Widget __instance)
    {
        if (__instance is BUI_Button btn)
        {
            var gsid = btn.GetGSID();
            if (gsid < 0) return;
            var cn = __instance.GetType().Name;
            A11yLog.Info($"[UI.Focus] 失焦 WidgetID={gsid} ({cn})");
            UIFocusTracker.NotifyLeave(gsid, cn);
        }
    }
}
