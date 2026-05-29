namespace WkAccess.B1;

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
        A11yLog.Debug($"[[Mod Build Info]]");
        A11yLog.Debug($"    Version : {ModVersion}");
        var hashSuffix = BuildMeta.IsDirty ? "+dev" : "";
        A11yLog.Debug($"    GitHash : {BuildMeta.GitHash}{hashSuffix}");
        A11yLog.Debug($"    Built   : {BuildMeta.BuildTimeUtc} UTC");
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
}
