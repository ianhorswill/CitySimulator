using System;
using Boo.Lang;

/// <summary>
/// Shared state for the simulator such as time
/// </summary>
public static class Simulator
{
    public static readonly List<SimulatorComponent> Components = new List<SimulatorComponent>();
    static Simulator()
    {
        CurrentTime = WorldStart;
    }

    public static DateTime WorldStart = new DateTime(1850, 1, 1, 10, 0, 0);
    public static DateTime WorldEnd = new DateTime(2019, 1, 1, 10, 0, 0);

    public static TimeSpan TimeIncrement = new TimeSpan(0, 12, 0, 0);

    private static DateTime _currenTime;
    public static DateTime CurrentTime
    {
        get => _currenTime;
        private set
        {
            _currenTime = value;
            CurrentTimeString = _currenTime.ToString(DateTimeFormat);
        }
    }
    public static string CurrentTimeString { get; private set; }
    public static string DateTimeFormat = "yyyy-MM-dd tt";

    /// <summary>
    /// Run the simulator for one update cycle
    /// </summary>
    public static void Step()
    {
        AdvanceTime();
        foreach (var c in Components)
            c.Step();
    }

    private static void AdvanceTime()
    {
        CurrentTime = CurrentTime.Add(TimeIncrement);
    }

    public static void StepIfTimeRemaining()
    {
        if (CurrentTime < WorldEnd)
            Step();
    }
}
