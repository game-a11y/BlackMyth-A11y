using System.Reflection;
using HarmonyLib;
using UnrealEngine.Runtime;

namespace WkAccess.A11yPatch;

/// <summary>
/// 场景检测器补丁：挂钩场景加载、游戏生命周期和 UI 切换的关键方法。
/// 所有补丁通过 SceneDetector 输出日志，用于无障碍功能可行性测试。
/// </summary>
public static class SceneDetectorPatch
{
    /// <summary>延迟注册所有场景检测补丁（BtlSvr.Main 加载后调用）。</summary>
    public static void ApplyPatches(Harmony harmony)
    {
        Patch_GameInstance_Init(harmony);
        Patch_GameMode_ReceiveTick(harmony);
        Patch_GameState_Tick(harmony);
        Patch_GameMode_HandleLeavingMap(harmony);
        Patch_GameMode_PostSeamlessTravel(harmony);

        // 无论补丁是否成功注册，都启动后备定时器确保轮询
        A11y.SceneDetector.StartFallbackTimer();
    }

    // ── 1. BGW_GameInstance_B1.InitGameInstObj ──
    // 游戏实例初始化 → 最先可获取 GameInstance 上下文，用于事件订阅
    static void Patch_GameInstance_Init(Harmony harmony)
    {
        try
        {
            var type = AccessTools.TypeByName("b1.BGW_GameInstance_B1");
            if (type == null) { A11yLog.Warning("skip BGW_GameInstance_B1 — type not found"); return; }

            var method = AccessTools.Method(type, "InitGameInstObj");
            if (method == null) { A11yLog.Warning("skip BGW_GameInstance_B1.InitGameInstObj — method not found"); return; }

            var postfix = AccessTools.Method(typeof(SceneDetectorPatch), nameof(OnGameInstanceInit_Postfix));
            harmony.Patch(method, postfix: new HarmonyMethod(postfix));
            A11yLog.Info("[SceneDetectorPatch] ✅ BGW_GameInstance_B1.InitGameInstObj");
        }
        catch (Exception ex) { A11yLog.Warning($"Patch GameInstance_Init failed: {ex.Message}"); }
    }

    // ── 2. BGG_GameModeB1.ReceiveTick ──
    // 全局 Tick 驱动 → 用于轮询 FSM 状态和 UI 页面变化
    static void Patch_GameMode_ReceiveTick(Harmony harmony)
    {
        try
        {
            // 注意命名空间是 b1.GameMode 而非 b1
            var type = AccessTools.TypeByName("b1.GameMode.BGG_GameModeB1");
            if (type == null) { A11yLog.Warning("skip BGG_GameModeB1 — type not found"); return; }

            var method = AccessTools.Method(type, "ReceiveTick_Implementation", new[] { typeof(float) });
            if (method == null) { A11yLog.Warning("skip BGG_GameModeB1.ReceiveTick — method not found"); return; }

            var postfix = AccessTools.Method(typeof(SceneDetectorPatch), nameof(OnTick_Postfix));
            harmony.Patch(method, postfix: new HarmonyMethod(postfix));
            A11yLog.Info("[SceneDetectorPatch] ✅ BGG_GameModeB1.ReceiveTick");
        }
        catch (Exception ex) { A11yLog.Warning($"Patch GameMode Tick failed: {ex.Message}"); }
    }

    // ── 3. BGGGameStateCS.OnTickDispatchEventCS_Implementation ──
    // GameState Tick 补充（GameMode 可能在某些场景不 tick）
    static void Patch_GameState_Tick(Harmony harmony)
    {
        try
        {
            var type = AccessTools.TypeByName("b1.BGGGameStateCS");
            if (type == null) { A11yLog.Warning("skip BGGGameStateCS — type not found"); return; }

            var method = AccessTools.Method(type, "OnTickDispatchEventCS_Implementation", new[] { typeof(float) });
            if (method == null) { A11yLog.Warning("skip BGGGameStateCS.OnTickDispatchEventCS — method not found"); return; }

            // 使用独立 handler 避免参数名冲突（原方法参数为 DeltaTime）
            var postfix = AccessTools.Method(typeof(SceneDetectorPatch), nameof(OnTick_Postfix));
            harmony.Patch(method, postfix: new HarmonyMethod(postfix));
            A11yLog.Info("[SceneDetectorPatch] ✅ BGGGameStateCS.OnTickDispatchEventCS");
        }
        catch (Exception ex) { A11yLog.Warning($"Patch GameState Tick failed: {ex.Message}"); }
    }

    // ── 4. ABGWGameMode.HandleLeavingMapCS_Implementation ──
    // 离开地图 → 捕获场景切换起点
    static void Patch_GameMode_HandleLeavingMap(Harmony harmony)
    {
        try
        {
            var type = AccessTools.TypeByName("b1.ABGWGameMode");
            if (type == null) { A11yLog.Warning("skip ABGWGameMode — type not found"); return; }

            var method = AccessTools.Method(type, "HandleLeavingMapCS_Implementation");
            if (method == null) { A11yLog.Warning("skip ABGWGameMode.HandleLeavingMapCS — method not found"); return; }

            var prefix = AccessTools.Method(typeof(SceneDetectorPatch), nameof(OnHandleLeavingMap_Prefix));
            harmony.Patch(method, prefix: new HarmonyMethod(prefix));
            A11yLog.Info("[SceneDetectorPatch] ✅ ABGWGameMode.HandleLeavingMapCS");
        }
        catch (Exception ex) { A11yLog.Warning($"Patch HandleLeavingMap failed: {ex.Message}"); }
    }

    // ── 5. ABGWGameMode.PostSeamlessTravelCS_Implementation ──
    // 无缝过渡完成 → 捕获场景切换终点
    static void Patch_GameMode_PostSeamlessTravel(Harmony harmony)
    {
        try
        {
            var type = AccessTools.TypeByName("b1.ABGWGameMode");
            if (type == null) return; // 已在上一个补丁检查过

            var method = AccessTools.Method(type, "PostSeamlessTravelCS_Implementation");
            if (method == null) { A11yLog.Warning("skip ABGWGameMode.PostSeamlessTravelCS — method not found"); return; }

            var postfix = AccessTools.Method(typeof(SceneDetectorPatch), nameof(OnPostSeamlessTravel_Postfix));
            harmony.Patch(method, postfix: new HarmonyMethod(postfix));
            A11yLog.Info("[SceneDetectorPatch] ✅ ABGWGameMode.PostSeamlessTravelCS");
        }
        catch (Exception ex) { A11yLog.Warning($"Patch PostSeamlessTravel failed: {ex.Message}"); }
    }

    // ── Patch handler 方法（由 Harmony 调用） ──

    public static void OnGameInstanceInit_Postfix(object __instance)
    {
        A11yLog.Info("[SceneDetector] === GameInstance Init ===");
        if (__instance is UObject uobj)
        {
            A11y.SceneDetector.OnGameInstanceInit_Postfix(uobj);
        }
    }

    /// <summary>无参数的通用 Tick postfix，不绑定原方法参数名。</summary>
    public static void OnTick_Postfix()
    {
        A11y.SceneDetector.OnTickUpdate();
    }

    public static void OnHandleLeavingMap_Prefix()
    {
        A11y.SceneDetector.OnHandleLeavingMap_Prefix();
    }

    public static void OnPostSeamlessTravel_Postfix()
    {
        A11yLog.Info("[SceneDetector] ✅ PostSeamlessTravel (native patch)");
    }
}
