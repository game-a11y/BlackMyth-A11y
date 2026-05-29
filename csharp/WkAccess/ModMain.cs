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
        A11yLog.Init(fileLogDir: Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory ?? ".", Common.ModDir, BuildInfo.ModName));
        DebugCommands.PrintGameBuildInfo();
        DebugCommands.PrintModBuildInfo();
        KeyBindings.RegisterAll();
        B1WidgetExtractors.RegisterAll(UIScreenTextProvider.Register);
        B1.SceneMonitor.Start();
        InteractMonitor.Start();
        _harmony.PatchAll();
        A11yLog.Info($"{Name} Init()");
    }

    public void DeInit()
    {
        A11yLog.Info($"{Name} DeInit");
        _harmony.UnpatchAll();
        B1.SceneMonitor.Stop();
        InteractMonitor.Stop();
        A11yLog.Deinit();
    }
}
