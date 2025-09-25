using HarmonyLib;

namespace WkAccess.A11yPatch;

public static class BGWGameInstancePatch
{
    [HarmonyPatch(typeof(BGW_GameInstance_B1), nameof(BGW_GameInstance_B1.InitGameInstObj))]
    internal static class BGW_GameInstance_B1_InitGameInstObj_Patch
    {
        public static void Postfix()
        {
            A11yLog.Info("[BGW_GameInstance_B1] Post InitGameInstObj");
            WkAccess.PrintAllAssemblies();
        }
    }
}