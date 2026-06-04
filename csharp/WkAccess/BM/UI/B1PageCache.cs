namespace WkAccess.BM.UI;

/// <summary>全局唯一页面的缓存引用</summary>
public static class B1PageCache
{
    internal static readonly Dictionary<string, UUserWidget> _pageCache = new();

    /// <summary>缓存全局唯一页面引用</summary>
    public static void CachePage(string className, UUserWidget instance)
    {
        _pageCache[className] = instance;
        A11yLog.Info($"[PageCache] 缓存页面: {className}");
    }

    /// <summary>获取缓存的全局唯一页面引用</summary>
    public static UUserWidget? GetCachedPage(string className) =>
        _pageCache.TryGetValue(className, out var page) ? page : null;
}
