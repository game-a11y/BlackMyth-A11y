namespace WkAccess.A11y.UE;

public static class WkUtils
{
    public static UWorld? GetWorld()
    {
        var uobjectRef = GCHelper.FindRef(FGlobals.GWorld);
        return uobjectRef?.Managed as UWorld;
    }

    public static APawn? GetControlledPawn() =>
        UGSE_EngineFuncLib.GetFirstLocalPlayerController(GetWorld()).GetControlledPawn();

    /// <summary>安全获取 UObject 的 UE4 运行时类名，获取不到时回退到 C# 类型名。</summary>
    public static string GetClassName(UObject obj)
    {
        try
        {
            var unrealName = obj.GetClass()?.GetName();
            if (!string.IsNullOrEmpty(unrealName)) return unrealName!;
        }
        catch { }
        return obj.GetType().Name;
    }
}
