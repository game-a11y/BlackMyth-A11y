using WkAccess.A11y;
using WkAccess.A11y.UE;
using WkAccess.BM.Locale;
using WkAccess.BM.UI;
namespace WkAccess.BM;

/// <summary>
/// 调试信息打印函数集合。快捷键通过 KeyBindings 引用此处的函数。
/// </summary>
internal static class DebugCommands
{
    public static void PrintGameBuildInfo()
    {
        A11yLog.Debug($"[[Game Build Info]]");
        A11yLog.Debug($"    ExeName    : {FApp.GetProjectName()}-{BuildEnv.BuildEnginePath}-{FApp.GetBuildConfiguration()}");
        A11yLog.Debug($"    Version    : {GSVersionUtil.GetAppVersionWithRevision()}");
        A11yLog.Debug($"    BuildTime  : {BuildEnv.BuildTime}");
        A11yLog.Debug($"    Environment: {DebugConfig.Environment}");

        A11yLog.Debug($"    [[Source]]");
        A11yLog.Debug($"    Branch     : {BuildEnv.GitBranchName}");
        A11yLog.Debug($"    Commit     : {BuildEnv.GitVersion}");
        A11yLog.Debug($"    P4Version  : {BuildEnv.P4Version}");
        A11yLog.Debug($"    EngineP4   : {BuildEnv.BuildEngineP4Ver}");
    }

    public static void PrintModBuildInfo()
    {
        var asm = typeof(BuildInfo).Assembly;
        var meta = asm.GetCustomAttributes(false)
                      .OfType<System.Reflection.AssemblyMetadataAttribute>()
                      .ToDictionary(a => a.Key, a => a.Value);

        A11yLog.Debug($"[[Mod Build Info]]");
        A11yLog.Debug($"    Version : {ModVersion}");
        var hash = meta.TryGetValue("GitHash", out var h) ? h : "unknown";
        var dirty = meta.TryGetValue("IsDirty", out var d) && d == "true";
        A11yLog.Debug($"    GitHash : {hash}{(dirty ? "+dev" : "")}");
        A11yLog.Debug($"    Built   : {(meta.TryGetValue("BuildTime", out var t) ? t : "unknown")}");
        A11yLog.Debug($"    DLL     : {typeof(BuildInfo).Assembly.GetName().Name}.dll");
    }

    public static void PrintPlayerInfo()
    {
        var player = WkUtils.GetControlledPawn();
        if (player == null)
        {
            A11yLog.Error("Player not found");
            return;
        }
        A11yLog.Warning($"Player: {player}");
        var hp = BGUFunctionLibraryCS.GetAttrValue(player, EBGUAttrFloat.Hp);
        var hpMax = BGUFunctionLibraryCS.GetAttrValue(player, EBGUAttrFloat.HpMax);
        A11yLog.Warning($"HP: {hp}/{hpMax}");
    }

    /// <summary>打印当前顶层页面的控件树（Ctrl+D2）</summary>
    public static void DumpTopPageWidgetTree()
    {
        try
        {
            var topPage = GSG.UIMgr?.GetStackTopUIPage();
            if (topPage == null)
            {
                A11yLog.Warning("[DebugCommands] 当前无顶层页面");
                return;
            }

            var root = topPage.GetRootBUIWidget();
            if (root == null || !root.IsValidLowLevel())
            {
                A11yLog.Warning("[DebugCommands] 顶层页面无 RootBUIWidget");
                return;
            }

            var pageId = topPage.PageID;
            var pageName = EnumLocale.Page((EnPageID)pageId);
            A11yLog.Warning($"=== 控件树: {pageName} (ID={pageId}) ===");
            UI.WidgetTreeDumper.DumpRecursive(root, indent: "", depth: 0, maxDepth: 12, log: A11yLog.Warning);
        }
        catch (Exception ex)
        {
            A11yLog.Error($"[DebugCommands] DumpTopPageWidgetTree 异常: {ex.Message}");
        }
    }
}
