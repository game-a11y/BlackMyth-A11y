using HarmonyLib;
using b1.UI;
using UnrealEngine.Runtime;

namespace WkAccess.A11y.UI;

/// <summary>挂钩 BUI_Widget.Construct，缓存全局唯一页面引用</summary>
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
                UIScreenTextProvider._pageCache[cn] = __instance;
                A11yLog.Info($"[PageCache] 缓存页面: {cn}");
                return;
            }
        }
    }
}
