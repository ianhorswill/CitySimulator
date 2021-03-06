﻿using System;
using System.Collections.Generic;

/// <summary>
/// Shared state for the simulator such as time
/// </summary>
public static class Simulator
{
    static Simulator()
    {
        CurrentTime = WorldStart;
        AddComponent(new Space());
        AddComponent(new PersonTown());
        AddComponent(new ActionSimulator());
//        AddComponent(new InstitutionManager(Space.Singleton));
    }

    private static void AddComponent(SimulatorComponent simulatorComponent)
    {
        Components.Add(simulatorComponent);
    }

    private static bool isInitialized;
    public static void Initialize()
    {
        if (isInitialized)
            return;

        foreach (var c in Components)
            c.Initialize();

        isInitialized = true;
    }

    /// <summary>
    /// True if simulator is not paused.
    /// </summary>
    public static bool IsRunning;

    /// <summary>
    /// List of components to tick for the simulation
    /// </summary>
    public static readonly List<SimulatorComponent> Components = new List<SimulatorComponent>();

    #region Time
    
    /// <summary>
    /// Current date and time within the simulator
    /// </summary>
    public static DateTime CurrentTime
    {
        get => _currenTime;
        private set
        {
            _currenTime = value;
            CurrentTimeString = _currenTime.ToString(DateTimeFormat);
        }
    }

    /// <summary>
    /// String representation of the current time and date within the simulation
    /// </summary>
    public static string CurrentTimeString { get; private set; }

    /// <summary>
    /// Time at which the simulation should start
    /// </summary>
    public static DateTime WorldStart = new DateTime(1850, 1, 1, 10, 0, 0);

    /// <summary>
    /// Time at which the simulation should stop
    /// </summary>
    public static DateTime WorldEnd = new DateTime(2019, 1, 1, 10, 0, 0);

    /// <summary>
    /// Size of timestep between ticks
    /// </summary>
    public static TimeSpan TimeIncrement = new TimeSpan(0, 12, 0, 0);

    private static DateTime _currenTime;

    public static string DateTimeFormat = "yyyy-MM-dd tt";

    private static void AdvanceTime()
    {
        CurrentTime = CurrentTime.Add(TimeIncrement);
    }
    #endregion

    #region Stepping
    /// <summary>
    /// Run the simulator for one update cycle
    /// </summary>
    public static void Step()
    {
        SimulatorComponent current = null;
        AdvanceTime();
        try
        {
            foreach (var c in Components)
            {
                current = c;
                c.Step();
            }
        }
        catch (Exception e)
        {
            IsRunning = false;
            Logger.Log(current,"Exception", e.GetType().Name, e.Message);
            Logger.FlushLog();   // Preserve log in case of crash
            throw;
        }
    }

    public static void StepIfTimeRemaining()
    {
        if (IsRunning)
        {
            Step();

            if (CurrentTime >= WorldEnd)
                IsRunning = false;
            string trigger = null;
            SimulatorComponent currentComponent = null;
            try
            {
                foreach (var c in Components)
                {
                    currentComponent = c;
                    foreach (var t in c.StopTriggers)
                {
                    trigger = t.Key;
                    if (t.Value())
                    {
                        Logger.Log(c, "Stopped by trigger", trigger);
                        IsRunning = false;
                        return;
                    }
                }}
            }
            catch (Exception e)
            {
                IsRunning = false;
                Logger.Log(currentComponent, $"Trigger {trigger} threw exception", e.Message);
            }
        }
    }

    /// <summary>
    /// Keep stepping until simulation pauses
    /// </summary>
    public static void StepUntilPaused()
    {
        while (IsRunning)
            StepIfTimeRemaining();
    }
    #endregion

    /// <summary>
    /// Call visualizers for all components
    /// </summary>
    public static void Visualize()
    {
        foreach (var c in Components)
            c.Visualize();
    }
}
