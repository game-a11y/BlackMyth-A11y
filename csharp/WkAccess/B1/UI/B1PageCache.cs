namespace WkAccess.B1.UI;

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
                B1PageCache._pageCache[cn] = __instance;
                A11yLog.Info($"[PageCache] 缓存页面: {cn}");
                return;
            }
        }
    }
}

/// <summary>全局唯一页面的缓存引用</summary>
public static class B1PageCache
{
    internal static readonly Dictionary<string, UUserWidget> _pageCache = new();

    /// <summary>获取缓存的全局唯一页面引用</summary>
    public static UUserWidget? GetCachedPage(string className) =>
        _pageCache.TryGetValue(className, out var page) ? page : null;
}
