using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using BarrelControlSystem.Backend.Models.Enums;

namespace BarrelControlSystem.Backend.Handlers;

public static class LoggingHandler
{
    private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
    private static readonly BlockingCollection<string> LogQueue = new();

    static LoggingHandler()
    {
        try
        {
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to create log directory: {ex.Message}");
        }

        // Start background worker for logging
        Task.Run(ProcessLogs);
    }

    private static void ProcessLogs()
    {
        foreach (var logEntry in LogQueue.GetConsumingEnumerable())
        {
            try
            {
                string fileName = $"{DateTime.Now:yyyy-MM-dd}.log";
                string filePath = Path.Combine(LogDirectory, fileName);
                File.AppendAllText(filePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to write to log file: {ex.Message}");
            }
        }
    }

    public static void Log(string message, LogLevel level = LogLevel.Info, [CallerFilePath] string callerPath = "")
    {
        string callerName = Path.GetFileNameWithoutExtension(callerPath);
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logEntry = $"[{timestamp}] [{level.ToString().ToUpper()}] [{callerName}] {message}";

        // Write to Console immediately
        Console.WriteLine(logEntry);

        // Queue for file writing
        LogQueue.Add(logEntry);
    }

    public static void LogInfo(string message, [CallerFilePath] string callerPath = "") => Log(message, LogLevel.Info, callerPath);
    public static void LogWarning(string message, [CallerFilePath] string callerPath = "") => Log(message, LogLevel.Warning, callerPath);
    public static void LogError(string message, [CallerFilePath] string callerPath = "") => Log(message, LogLevel.Error, callerPath);
}