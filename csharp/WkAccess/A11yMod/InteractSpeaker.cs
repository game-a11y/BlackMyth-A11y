using WkAccess.A11y;
using WkAccess.BM;

namespace WkAccess.A11yMod;

/// <summary>
/// 订阅 InteractMonitor.OnInteractTextChanged 事件，将可交互目标变化转为语音输出。
/// 遵循架构分层：BM 发布事件，A11yMod 负责朗读。
/// </summary>
public static class InteractSpeaker
{
    public static void Init()
    {
        InteractMonitor.OnInteractTextChanged += OnInteractTextChanged;
    }

    static void OnInteractTextChanged(string text)
    {
        A11yTolk.Speak(text, interrupt: true);
    }
}
