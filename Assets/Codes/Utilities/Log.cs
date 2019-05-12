using System.IO;
using System.Text;

/// <summary>
/// Logs messages from the simulator to a spreadsheet file.
/// Also makes recent log messages available to GUI.
/// </summary>
public static class Logger
{
    /// <summary>
    /// Initialize logging system
    /// </summary>
    static Logger()
    {
        // Initialize the log file
        LogFile = File.CreateText("log.tsv");
        LogFile.WriteLine("Time\tSubsystem\tInformation");

        // Arrange for the log file to be closed upon quitting.
        UnityEngine.Application.quitting +=
            () => LogFile.Close();
    }

    #region Static fields
    /// <summary>
    /// Text file to which we are writing the log messages.
    /// </summary>
    static readonly TextWriter LogFile;
    /// <summary>
    /// Size of the recent message buffer.
    /// </summary>
    private const int RecentLength = 100;
    /// <summary>
    /// Cache of recently generated log messages, stored as a circular buffer.
    /// </summary>
    static readonly string[] RecentLogMessages = new string[RecentLength];
    /// <summary>
    /// Position to store next log message at in RecentLogMessages
    /// </summary>
    private static int nextRecent;
    #endregion

    /// <summary>
    /// Adds new line to log
    /// </summary>
    /// <param name="subsystem">Component issuing the message</param>
    /// <param name="arguments">Strings to include in the message</param>
    public static void Log(string subsystem, params string[] arguments)
    {
        LogFile.Write(Simulator.CurrentTimeString);
        LogFile.Write('\t');
        LogFile.Write(subsystem);
        LogFile.Write('\t');
        foreach (var a in arguments)
        {
            LogFile.Write(a);
            LogFile.Write('\t');
        }
        LogFile.Write('\n');
        RecentLogMessages[(nextRecent++)%RecentLogMessages.Length] = FormatLogString(subsystem, arguments);
    }

    /// <summary>
    /// Adds new line to log
    /// </summary>
    /// <param name="component">Component issuing the message</param>
    /// <param name="arguments">Strings to include in the message</param>
    public static void Log(SimulatorComponent component, params string[] arguments)
    {
        Log(component.GetType().Name, arguments);
    }

    /// <summary>
    /// Returns the time'th most recent log message.
    /// Time=0 returns most recent, time=1 returns second-most recent, etc.
    /// </summary>
    public static string Recent(int time)
    {
        return RecentLogMessages[((nextRecent - 1) - time) % RecentLogMessages.Length] ?? "";
    }


    #region Recents buffer maintenance
    /// <summary>
    /// Buffer for building log strings.  Used only by FormatLogString.
    /// This is more space efficient than calling Format repeatedly.
    /// </summary>
    private static readonly StringBuilder LogBuffer = new StringBuilder();

    /// <summary>
    /// Create a single string from a log message that can be displayed in the log GUI.
    /// </summary>
    private static string FormatLogString(string subsystem, string[] arguments)
    {
        LogBuffer.Length = 0;
        LogBuffer.Append($"{Simulator.CurrentTimeString} {subsystem}: ");
        bool first = true;
        foreach (var a in arguments)
        {
            if (first)
                first = false;
            else
                LogBuffer.Append(", ");
            LogBuffer.Append(a);
        }

        return LogBuffer.ToString();
    }
    #endregion

    /// <summary>
    /// WRite any pending log messages out to disk
    /// </summary>
    public static void FlushLog()
    {
        LogFile.Flush();
    }
}
