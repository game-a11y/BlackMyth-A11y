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
        A11yLog.Init();
        DebugCommands.PrintGameVersion();
        // 注册快捷键
        KeyBindings.RegisterAll();

        // 启动后备定时器，检测场景和 UI 变化
        A11y.SceneDetector.StartFallbackTimer();

        A11yLog.Info($"{Name} Init()");
    }

    public void DeInit()
    {
        A11yLog.Info($"{Name} DeInit");
        A11y.SceneDetector.Deinit();
    }

}
