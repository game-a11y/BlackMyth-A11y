using System;
using System.Collections.Generic;
using System.Linq;

namespace WkAccess.B1;

/// <summary>
/// 游戏场景状态容器：持有当前场景、地图、UI 页面等只读状态，
/// 通过事件和查询 API 对外暴露。写操作通过 internal 方法由 SceneMonitor 驱动。
/// </summary>
public static class GameState
{
    public enum GameScene
    {
        Unknown,
        Startup,
        LogIn,
        MainMenu,
        Loading,
        InGame,
        PauseMenu,
        ShaderCompiling,
    }

    static GameScene _currentScene = GameScene.Unknown;
    static string _currentMapName = "";
    static int _currentLevelId = -1;
    static readonly HashSet<int> _visiblePages = new();

    /// <summary>UI 页面打开/关闭时触发。(pageId, isActive)</summary>
    public static event Action<int, bool>? OnUIPageChanged;

    // ── 公开只读属性 ──

    public static GameScene CurrentScene => _currentScene;
    public static string CurrentMapName => _currentMapName;
    public static int CurrentLevelId => _currentLevelId;
    public static bool IsInGame => _currentScene == GameScene.InGame;
    public static bool IsPageOpen(int pageId) => _visiblePages.Contains(pageId);

    // ── internal 写方法（由 SceneMonitor 调用） ──

    internal static void SetScene(GameScene newScene)
    {
        if (_currentScene == newScene) return;
        var old = _currentScene;
        _currentScene = newScene;
        A11y.A11yLog.Warning($"[SceneMonitor] 🔔 === 场景切换: {old} -> {newScene} ===");
        A11y.A11yLog.Info($"[SceneMonitor]     Map={_currentMapName}, LevelId={_currentLevelId}");
    }

    internal static void SetMapName(string mapName) => _currentMapName = mapName;

    internal static void SetCurrentLevel(int levelId) => _currentLevelId = levelId;

    internal static void SetPageActive(int pageId, bool active)
    {
        if (active)
            _visiblePages.Add(pageId);
        else
            _visiblePages.Remove(pageId);

        OnUIPageChanged?.Invoke(pageId, active);
    }

    /// <summary>轮询一致性清理用：移除页面但不触发 OnUIPageChanged 事件。</summary>
    internal static void RemovePageSilent(int pageId) => _visiblePages.Remove(pageId);

    internal static void Clear()
    {
        _currentScene = GameScene.Unknown;
        _currentMapName = "";
        _currentLevelId = -1;
        _visiblePages.Clear();
    }

    /// <summary>供 SceneMonitor.PrintSummary 使用，获取可见页面快照。</summary>
    internal static IReadOnlyCollection<int> VisiblePages => _visiblePages;
}
