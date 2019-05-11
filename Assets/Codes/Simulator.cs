using System;

/// <summary>
/// Shared state for the simulator such as time
/// </summary>
public static class Simulator
{
    static Simulator()
    {
        CurrentTime = WorldStart;
    }

    public static DateTime WorldStart = new DateTime(1850, 1, 1, 10, 0, 0);
    public static TimeSpan TimeIncrement = new TimeSpan(0, 12, 0, 0);

    private static DateTime _currenTime;
    public static DateTime CurrentTime
    {
        get => _currenTime;
        set
        {
            _currenTime = value;
            CurrentTimeString = _currenTime.ToString(DateTimeFormat);
        }
    }
    public static string CurrentTimeString { get; private set; }
    public static string DateTimeFormat = "yyyy-MM-dd tt";

    public static void AdvanceTime()
    {
        CurrentTime = CurrentTime.Add(TimeIncrement);
    }
}
