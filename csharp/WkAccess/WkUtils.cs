namespace WkAccess;

public static class WkUtils
{
    public static UWorld? GetWorld()
    {
        var uobjectRef = GCHelper.FindRef(FGlobals.GWorld);
        return uobjectRef?.Managed as UWorld;
    }

    public static APawn? GetControlledPawn() => UGSE_EngineFuncLib.GetFirstLocalPlayerController(GetWorld()).GetControlledPawn();
}
