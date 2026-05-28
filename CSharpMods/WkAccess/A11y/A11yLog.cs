using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace WkAccess.A11y;

/// <summary>
/// 日志系统：控制台输出 + 文件日志。
///
/// 文件日志默认写入 mod 所在文件夹下的 WkAccess.log，
/// 支持按级别过滤，启动时截断过大的日志文件 (阈值 5MB)。
///
/// 使用方式：
///   A11yLog.Init();                     // 控制台 + 文件日志初始化
///   A11yLog.Info(...) / Warning(...) / Error(...)
///   A11yLog.Deinit();                   // Mod 卸载时关闭文件
/// </summary>
public class A11yLog
{
    const string LOG_PREFIX = "A11y";
    const long MAX_FILE_SIZE = 5 * 1024 * 1024;   // 5 MB，超过则截断
    const long KEEP_FILE_SIZE = 1 * 1024 * 1024;   // 保留 1 MB

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleOutputCP(uint wCodePageID);
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleCP(uint wCodePageID);

    // ── 文件日志状态 ──
    const string DefaultModDir = "\0";           // 哨兵值：表示"使用默认 mod 目录"
    static readonly object _fileLock = new();
    static StreamWriter? _fileWriter;
    static LogType _fileMinLevel = LogType.Debug;

    #region Public API 公共API

    /// <summary>
    /// 初始化日志系统：设置控制台 UTF-8 编码，启动文件日志。
    /// 文件默认写入 mod 所在文件夹下的 WkAccess.log，超过 5MB 自动截断保留末尾 1MB。
    /// </summary>
    /// <param name="fileLogDir">日志目录，默认使用 mod DLL 所在目录。设为空字符串禁用文件日志。</param>
    /// <param name="fileMinLevel">文件日志最低级别，默认 Debug（全部写入）</param>
    public static void Init(string? fileLogDir = DefaultModDir, LogType fileMinLevel = LogType.Debug)
    {
        SetConsoleCP(65001);
        SetConsoleOutputCP(65001);
        Console.OutputEncoding = Encoding.UTF8;

        if (string.IsNullOrEmpty(fileLogDir)) return;
        if (fileLogDir == DefaultModDir) fileLogDir = GetDefaultLogDir();

        _fileMinLevel = fileMinLevel;
        var filePath = Path.Combine(fileLogDir, "WkAccess.log");
        Directory.CreateDirectory(fileLogDir);
        ManageFileSize(filePath);

        Console.WriteLine($"[A11y] Log file: {filePath}");

        lock (_fileLock)
        {
            _fileWriter = new StreamWriter(filePath, append: true, Encoding.UTF8)
            {
                AutoFlush = true
            };
            _fileWriter.WriteLine($"--- Session start @ {DateTime.Now:yyyy-MM-dd HH:mm:ss} ---");
        }
    }

    /// <summary>关闭文件日志（Mod 卸载时调用）。</summary>
    public static void Deinit()
    {
        lock (_fileLock)
        {
            if (_fileWriter != null)
            {
                _fileWriter.WriteLine($"--- Session end @ {DateTime.Now:yyyy-MM-dd HH:mm:ss} ---");
                _fileWriter.Close();
                _fileWriter = null;
            }
        }
    }

    public static void Info(string message) => Write(LogType.Info, message);

    public static void Warning(string message) => Write(LogType.Warning, message);

    public static void Error(string message) => Write(LogType.Error, message);

    public static void Exception(string message, Exception exception, object? obj = null)
        => WriteException(message, exception);

    public static void Exception(string message, Exception exception)
        => WriteException(message, exception);

    #endregion

    #region Write 核心写入

    static void Write(LogType type, string message)
    {
        if (string.IsNullOrEmpty(message)) return;

        var line = FormatLine(type, message, out var color);
        WriteToConsole(line, color);
        WriteToFile(type, line);
    }

    static void WriteException(string message, Exception exception)
    {
        var line = FormatLine(LogType.Exception, message, out var color);
        WriteToConsole(line, color);
        WriteToFile(LogType.Exception, line);

        if (!string.IsNullOrEmpty(exception.Message))
        {
            var exLine = FormatLine(LogType.Exception, exception.Message, out _);
            WriteToConsole(exLine, color);
            WriteToFile(LogType.Exception, exLine);
        }
        if (!string.IsNullOrEmpty(exception.StackTrace))
        {
            WriteToConsole(exception.StackTrace, ConsoleColor.Gray);
            WriteToFile(LogType.Exception, exception.StackTrace);
        }
        WriteToConsole("", color);
    }

    #endregion

    #region 格式化

    public enum LogType
    {
        Off,
        Exception,
        Error,
        Warning,
        Info,
        Debug
    }

    static string FormatLine(LogType type, string text, out ConsoleColor color)
    {
        var ts = DateTime.Now.ToString("HH:mm:ss.fff");
        var level = type switch
        {
            LogType.Error => "E",
            LogType.Exception => "X",
            LogType.Warning => "W",
            LogType.Info => "I",
            LogType.Debug => "D",
            _ => "?",
        };
        color = type switch
        {
            LogType.Error or LogType.Exception => ConsoleColor.Red,
            LogType.Warning => ConsoleColor.Yellow,
            LogType.Info => ConsoleColor.White,
            LogType.Debug => ConsoleColor.Magenta,
            _ => ConsoleColor.Gray,
        };
        return $"{ts} [{level}] [{LOG_PREFIX}] {text}";
    }

    #endregion

    #region 控制台输出

    static void WriteToConsole(string message, ConsoleColor color)
    {
        if (string.IsNullOrEmpty(message)) return;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    #endregion

    #region 文件输出

    static void WriteToFile(LogType type, string line)
    {
        if (_fileWriter == null) return;
        if (type > _fileMinLevel && type != LogType.Exception) return;

        lock (_fileLock)
        {
            _fileWriter?.WriteLine(line);
        }
    }

    /// <summary>
    /// 文件超过 MAX_FILE_SIZE 时，保留末尾 KEEP_FILE_SIZE 字节。
    /// 用简单读取-截断-重写的方式，不依赖第三方库。
    /// </summary>
    static void ManageFileSize(string filePath)
    {
        if (!File.Exists(filePath)) return;

        try
        {
            var fi = new FileInfo(filePath);
            if (fi.Length <= MAX_FILE_SIZE) return;

            // 保留末尾 KEEP_FILE_SIZE 字节
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var keep = Math.Min(KEEP_FILE_SIZE, fs.Length);
            fs.Seek(-keep, SeekOrigin.End);
            var buf = new byte[keep];
            _ = fs.Read(buf, 0, buf.Length);

            // 跳过第一行（可能被截断），写入新文件
            var text = Encoding.UTF8.GetString(buf);
            var firstNewline = text.IndexOf('\n');
            if (firstNewline > 0) text = text.Substring(firstNewline + 1);

            File.WriteAllText(filePath, $"[日志截断 — 保留最近条目]\n{text}", Encoding.UTF8);
        }
        catch
        {
            // 截断失败不影响主流程
        }
    }

    /// <summary>获取默认日志目录：mod DLL 所在目录（Release）/ CSharpLoader/Mods/（Develop）。</summary>
    static string GetDefaultLogDir()
    {
        var loc = typeof(A11yLog).Assembly.Location;
        if (!string.IsNullOrEmpty(loc))
            return Path.GetDirectoryName(loc) ?? ".";
        // Develop mode: assembly in-memory, construct path from mod directory
        var baseDir = AppDomain.CurrentDomain.BaseDirectory ?? ".";
        var modsDir = Path.Combine(baseDir, Common.ModDir);
        if (Directory.Exists(modsDir))
        {
            var name = typeof(A11yLog).Assembly.GetName().Name ?? "";
            // Strip timestamp suffix added by CSharpManager in Develop mode
            foreach (var dir in Directory.GetDirectories(modsDir))
            {
                var dirName = Path.GetFileName(dir);
                if (name.StartsWith(dirName, StringComparison.OrdinalIgnoreCase))
                    return dir;
            }
        }
        return baseDir;
    }

    #endregion
}
