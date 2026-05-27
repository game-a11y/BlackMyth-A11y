using CSharpModBase.Input;
using HarmonyLib;

namespace WkAccess;

public sealed class WkAccess : ICSharpMod
{
    static readonly Harmony _harmony = new($"{BuildInfo.ModName}.{BuildInfo.ModVersion}");

    public string Name => ModName;
    public string Version => ModVersion;

    public WkAccess()
    {
        A11yLog.Info($"{Name} Constructor called @ {DateTime.Now}");
    }

    public void Init()
    {
        A11yLog.Init();
        DebugCommands.PrintBuildInfo();
        KeyBindings.RegisterAll();
        A11y.SceneDetector.StartFallbackTimer();
        _harmony.PatchAll();
        A11yLog.Info($"{Name} Init()");
    }

    public void DeInit()
    {
        A11yLog.Info($"{Name} DeInit");
        _harmony.UnpatchAll();
        A11y.SceneDetector.Deinit();
    }
}
