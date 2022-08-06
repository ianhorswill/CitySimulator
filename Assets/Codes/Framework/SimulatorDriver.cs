using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SimulatorDriver : MonoBehaviour
{
    public GUIStyle TextStyle;
    private bool isThreaded;
    private Thread simulatorThread;
    public bool Visible { get; set; }

    internal void Start()
    {
        Visible = true;
        Simulator.Initialize();
    }

    internal void Update()
    {
        if (Simulator.IsRunning)
        {
            if (isThreaded)
                KickSimulatorThread();
            else
                Simulator.StepIfTimeRemaining();
        }
        Simulator.Visualize();
    }

    void KickSimulatorThread()
    {
        if (simulatorThread == null || !simulatorThread.IsAlive)
        {
            simulatorThread = new Thread(Simulator.StepUntilPaused);
            simulatorThread.Start();
        }
    }

    internal void OnGUI()
    {
        if (!Visible)
            return;
        GUILayout.BeginArea(new Rect(20,10, 1000, 2000));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(Simulator.IsRunning ? "Pause" : "Start", GUILayout.Width(100)))
            Simulator.IsRunning = !Simulator.IsRunning;
        if (GUILayout.Button(isThreaded ? "Turbo Off" : "Turbo On", GUILayout.Width(100)))
        {
            isThreaded = !isThreaded;
            if (!isThreaded)
                Simulator.IsRunning = false;
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("Welcome to the town of <insert name here>", TextStyle);
        GUILayout.Label(Simulator.CurrentTimeString, TextStyle);

        foreach (var c in Simulator.Components)
            foreach (var w in c.WatchPoints)
                GUILayout.Label(RunWatchpoint(w),TextStyle);

        for (var i = 0; i < 40; i++)
            GUILayout.Label(Logger.Recent(i),TextStyle);
        GUILayout.EndArea();
    }

    private string RunWatchpoint(KeyValuePair<string, Func<string>> watch)
    {
        try
        {
            return watch.Value();
        }
        catch (Exception e)
        {
            return $"Watchpoint {watch.Key} threw exception: {e.Message}";
        }
    }
}
