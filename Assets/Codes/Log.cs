using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Logger
{
    static TextWriter logFile = File.CreateText("log.tsv");
    public static void Log(string subsystem, params string[] arguments)
    {
        logFile.Write(subsystem);
        logFile.Write('\t');
        foreach (var a in arguments)
        {
            logFile.Write(a);
            logFile.Write('\t');
        }
        logFile.Write('\n');
    }
}
