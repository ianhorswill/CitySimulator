using System;
using System.Collections.Generic;
using Codes.Institution;

public class ActionSimulator : SimulatorComponent
{
    public static List<Action> action_history = new List<Action>();

    static ActionSimulator()
    {
        action_history = new List<Action>();
    }

    Person RandomlyChoosePeople()
    {
        return PersonTown.Singleton.aliveResidents.RandomElement();
    }

    public override void Step()
    {
        // Number of actions per step
        for (int i = 0; i < 3; i++)
        {
            ActionType randAction = ActionLibrary.ChanceFilteredRandomSelection();
            Action a;
            if (randAction != null)
                a = randAction.Instantiate();
            else
                continue;

            if (a != null) { randAction.Execute(a); }
        }

        if (PersonTown.Singleton.aliveResidents.Count == 0) // Town is dead
            return;
    }
}