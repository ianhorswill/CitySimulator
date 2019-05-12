using System;
using Boo.Lang;

/// <summary>
/// Shared state for the simulator such as time
/// </summary>
public static class Simulator
{
    static Simulator()
    {
        CurrentTime = WorldStart;
    }

    /// <summary>
    /// True if simulator is not paused.
    /// </summary>
    public static bool IsRunning;

    /// <summary>
    /// List of components to tick for the simulation
    /// </summary>
    public static readonly List<SimulatorComponent> Components = new List<SimulatorComponent>();

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
    public static string DateTimeFormat = "yyyy-MM-dd tt";

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

    private static void AdvanceTime()
    {
        CurrentTime = CurrentTime.Add(TimeIncrement);
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
}
