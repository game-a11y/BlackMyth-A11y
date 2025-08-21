namespace WkAccess.A11y;

/// <summary>
/// 输出到所有可能的输出.
/// </summary>
public class A11yLog
{
    const string LOG_PREFIX = "A11y";

    #region Public API 公共API
    public static void Info(string message)
    {
        FormatConsoleMsg(LogType.Info, message);
    }

    public static void Warning(string message)
    {
        FormatConsoleMsg(LogType.Warning, message);
    }
    public static void Error(string message)
    {
        FormatConsoleMsg(LogType.Error, message);
    }

    public static void Exception(
        string message,
        Exception exception, Object? obj=null)
    {
        FormatConsoleMsg(LogType.Excption, message);
        FormatConsoleMsg(LogType.Excption, exception.Message, exception.StackTrace);
        FormatConsoleMsg(LogType.Excption, "===================");
    }
    #endregion

    #region A11yInternal A11y内部方法
    internal enum LogType
    {
        Off,
        Excption,
        Error,
        Warning,
        Info,
        Debug
    }

    internal static string GetLogTypeString(LogType type, bool simpleStyle=true)
    {
        if (simpleStyle)
        {
            return type switch
            {
                LogType.Off => "",
                LogType.Error => "E",
                LogType.Excption => "X",
                LogType.Warning => "W",
                LogType.Info => "I",
                LogType.Debug => "D",
                _ => "?", // Unknown
            };
        }

        return type switch
        {
            LogType.Off => "",
            LogType.Error => "ERROR ",
            LogType.Excption => "EXCEPT",
            LogType.Warning => "WARN  ",
            LogType.Info => "INFO  ",
            LogType.Debug => "DEBUG ",
            _ => "UNKNOW",
        };
    }

    internal static ConsoleColor GetLogTypeColor(LogType type)
    {
        return type switch
        {
            LogType.Error or LogType.Excption => ConsoleColor.Red,
            LogType.Warning => ConsoleColor.Yellow,
            LogType.Info => ConsoleColor.White,
            LogType.Debug => ConsoleColor.Magenta,
            _ => ConsoleColor.Gray,
        };
    }

    internal static void WriteToConsole(string message, ConsoleColor color = ConsoleColor.White)
    {
        if (string.IsNullOrEmpty(message)) message = string.Empty;

        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    internal static void FormatConsoleMsg(LogType type, string logString, string stackTrace="")
    {
        // 忽略特定格式的日志
        //if (IsIgnoreLogType(type, logString)) return;
        if (string.IsNullOrEmpty(logString)) logString = string.Empty;

        string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        string logTypeStr = A11yLog.GetLogTypeString(type);
        string message = $"{timestamp} [{logTypeStr}] [{LOG_PREFIX}] {logString}";
        
        // 根据日志类型设置控制台颜色并输出
        ConsoleColor color = A11yLog.GetLogTypeColor(type);
        WriteToConsole(message, color);
        
        // 如果是错误或异常，也输出堆栈跟踪
        if ((type == LogType.Error || type == LogType.Excption) && !string.IsNullOrEmpty(stackTrace))
        {
            WriteToConsole("StackTrace:", ConsoleColor.Gray);
            WriteToConsole(stackTrace, ConsoleColor.Gray);
            WriteToConsole("");
        }
    }
    #endregion
}
