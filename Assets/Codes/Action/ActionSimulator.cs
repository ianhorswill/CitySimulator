using System.Collections.Generic;

/// <summary>
/// Simulation thread link to unity, manages action execution.
/// </summary>
public class ActionSimulator : SimulatorComponent
{
    /// <summary>
    /// A list of all completed actions
    /// </summary>
    public static List<Action> action_history = new List<Action>();

    /// <summary>
    /// Initializes the ActionSimulator and the list of previous actions
    /// </summary>
    static ActionSimulator()
    {
        action_history = new List<Action>();
    }

    /// <summary>
    /// Randomly chooses people.
    /// </summary>
    /// <returns>The choosen person</returns>
    Person RandomlyChoosePeople()
    {
        return PersonTown.Singleton.aliveResidents.RandomElement();
    }

    /// <summary>
    /// Step this instance.
    /// </summary>
    public override void Step()
    {
        // Do a certain number of actions based loosely on the population,
        // the frequency of the reference action, the overall sum of action
        // frequencies, and the target number of iterations...
        // target_iterations and reference_action (as well as all action frequencies)
        // asre set in ActionLibrary
        float fib_factor = PersonTown.Singleton.aliveResidents.Count / 10.0f;
        for (int i = 0; i < (ActionLibrary.ActualIterations * fib_factor); i++)
        {
            ActionType randAction = ActionLibrary.FrequencyFilteredRandomSelection();
            if (randAction != null)
                randAction.InstantiateAndExecute();
            else
                continue;
        }

        if (PersonTown.Singleton.aliveResidents.Count == 0) // Town is dead
            return;
    }
}