using WkAccess.A11y;
using WkAccess.A11yMod;
using WkAccess.BM;
using WkAccess.BM.UI;

namespace WkAccess;

public sealed class WkAccess : ICSharpMod
{
    static readonly Harmony _harmony = new($"{BuildInfo.ModName}.{BuildInfo.ModVersion}");

    public string Name => ModName;
    public string ModNameFull => $"{ModName} MOD";
    public string Version => ModVersion;

    public WkAccess()
    {
        A11yLog.Init(fileLogDir: Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory ?? ".", Common.ModDir, BuildInfo.ModName));

        // 输出游戏版本信息
        DebugCommands.PrintGameBuildInfo();
        A11yLog.Info($"{ModNameFull} Constructor called @ {DateTime.Now}");
    }

    /// <summary>
    /// 初始化 MOD 模块
    /// </summary>
    /// <remarks>注意：热加载模组，会再次调用此函数！</remarks>
    public void Init()
    {
        A11yLog.Info($"{ModNameFull} Init() Start:");
        DebugCommands.PrintModBuildInfo();

        KeyBindings.RegisterAll();
        B1WidgetExtractors.RegisterAll(UIScreenTextProvider.Register);

        SceneMonitor.Start();
        B1InputTipsScanner.Init();
        B1InputTipsScanner.OnInputTipsScanned += (text) => A11yTolk.Speak(text, false);
        InteractMonitor.Start();
        InteractSpeaker.Init();
        UIFocusSpeaker.Init();
        _harmony.PatchAll();

        A11yLog.Info($"{ModNameFull} Init END.");
    }

    public void DeInit()
    {
        A11yLog.Info($"{ModNameFull} DeInit() Start:");

        _harmony.UnpatchAll();
        InteractMonitor.Stop();
        SceneMonitor.Stop();


        A11yLog.Deinit();
    }
}