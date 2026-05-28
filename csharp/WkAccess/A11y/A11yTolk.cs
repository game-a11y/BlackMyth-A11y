namespace WkAccess.A11y;

/// <summary>
/// Tolk 屏幕朗读占位类。当前实现将朗读文本输出到日志。
/// 后续可替换为对 C++ Tolk 模块的 P/Invoke 调用。
/// </summary>
public static class A11yTolk
{
    const string Prefix = "[TTS]";

    public static void Speak(string text, bool interrupt = false)
    {
        if (string.IsNullOrEmpty(text)) return;
        
        //                                               "INTERRUPT"
        string interruptText = interrupt ? "INTERRUPT" : "  QUEUE++";
        A11yLog.Warning($"Tolk.Speak({interruptText}): \"{text}\"");
    }

    public static void Silence() => A11yLog.Warning($"{Prefix} Tolk.Silence()");
    public static bool IsAvailable() => true;
}
