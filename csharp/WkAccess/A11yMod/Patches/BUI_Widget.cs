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
