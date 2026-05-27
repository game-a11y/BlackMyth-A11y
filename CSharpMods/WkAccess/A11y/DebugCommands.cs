namespace WkAccess.A11y;

/// <summary>
/// 调试信息打印函数集合。快捷键通过 KeyBindings 引用此处的函数。
/// </summary>
internal static class DebugCommands
{
    public static void PrintGameVersion()
    {
        var v = GSVersionUtil.GetAppVersionWithRevision();
        A11yLog.Info($"Game Version: {v}");
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
