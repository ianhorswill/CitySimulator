using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SimulatorDriver : MonoBehaviour
{
    public bool IsRunning;
    public GUIStyle TextStyle;

    internal void Update()
    {
        if (IsRunning)
            Simulator.TickIfTimeRemaining();
    }

    internal void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200,100, 500, 2000));
        GUILayout.Label("Welcome to the town of <insert name here>", TextStyle);
        GUILayout.Label($"Population: {PersonTown.Singleton.aliveResidents.Count}", TextStyle);
        GUILayout.Label(Simulator.CurrentTimeString, TextStyle);
        if (GUILayout.Button(IsRunning ? "Stop" : "Start", TextStyle))
            IsRunning = !IsRunning;
        for (var i = 0; i < 40; i++)
            GUILayout.Label(Logger.Recent(i),TextStyle);
        GUILayout.EndArea();
    }
}
