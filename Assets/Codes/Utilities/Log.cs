using System.IO;
using System.Net.Mime;

public static class Logger
{
    static TextWriter logFile;

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
    }

    public static void FlushLog()
    {
        logFile.Flush();
    }
}
