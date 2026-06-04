using WkAccess.A11y;
using WkAccess.A11y.UE;
using WkAccess.A11yMod;
using WkAccess.BM.UI;

namespace WkAccess.A11yMod.Patches;

[HarmonyPatch(typeof(BUI_Widget), "Construct_Implementation")]
static class H_PageConstruct
{
    static readonly string[] _targetPages = {
        "BUI_EquipMain_C", "BUI_BagMain_C", "BUI_TalentMain_C",
        "BUI_LearnTalent_C", "BUI_TravelNotesMain_C", "BUI_RoleMain_C"
    };

    static void Postfix(BUI_Widget __instance)
    {
        var cn = __instance.GetClass().GetFName().ToString();
        foreach (var p in _targetPages)
        {
            if (cn == p)
            {
                B1PageCache.CachePage(cn, __instance);
                return;
            }
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
