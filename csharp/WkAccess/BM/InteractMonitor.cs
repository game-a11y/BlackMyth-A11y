using b1.ECS;

namespace WkAccess.BM;

/// <summary>
/// 可交互物品无障碍检测 — 挂钩 BPS_PlayerInteractComp.TickForInteractiveActor，
/// 当最佳交互目标变化时播报物品类别和交互动作。
/// </summary>
public static class InteractMonitor
{
    static EntitySharedRef? _lastBestRef;
    static int _lastUnitId = -1;

    static readonly Dictionary<EInteractType, string> _typeNames = new()
    {
        { EInteractType.RebirthPoint,     "土地庙" },
        { EInteractType.StandardObj,      "宝箱" },
        { EInteractType.StrangeBox,       "宝箱" },
        { EInteractType.Collection,       "采集物" },
        { EInteractType.DropItem,         "掉落物" },
        { EInteractType.TaskNpc,          "NPC" },
        { EInteractType.MeditationPoint,  "打坐点" },
        { EInteractType.Rescue,           "救援目标" },
        { EInteractType.BoLangGu,         "拨浪鼓" },
        { EInteractType.RequireItem,      "道具交互" },
        { EInteractType.Focus,            "焦点" },
        { EInteractType.Cricket,          "蟋蟀" },
    };

    public static void Start()
    {
        _lastBestRef = null;
        _lastUnitId = -1;
    }

    public static void Stop()
    {
        _lastBestRef = null;
        _lastUnitId = -1;
    }

    internal static void OnBestInteractChanged(EntitySharedRef? newRef)
    {
        if (newRef == _lastBestRef) return;
        _lastBestRef = newRef;

        if (newRef == null) return;
        if (!GameState.IsInGame) return;

        var actor = newRef.Actor();
        if (actor.IsNullOrDestroyed()) return;

        var data = BGU_DataUtil.GetReadOnlyData<BUC_InteractData>(actor);
        if (data == null) return;

        if (data.InteractiveUnitID == _lastUnitId) return;
        _lastUnitId = data.InteractiveUnitID;

        var text = BuildSpeakText(data);
        if (!string.IsNullOrEmpty(text))
        {
            A11yLog.Info($"[Interact] {text} (UnitID={data.InteractiveUnitID})");
            A11yTolk.Speak(text!, interrupt: true);
        }
    }

    static string? BuildSpeakText(BUC_InteractData data)
    {
        var objectName = GetObjectName(data);
        var actionName = GetActionName(data);

        if (objectName != null && actionName != null) return $"{objectName}，{actionName}";
        if (objectName != null) return objectName;
        if (actionName != null) return actionName;
        return "可交互物品";
    }

    static string? GetObjectName(BUC_InteractData data)
    {
        var commDesc = data.InteractiveUnitCommDesc;
        if (commDesc == null) return null;
        return _typeNames.TryGetValue(commDesc.InteractType, out var name) ? name : null;
    }

    static string? GetActionName(BUC_InteractData data)
    {
        if (data.ActionList.Count == 0) return null;
        return B1WidgetResolvers.ResolveFText(data.ActionList[0].InteractName);
    }
}

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
