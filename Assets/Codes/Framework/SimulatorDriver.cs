using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SimulatorDriver : MonoBehaviour
{
    public GUIStyle TextStyle;

    internal void Update()
    {
        if (Simulator.IsRunning)
            Simulator.StepIfTimeRemaining();
        Simulator.Visualize();
    }

    internal void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200,100, 1000, 2000));
        GUILayout.Label("Welcome to the town of <insert name here>", TextStyle);
        GUILayout.Label($"Population: {PersonTown.Singleton.aliveResidents.Count}", TextStyle);
        GUILayout.Label(Simulator.CurrentTimeString, TextStyle);

        if (GUILayout.Button(Simulator.IsRunning ? "Stop" : "Start", GUILayout.Width(100)))
            Simulator.IsRunning = !Simulator.IsRunning;

        foreach (var c in Simulator.Components)
            foreach (var w in c.WatchPoints)
                GUILayout.Label(RunWatchpoint(w));

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
