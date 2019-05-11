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
        GUILayout.BeginArea(new Rect(300,100, 500, 1000));
        GUILayout.Label("Welcome to the town of <insert name here>", TextStyle);
        GUILayout.Label($"Population: {PersonTown.Singleton.aliveResidents.Count}", TextStyle);
        GUILayout.Label(Simulator.CurrentTimeString);
        if (GUILayout.Button(IsRunning ? "Stop" : "Start", TextStyle))
            IsRunning = !IsRunning;
        GUILayout.EndArea();
    }
}
