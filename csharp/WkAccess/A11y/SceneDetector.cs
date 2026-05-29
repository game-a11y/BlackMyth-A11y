using System.Linq;
using System.Threading;
using b1;
using B1UI;
using B1UI.GSUI;
using GSE.GSUI;
using UnrealEngine.Runtime;

namespace WkAccess.A11y;

/// <summary>
/// 场景检测器：挂钩场景加载/UI 页面切换，用于无障碍功能可行性测试。
/// 输出所有场景变化和页面切换到控制台日志。
///
/// 架构问题（暂不重构，待实现首个无障碍功能时一并处理）：
///   1. 职责过重 — 470 行混入事件订阅/轮询/状态管理/调试打印四项职责
///   2. 状态不对外暴露 — _currentScene 和 _visiblePages 是私有字段，
///      无障碍功能代码只能通过 OnUIPageChanged 事件间接感知，
///      无法主动查询"当前是否在土地庙"等具体状态
///   3. 重构方向 — 提取 GameState(只读查询+事件) 和 SceneMonitor(检测驱动) 两个类，
///      见 CLAUDE.md 架构分析章节
/// </summary>
public static partial class SceneDetector
{
    // ── 场景状态 ──
    public enum GameScene
    {
        Unknown,
        Startup,          // 游戏启动
        LogIn,            // 登录界面
        MainMenu,         // 主菜单
        Loading,          // 加载中
        InGame,           // 游戏中
        PauseMenu,        // 暂停菜单/功能页面
        ShaderCompiling,  // 着色器编译
    }

    static GameScene _currentScene = GameScene.Unknown;
    static string _currentMapName = "";
    static int _currentLevelId = -1;

    static BGW_EventCollection? _events;
    static BGW_GameLifeTimeMgr? _lifeTime;
    static bool _eventsSubscribed;
    static bool _gsgAvailable;

    /// <summary>UI 页面打开/关闭时触发。(pageId, isActive)</summary>
    public static event Action<int, bool>? OnUIPageChanged;

    /// <summary>当前游戏场景</summary>
    public static GameScene CurrentScene => _currentScene;
    /// <summary>是否在游戏中</summary>
    public static bool IsInGame => _currentScene == GameScene.InGame;
    /// <summary>指定 UI 页面是否可见</summary>
    public static bool IsPageOpen(int pageId) => _visiblePages.Contains(pageId);

    // 当前可见 UI 页面集合
    static readonly HashSet<int> _visiblePages = new();

    // ── 初始化（由补丁在适当时机调用） ──

    /// <summary>尝试从 GameInstance 上下文初始化事件订阅。</summary>
    public static void TryInit(UObject context)
    {
        if (_eventsSubscribed) return;
        try
        {
            _events = BGW_EventCollection.Get(context);
            _lifeTime = BGW_GameLifeTimeMgr.Get(context);

            if (_events != null)
            {
                SubscribeEvents();
                _eventsSubscribed = true;
                A11yLog.Info("[SceneDetector] ✅ BGW_EventCollection 事件订阅完成");
            }
        }
        catch (Exception ex)
        {
            A11yLog.Warning($"[SceneDetector] Init 暂不可用: {ex.Message}");
        }
    }

    /// <summary>尝试初始化 GSG（B1UI_GSE.Script 加载后调用）。</summary>
    public static void TryInitGSG()
    {
        if (_gsgAvailable) return;
        try
        {
            // 检查 GSG 是否可用（B1UI_GSE.Script 是否已加载）
            var ctx = GSG.Context;
            _gsgAvailable = true;
            A11yLog.Info("[SceneDetector] ✅ GSG (B1UI_GSE.Script) 可用");
        }
        catch
        {
            // GSG 尚未加载
        }
    }

    static void UpdateScene(GameScene newScene)
    {
        if (_currentScene == newScene) return;
        var old = _currentScene;
        _currentScene = newScene;
        A11yLog.Warning($"[SceneDetector] 🔔 === 场景切换: {old} -> {newScene} ===");
        A11yLog.Info($"[SceneDetector]     Map={_currentMapName}, LevelId={_currentLevelId}");

        // 检查是否需要标记 GSG 可用（主菜单或游戏中都说明 UI 系统已就绪）
        if (newScene is GameScene.MainMenu or GameScene.InGame)
        {
            TryInitGSG();
        }
    }

    /// <summary>清理资源（Mod 卸载时调用）。</summary>
    public static void Deinit()
    {
        _fallbackTimer?.Dispose();
        _fallbackTimer = null;
        _timerStarted = false;
        _eventsSubscribed = false;
        // 解绑所有事件处理器
        UnsubscribeEvents();
        _events = null;
        _lifeTime = null;
    }

    /// <summary>打印当前所有可见 UI 页面的摘要。</summary>
    public static void PrintCurrentUI()
    {
        A11yLog.Warning($"=== 当前场景: {_currentScene} ===");
        A11yLog.Info($"  Map: {_currentMapName}, LevelId: {_currentLevelId}");
        A11yLog.Info($"  Events: {(_eventsSubscribed ? "✅" : "❌")}, GSG: {(_gsgAvailable ? "✅" : "❌")}");
        A11yLog.Info($"  Visible Pages ({_visiblePages.Count}):");

        foreach (var pageId in _visiblePages.OrderBy(id => id))
        {
            A11yLog.Info($"    - ({pageId}) {((EnPageID)pageId)}");
        }

        // 打印 FSM 状态
        try
        {
            if (_lifeTime != null)
            {
                var curState = _lifeTime.GlobalFSMInstanceCurState;
                A11yLog.Info($"  FSM State: {curState}");
                A11yLog.Info($"  InTravelLevel: {_lifeTime.IsInTravelLevel()}");
                A11yLog.Info($"  IsMainMenu: {_lifeTime.IsInFSMState(SGI_Global.MainMenu)}");
                A11yLog.Info($"  IsInGame: {_lifeTime.IsInFSMState(SGI_Global.InBattleStandAlone)}");
            }
        }
        catch (Exception ex)
        {
            A11yLog.Warning($"  FSM query failed: {ex.Message}");
        }
    }
}
