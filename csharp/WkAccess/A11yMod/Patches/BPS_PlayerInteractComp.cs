using b1.ECS;

namespace WkAccess.A11yMod.Patches;

[HarmonyPatch(typeof(BPS_PlayerInteractComp), "TickForInteractiveActor")]
static class H_TickForInteractiveActor
{
    static readonly AccessTools.FieldRef<BPS_PlayerInteractComp, InteractContext>
        _getContext = AccessTools.FieldRefAccess<BPS_PlayerInteractComp, InteractContext>("Context");

    static void Postfix(BPS_PlayerInteractComp __instance)
    {
        try
        {
            var context = _getContext(__instance);
            if (context?.PlayerInteractData == null) return;

            InteractMonitor.OnBestInteractChanged(context.PlayerInteractData.BestInteractEntityRef);
        }
        catch (System.Exception ex)
        {
            A11yLog.Error($"[H_TickForInteractiveActor] {ex.Message}");
        }
    }
}
