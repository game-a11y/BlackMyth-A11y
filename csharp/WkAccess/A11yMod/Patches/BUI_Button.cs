using WkAccess.A11y;
using WkAccess.A11y.UE;
using WkAccess.A11yMod;
namespace WkAccess.A11yMod.Patches;

[HarmonyPatch(typeof(BUI_Button), "OnAddedToFocusPath_Implementation")]
static class H_FocusEnter
{
    static void Postfix(BUI_Button __instance)
    {
        try
        {
            var gsid = __instance.GetGSID();
            if (gsid < 0) return;
            var cn = WkUtils.GetClassName(__instance);
            var text = UIScreenTextProvider.Extract(__instance as UnrealEngine.UMG.UUserWidget);
            UIFocusTracker.NotifyEnter(gsid, cn, text);
        }
        catch (System.Exception ex)
        {
            A11yLog.Error($"[H_FocusEnter] {ex.Message}");
        }
    }
}

[HarmonyPatch(typeof(BUI_Widget), "OnRemovedFromFocusPath_Implementation")]
static class H_FocusLeave
{
    static void Postfix(BUI_Widget __instance)
    {
        try
        {
            if (__instance is BUI_Button btn)
            {
                var gsid = btn.GetGSID();
                if (gsid < 0) return;
                var cn = WkUtils.GetClassName(__instance);
                UIFocusTracker.NotifyLeave(gsid, cn);
            }
        }
        catch (System.Exception ex)
        {
            A11yLog.Error($"[H_FocusLeave] {ex.Message}");
        }
    }
}

[HarmonyPatch(typeof(BUI_Button), "OnMouseButtonDown_Implementation")]
static class H_MouseDown
{
    static void Postfix(BUI_Button __instance)
    {
        var gsid = __instance.GetGSID();
        if (gsid >= 0)
        {
            var cn = WkUtils.GetClassName(__instance);
            A11yLog.Debug($"[UI.Click] 点击 {cn}#{gsid}");
        }
    }
}

[HarmonyPatch(typeof(BUI_Button), "OnKeyUp_Implementation")]
static class H_KeyUp
{
    static void Postfix(BUI_Button __instance)
    {
        var gsid = __instance.GetGSID();
        if (gsid >= 0)
        {
            var cn = WkUtils.GetClassName(__instance);
            A11yLog.Debug($"[UI.KeyUp] 按键 {cn}#{gsid}");
        }
    }
}
