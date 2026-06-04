using WkAccess.A11y;

namespace WkAccess.A11yMod;

/// <summary>
/// 订阅 UIFocusTracker.OnFocusEnter 事件，将 UI 焦点变化转为语音输出。
/// 遵循架构分层：Patch 委托给 UIFocusTracker，A11yMod 负责朗读。
/// </summary>
public static class UIFocusSpeaker
{
    public static void Init()
    {
        UIFocusTracker.OnFocusEnter += OnFocusEnter;
    }

    static void OnFocusEnter(int gsid, string className, string? text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            A11yLog.Debug($"[UI.Focus] 聚焦 {className}#{gsid}: {text}");
            A11yTolk.Speak(text!, interrupt: true);
        }
        else
        {
            A11yLog.Debug($"[UI.Focus] 聚焦 {className}#{gsid}");
        }
    }
}
