using CSharpModBase.Input;

// using HarmonyLib;

namespace WkAccess;

public sealed class WkAccess : ICSharpMod
{
    // private readonly Harmony harmony;

    public string Name => ModName;
    public string Version => ModVersion;

    public void Init()
    {
        A11yLog.SetConsoleUTF8();
        A11yLog.Info($"{Name} Init()");
        Utils.RegisterKeyBind(Key.ENTER, () => Console.WriteLine("Enter pressed"));
        Utils.RegisterKeyBind(ModifierKeys.Control, Key.ENTER, FindPlayer);

        // hook
        // harmony.PatchAll();
    }

    public void DeInit()
    {
        A11yLog.Info($"{Name} DeInit");
        // harmony.UnpatchAll();
    }

    private static void FindPlayer()
    {
        A11yLog.Info("Ctrl+Enter pressed");
        var player = WkUtils.GetControlledPawn();
        if (player == null)
        {
            A11yLog.Error("Player not found");
        }
        else
        {
            A11yLog.Warning($"Player found: {player}");
            var hp = BGUFunctionLibraryCS.GetAttrValue(player, EBGUAttrFloat.Hp);
            var hpMax = BGUFunctionLibraryCS.GetAttrValue(player, EBGUAttrFloat.HpMax);
            A11yLog.Warning($"HP: {hp}/{hpMax}");
        }
    }
}