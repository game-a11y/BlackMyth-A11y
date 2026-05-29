using CSharpModBase.Input;

namespace WkAccess.A11yMod;

/// <summary>
/// 快捷键集中注册。所有回调定义为私有方法，不对外暴露。
/// Init() 中调用 RegisterAll() 即可完成全部注册。
/// </summary>
internal static class KeyBindings
{
    public static void RegisterAll()
    {
        // Ctrl + Enter → 打印玩家信息
        Utils.RegisterKeyBind(ModifierKeys.Control, Key.ENTER, DebugCommands.PrintPlayerInfo);

        // Ctrl + D1 → 打印当前场景/UI 状态
        Utils.RegisterKeyBind(ModifierKeys.Control, Key.D1, SceneMonitor.PrintSummary);
    }
}
