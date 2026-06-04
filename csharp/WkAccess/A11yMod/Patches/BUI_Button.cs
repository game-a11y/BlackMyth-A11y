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
            if (!string.IsNullOrEmpty(text))
            {
                A11yLog.Debug($"[UI.Focus] 聚焦 {cn}#{gsid}: {text}");
                A11yTolk.Speak(text!, true);
            }
            else
                A11yLog.Debug($"[UI.Focus] 聚焦 {cn}#{gsid}");
            UIFocusTracker.NotifyEnter(gsid, cn);
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
