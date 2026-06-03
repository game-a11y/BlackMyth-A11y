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
        var sceneName = SceneChineseName(GameState.CurrentScene);

        // 顶层页面
        var topPage = GSG.UIMgr?.GetStackTopUIPage();
        var topName = topPage != null ? ((EnPageID)topPage.PageID).ToString() : "";

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
            var name = ((EnPageID)pageId).ToString();
            var cfg = GSG.UIMgr?.FindUIPageCfg(pageId);
            var uiName = cfg?.UIName.ToString() ?? topPage.GetType().Name;
            var order = cfg?.Order.ToString() ?? "?";
            A11yLog.Debug($"[PageSpeaker] TopPage: PageID={pageId}  {name}  UIName={uiName}  Order={order}");
        }

        if (GameState.VisiblePages.Count > 0)
        {
            var names = GameState.VisiblePages
                .OrderBy(id => id)
                .Select(id => ((EnPageID)id).ToString());
            A11yLog.Debug($"[PageSpeaker] 打开的界面: {string.Join(", ", names)}");
        }
    }

    static string SceneChineseName(GameState.GameScene scene) => scene switch
    {
        GameState.GameScene.Startup         => "启动画面",
        GameState.GameScene.LogIn           => "登录界面",
        GameState.GameScene.MainMenu        => "主菜单",
        GameState.GameScene.Loading         => "加载中",
        GameState.GameScene.InGame          => "游戏中",
        GameState.GameScene.PauseMenu       => "暂停菜单",
        GameState.GameScene.ShaderCompiling => "着色器编译中",
        _                                   => "未知场景",
    };
}
