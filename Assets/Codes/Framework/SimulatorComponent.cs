using System;
using System.Collections.Generic;

/// <summary>
/// A component of the simulator.  Basically an object that can be ticked.
/// </summary>
public abstract class SimulatorComponent
{
    /// <summary>
    /// Called once per time-step of the simulation
    /// </summary>
    public abstract void Step();

    #region Debugging support
    /// <summary>
    /// List of functions to call to generate strings to display on the screen
    /// </summary>
    internal readonly List<Func<string>> WatchPoints = new List<Func<string>>();

    /// <summary>
    /// Display the value of function on every frame.
    /// </summary>
    /// <param name="function">Function to call to generate a string to display</param>
    protected void Watch(Func<string> function)
    {
        WatchPoints.Add(function);
    }

    /// <summary>
    /// List of functions to call to test whether to stop the simulation
    /// </summary>
    internal readonly List<Func<bool>> StopTriggers = new List<Func<bool>>();

    /// <summary>
    /// Declares that the simulation should pause if/when the function returns true.
    /// </summary>
    /// <param name="trigger"></param>
    protected void StopWhen(Func<bool> trigger)
    {
        StopTriggers.Add(trigger);
    }

    /// <summary>
    /// Suppress log messages from this component when true.
    /// </summary>
    public bool Mute;

    /// <summary>
    /// Adds new line to log
    /// </summary>
    /// <param name="arguments">Strings to include in the message</param>
    protected void Log(params string[] arguments)
    {
        if (!Mute)
            Logger.Log(this, arguments);
    }
    #endregion
}