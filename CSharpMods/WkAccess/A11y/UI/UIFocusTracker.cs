using HarmonyLib;

namespace WkAccess.A11y.UI;

[HarmonyPatch(typeof(BUI_Button), "OnAddedToFocusPath_Implementation")]
static class H_FocusEnter
{
    static void Postfix(BUI_Button __instance)
    {
        var gsid = __instance.GetGSID();
        if (gsid >= 0)
            A11yLog.Info($"[UI.Focus] 聚焦 WidgetID={gsid}");
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
            if (gsid >= 0)
                A11yLog.Info($"[UI.Focus] 失焦 WidgetID={gsid}");
        }
    }
}
