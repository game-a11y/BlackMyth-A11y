using CSharpModBase.Input;
using HarmonyLib;
using System.Reflection;

namespace WkAccess;

public sealed class WkAccess : ICSharpMod
{
    private readonly Harmony _harmony;

    public string Name => ModName;
    public string Version => ModVersion;

    public WkAccess()
    {
        _harmony = new Harmony($"{BuildInfo.ModName}.{BuildInfo.ModVersion}");
        A11yLog.Info($"{Name} Constructor called @ {DateTime.Now}");
    }

    /// <summary>
    /// 应用所有补丁，打印已应用的补丁方法。
    /// </summary>
    void PatchAll()
    {
        _harmony.PatchAll();
        PrintPatchedMethods();
    }

    public void Init()
    {
        A11yLog.SetConsoleUTF8();
        A11yLog.Info($"{Name} Init()");
        var gameVersion = GSVersionUtil.GetAppVersionWithRevision();
        A11yLog.Info($"Game Version: {gameVersion}");
        Utils.RegisterKeyBind(Key.ENTER, () => Console.WriteLine("Enter pressed"));
        Utils.RegisterKeyBind(ModifierKeys.Control, Key.ENTER, FindPlayer);
        Utils.RegisterKeyBind(ModifierKeys.Control, Key.F11, PrintPatchedMethods);
        Utils.RegisterKeyBind(ModifierKeys.Control, Key.F12, PrintAllAssemblies);
        
        try
        {

            MethodInfo m = null;
            
            // 方法1：直接通过字符串获取方法
            m = AccessTools.Method("B1UI.GSUI.UIStartGame:OnLoadingScreenClose");
            if (m != null)
            {
                A11yLog.Info($"Found method: {m.DeclaringType.FullName}.{m.Name}");
                PatchAll();
                return;
            }
            
            A11yLog.Warning("Not work: AccessTools.Method(\"B1UI.GSUI.UIStartGame:OnLoadingScreenClose\")");
            
            // 方法2：先获取类型再获取方法
            var uiStartGameType = AccessTools.TypeByName("B1UI.GSUI.UIStartGame");
            if (uiStartGameType != null)
            {
                m = AccessTools.Method(uiStartGameType, "OnLoadingScreenClose");
                if (m == null)
                {
                    m = AccessTools.Method(uiStartGameType, "PlayAnimtionOnConstruct");
                }
                
                if (m != null)
                {
                    A11yLog.Info($"Found method: {m.DeclaringType.FullName}.{m.Name}");
                    PatchAll();
                    return;
                }
            }
            
            A11yLog.Warning("Not work: AccessTools.TypeByName(\"B1UI.GSUI.UIStartGame\")");
            
            // 方法3：通过程序集名称获取类型
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "B1UI_GSE.Script");
                
            if (assembly == null)
            {
                A11yLog.Error("Assembly 'B1UI_GSE.Script' not found");
                // 列出所有已加载的程序集进行调试
                PrintAllAssemblies();
                PatchAll();
                return;
            }
            
            A11yLog.Info($"Found assembly: {assembly.FullName}");
            uiStartGameType = assembly.GetType("B1UI.GSUI.UIStartGame");
            
            if (uiStartGameType == null)
            {
                A11yLog.Error("Type 'B1UI.GSUI.UIStartGame' not found in assembly");
                // 列出程序集中的所有类型进行调试
                var types = assembly.GetTypes().Where(t => t.Name.Contains("UIStartGame")).ToArray();
                A11yLog.Info($"Similar types found: {string.Join(", ", types.Select(t => t.FullName))}");
                PatchAll();
                return;
            }
            
            A11yLog.Info($"Found type: {uiStartGameType.FullName}");
            
            // 尝试获取方法
            m = AccessTools.Method(uiStartGameType, "OnLoadingScreenClose");
            if (m == null)
            {
                m = AccessTools.Method(uiStartGameType, "PlayAnimtionOnConstruct");
            }
            
            if (m != null)
            {
                A11yLog.Info($"Found method: {m.DeclaringType.FullName}.{m.Name}");
            }
            else
            {
                A11yLog.Error("Failed to find any target method in UIStartGame class");
            }
        }
        catch (Exception ex)
        {
            A11yLog.Exception($"获取 UIStartGame 方法失败", ex);
        }

        PatchAll();
    }

    public void DeInit()
    {
        A11yLog.Info($"{Name} DeInit");
    
        PrintPatchedMethods();
        _harmony.UnpatchAll();
        PrintPatchedMethods();
    }

    /// <summary>
    /// 打印所有已应用的补丁方法。
    /// </summary>
    void PrintPatchedMethods()
    {
        A11yLog.Warning($"All Patched Methods count: {_harmony.GetPatchedMethods().Count()}");
        foreach (var method in _harmony.GetPatchedMethods()) {
            A11yLog.Warning($"\t{method.DeclaringType.FullName}.{method.Name}");
        }
    }

    public static void PrintAllAssemblies()
    {
        A11yLog.Warning("All loaded assemblies:");
        // 列出所有已加载的程序集进行调试
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetName().Name.Contains("B1") || a.GetName().Name.Contains("b1") || a.GetName().Name.Contains("GSE"))
            .Select(a => a.GetName().Name)
            .ToArray();
        A11yLog.Info($"  Related assemblies found: {string.Join(", ", assemblies)}");
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