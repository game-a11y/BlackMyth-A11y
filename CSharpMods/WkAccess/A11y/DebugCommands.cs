namespace WkAccess.A11y;

/// <summary>
/// 调试信息打印函数集合。快捷键通过 KeyBindings 引用此处的函数。
/// </summary>
internal static class DebugCommands
{
    public static void PrintBuildInfo()
    {
        A11yLog.Info($"=== BuildInfo ===");
        A11yLog.Info($"ExeName  : {FApp.GetProjectName()}-{BuildEnv.BuildEnginePath}-{FApp.GetBuildConfiguration()}");
        A11yLog.Info($"Version  : {GSVersionUtil.GetAppVersionWithRevision()}");
        A11yLog.Info($"BuildTime: {BuildEnv.BuildTime}");
        A11yLog.Info($"Environment: {DebugConfig.Environment}");

        A11yLog.Info($"=== Source ===");
        A11yLog.Info($"Branch   : {BuildEnv.GitBranchName}");
        A11yLog.Info($"Commit   : {BuildEnv.GitVersion}");
        A11yLog.Info($"P4Version: {BuildEnv.P4Version}");
        A11yLog.Info($"EngineP4 : {BuildEnv.BuildEngineP4Ver}");
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
}
