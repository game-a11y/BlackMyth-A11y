using HarmonyLib;

namespace WkAccess.A11yPatch;

/// <summary>
/// BGW_ILRuntimeMgr / BGW_ManagedReflectMgr 的补丁。
/// 不能在 Init() 中用 [HarmonyPatch] 静态注册，因为定义它们的程序集 BtlSvr.Main
/// 此时尚未加载，typeof(…) 会导致 TypeLoadException。
/// 改为由 Init() 中立即尝试 + AssemblyLoad 事件兜底，通过 ApplyPatches 手动注册。
/// </summary>
public static class BGWManagersPatch
{
    /// <summary>延迟应用所有管理器补丁（此时 BtlSvr.Main 应已加载）。</summary>
    public static void ApplyPatches(Harmony harmony)
    {
        Patch_ILRuntimeMgr_OnInit(harmony);
        Patch_ManagedReflectMgr_OnInit(harmony);
        Patch_ManagedReflectMgr_LoadScriptAssemblyFile(harmony);
    }

    static void Patch_ILRuntimeMgr_OnInit(Harmony harmony)
    {
        try
        {
            var type = AccessTools.TypeByName("b1.BGW_ILRuntimeMgr");
            if (type == null)
            {
                A11yLog.Warning("skip BGW_ILRuntimeMgr.OnInit — type not found");
                return;
            }
            var method = AccessTools.Method(type, "OnInit");
            if (method == null)
            {
                A11yLog.Warning("skip BGW_ILRuntimeMgr.OnInit — method not found");
                return;
            }

            var postfix = AccessTools.Method(typeof(BGWManagersPatch), nameof(OnInit_Postfix));
            harmony.Patch(method, postfix: new HarmonyMethod(postfix));
            A11yLog.Info("[BGW_ILRuntimeMgr] OnInit patched");
        }
        catch (Exception ex)
        {
            A11yLog.Warning($"Cannot patch BGW_ILRuntimeMgr.OnInit: {ex.Message}");
        }
    }

    static void Patch_ManagedReflectMgr_OnInit(Harmony harmony)
    {
        try
        {
            var type = AccessTools.TypeByName("b1.BGW_ManagedReflectMgr");
            if (type == null)
            {
                A11yLog.Warning("skip BGW_ManagedReflectMgr.OnInit — type not found");
                return;
            }
            var method = AccessTools.Method(type, "OnInit");
            if (method == null)
            {
                A11yLog.Warning("skip BGW_ManagedReflectMgr.OnInit — method not found");
                return;
            }

            var postfix = AccessTools.Method(typeof(BGWManagersPatch), nameof(ManagedReflectMgr_OnInit_Postfix));
            harmony.Patch(method, postfix: new HarmonyMethod(postfix));
            A11yLog.Info("[BGW_ManagedReflectMgr] OnInit patched");
        }
        catch (Exception ex)
        {
            A11yLog.Warning($"Cannot patch BGW_ManagedReflectMgr.OnInit: {ex.Message}");
        }
    }

    static void Patch_ManagedReflectMgr_LoadScriptAssemblyFile(Harmony harmony)
    {
        try
        {
            var type = AccessTools.TypeByName("b1.BGW_ManagedReflectMgr");
            if (type == null)
            {
                A11yLog.Warning("skip BGW_ManagedReflectMgr.LoadScriptAssemblyFile — type not found");
                return;
            }
            var method = AccessTools.Method(type, "LoadScriptAssemblyFile");
            if (method == null)
            {
                A11yLog.Warning("skip BGW_ManagedReflectMgr.LoadScriptAssemblyFile — method not found");
                return;
            }

            var prefix = AccessTools.Method(typeof(BGWManagersPatch), nameof(LoadScriptAssemblyFile_Prefix));
            var postfix = AccessTools.Method(typeof(BGWManagersPatch), nameof(LoadScriptAssemblyFile_Postfix));
            harmony.Patch(method, prefix: new HarmonyMethod(prefix), postfix: new HarmonyMethod(postfix));
            A11yLog.Info("[BGW_ManagedReflectMgr] LoadScriptAssemblyFile patched");
        }
        catch (Exception ex)
        {
            A11yLog.Warning($"Cannot patch BGW_ManagedReflectMgr.LoadScriptAssemblyFile: {ex.Message}");
        }
    }

    // ---- Patch handlers ----

    public static void OnInit_Postfix()
    {
        A11yLog.Info("[BGW_ILRuntimeMgr] OnInit 方法已执行 — ILRuntime 初始化开始");
    }

    public static void ManagedReflectMgr_OnInit_Postfix()
    {
        A11yLog.Info("[BGW_ManagedReflectMgr] OnInit 方法已执行 — 托管反射初始化开始");
    }

    public static bool LoadScriptAssemblyFile_Prefix()
    {
        A11yLog.Info("[BGW_ManagedReflectMgr] LoadScriptAssemblyFile 方法被调用");
        WkAccess.PrintAllAssemblies();
        return true;
    }

    public static void LoadScriptAssemblyFile_Postfix()
    {
        A11yLog.Info("[BGW_ManagedReflectMgr] LoadScriptAssemblyFile 方法已执行");
        WkAccess.PrintAllAssemblies();
        // TODO: 在这里挂钩 B1UI_GSE.Script
    }
}
