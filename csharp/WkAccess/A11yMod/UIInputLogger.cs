namespace WkAccess.A11yMod;

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
