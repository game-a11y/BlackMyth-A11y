namespace WkAccess.A11yMod;

/// <summary>
/// F1 快捷键：用语音播报当前 UI 场景和页面信息。
/// </summary>
internal static class PageSpeaker
{
    public static void DescribeCurrentUI()
    {
        try
        {
            DescribeCurrentUIImpl();
        }
        catch (Exception ex)
        {
            A11yLog.Error($"[PageSpeaker] DescribeCurrentUI 异常: {ex.Message}");
        }
    }

    static void DescribeCurrentUIImpl()
    {
        var sceneName = EnumLocale.Scene(GameState.CurrentScene);

        // 顶层页面
        var topPage = GSG.UIMgr?.GetStackTopUIPage();
        var topName = topPage != null ? EnumLocale.Page((EnPageID)topPage.PageID) : "";

        var parts = new List<string>();
        if (!string.IsNullOrEmpty(topName))
            parts.Add($"当前场景：{sceneName}，{topName}");
        else
            parts.Add($"当前场景：{sceneName}");

        // 地图 + 关卡
        if (!string.IsNullOrEmpty(GameState.CurrentMapName)
            && GameState.CurrentMapName != "None")
        {
            parts.Add($"地图：{GameState.CurrentMapName}");
        }
        if (GameState.CurrentLevelId > 0)
            parts.Add($"关卡ID：{GameState.CurrentLevelId}");

        var description = string.Join("。", parts);
        A11yTolk.Speak(description, interrupt: true);

        if (topPage != null)
        {
            var pageId = topPage.PageID;
            var name = EnumLocale.Page((EnPageID)pageId);
            var cfg = GSG.UIMgr?.FindUIPageCfg(pageId);
            var uiName = cfg?.UIName.ToString() ?? topPage.GetType().Name;
            var order = cfg?.Order.ToString() ?? "?";
            A11yLog.Debug($"[PageSpeaker] TopPage: PageID={pageId}  {name}  UIName={uiName}  Order={order}");

            B1.UI.B1InputTipsScanner.ScanAndSpeak(pageId);
        }

        if (GameState.VisiblePages.Count > 0)
        {
            var names = GameState.VisiblePages
                .OrderBy(id => id)
                .Select(id => EnumLocale.Page((EnPageID)id));
            A11yLog.Debug($"[PageSpeaker] 打开的界面: {string.Join(", ", names)}");
        }
    }
}
