using CSharpModBase.Input;

namespace WkAccess;

public sealed class WkAccess : ICSharpMod
{
    public string Name => ModName;
    public string Version => ModVersion;

    public WkAccess()
    {
        A11yLog.Info($"{Name} Constructor called @ {DateTime.Now}");
    }

    public void Init()
    {
        A11yLog.SetConsoleUTF8();
        A11yLog.Info($"{Name} Init()");
        var gameVersion = GSVersionUtil.GetAppVersionWithRevision();
        A11yLog.Info($"Game Version: {gameVersion}");
        Utils.RegisterKeyBind(Key.ENTER, () => Console.WriteLine("Enter pressed"));
        Utils.RegisterKeyBind(ModifierKeys.Control, Key.ENTER, FindPlayer);
        Utils.RegisterKeyBind(ModifierKeys.Control, Key.D1, () => A11y.SceneDetector.PrintCurrentUI());

        // 启动后备定时器，检测场景和 UI 变化
        AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
        A11y.SceneDetector.StartFallbackTimer();
    }

    static void OnAssemblyLoad(object? sender, AssemblyLoadEventArgs args)
    {
        if (args.LoadedAssembly.GetName().Name == "B1UI_GSE.Script")
        {
            A11yLog.Info("B1UI_GSE.Script loaded, notifying SceneDetector...");
            A11y.SceneDetector.TryInitGSG();
        }
    }

    public void DeInit()
    {
        A11yLog.Info($"{Name} DeInit");
        A11y.SceneDetector.Deinit();
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
