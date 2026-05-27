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
        DebugCommands.PrintGameVersion();
        // 注册快捷键
        KeyBindings.RegisterAll();

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

}
