using System;

/// <summary>
/// Shared state for the simulator such as time
/// </summary>
public static class Simulator
{
    public static DateTime WorldStart = new DateTime(1850, 1, 1, 10, 0, 0);
    public static TimeSpan DayDuration = new TimeSpan(0, 12, 0, 0);
    public static DateTime CurrentTime = WorldStart;

    public static void AdvanceTime()
    {
        CurrentTime = CurrentTime.Add(DayDuration);
    }
}
