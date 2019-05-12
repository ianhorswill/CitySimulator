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
    internal readonly Dictionary<string, Func<string>> WatchPoints = new Dictionary<string, Func<string>>();

    /// <summary>
    /// Display the value of function on every frame.
    /// </summary>
    /// <param name="description">Description of the watch point</param>
    /// <param name="function">Function to call to generate a string to display</param>
    protected void Watch(string description, Func<string> function)
    {
        WatchPoints[description] = function;
    }

    /// <summary>
    /// List of functions to call to test whether to stop the simulation
    /// </summary>
    internal readonly Dictionary<string, Func<bool>> StopTriggers = new Dictionary<string, Func<bool>>();

    /// <summary>
    /// Declares that the simulation should pause if/when the function returns true.
    /// </summary>
    /// <param name="description">Description of the trigger</param>
    /// <param name="trigger"></param>
    protected void StopWhen(string description, Func<bool> trigger)
    {
        StopTriggers[description] = trigger;
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