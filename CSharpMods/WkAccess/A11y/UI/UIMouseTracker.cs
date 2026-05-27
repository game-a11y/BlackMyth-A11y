using HarmonyLib;

namespace WkAccess.A11y.UI;

[HarmonyPatch(typeof(BUI_Button), "OnMouseButtonDown_Implementation")]
static class H_MouseDown
{
    static void Postfix(BUI_Button __instance)
    {
        var gsid = __instance.GetGSID();
        if (gsid >= 0)
            A11yLog.Info($"[UI.Click] 点击 WidgetID={gsid}");
    }
}

[HarmonyPatch(typeof(BUI_Button), "OnKeyUp_Implementation")]
static class H_KeyUp
{
    static void Postfix(BUI_Button __instance)
    {
        var gsid = __instance.GetGSID();
        if (gsid >= 0)
            A11yLog.Info($"[UI.KeyUp] 按键 WidgetID={gsid}");
    }
}
