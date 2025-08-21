using HarmonyLib;

namespace WkAccess.A11yPatch;

public static class BGWManagersPatch
{
    /*
        注意：仅当
            if (DebugConfig.ILRuntime)
            {
                base.CreateUObj<BGW_ILRuntimeMgr>();
            }
            else
            {
                base.CreateUObj<BGW_ManagedReflectMgr>();
            }
     */
    [HarmonyPatch(typeof(BGW_ILRuntimeMgr), nameof(BGW_ILRuntimeMgr.OnInit))]
    internal static class BGW_ILRuntimeMgrPatch__OnInit
    {
        public static void Postfix()
        {
            A11yLog.Info("[BGW_ILRuntimeMgr] OnInit 方法已执行 - ILRuntime 初始化开始");
        }
    }

    [HarmonyPatch(typeof(BGW_ManagedReflectMgr), nameof(BGW_ManagedReflectMgr.OnInit))]
    internal static class BGW_ManagedReflectMgrPatch
    {
        public static void Postfix()
        {
            A11yLog.Info("[BGW_ManagedReflectMgr] OnInit 方法已执行 - 托管反射初始化开始");
        }
    }


    [HarmonyPatch(typeof(BGW_ManagedReflectMgr), nameof(BGW_ManagedReflectMgr.LoadScriptAssemblyFile))]
    internal static class BGW_ManagedReflectMgr__LoadScriptAssemblyFile
    {
        public static bool Prefix()
        {
            A11yLog.Info("[BGW_ManagedReflectMgr] LoadScriptAssemblyFile 方法被调用");
            WkAccess.PrintAllAssemblies();
            return true;
        }

        public static void Postfix()
        {
            A11yLog.Info("[BGW_ManagedReflectMgr] LoadScriptAssemblyFile 方法已执行");
            WkAccess.PrintAllAssemblies();
            // TODO: 在这里挂钩 B1UI_GSE.Script
        }
    }
}