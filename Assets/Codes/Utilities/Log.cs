using System.IO;
using System.Net.Mime;
using System.Text;
using TMPro;

public static class Logger
{
    static TextWriter logFile;
    private const int RecentLength = 100;
    static string[] recent = new string[RecentLength];
    private static int nextRecent;

    public static string Recent(int time)
    {
        return recent[((nextRecent - 1) - time) % recent.Length] ?? "";
    }

    static Logger()
    {
        // Initialize the log file
        logFile = File.CreateText("log.tsv");
        logFile.WriteLine("Time\tSubsystem\tInformation");

        // Arrange for the log file to be closed upon quitting.
        UnityEngine.Application.quitting +=
            () => logFile.Close();
    }

    public static void Log(string subsystem, params string[] arguments)
    {
        logFile.Write(Simulator.CurrentTimeString);
        logFile.Write('\t');
        logFile.Write(subsystem);
        logFile.Write('\t');
        foreach (var a in arguments)
        {
            logFile.Write(a);
            logFile.Write('\t');
        }
        logFile.Write('\n');
        recent[nextRecent++] = FormatLogString(subsystem, arguments);
    }

    private static StringBuilder logBuffer = new StringBuilder();
    private static string FormatLogString(string subsystem, string[] arguments)
    {
        logBuffer.Length = 0;
        logBuffer.Append($"{Simulator.CurrentTimeString} {subsystem}: ");
        bool first = true;
        foreach (var a in arguments)
        {
            if (first)
                first = false;
            else
                logBuffer.Append(", ");
            logBuffer.Append(a);
        }

        return logBuffer.ToString();
    }

    public static void FlushLog()
    {
        logFile.Flush();
    }
}
