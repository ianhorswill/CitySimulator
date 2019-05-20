using System;
using System.Collections.Generic;
using Codes.Institution;

public class ActionSimulator : SimulatorComponent
{
    Person RandomlyChoosePeople()
    {
        return PersonTown.Singleton.aliveResidents.RandomElement();
    }

    public override void Step()
    {
        for (int i = 0; i < 10; i++)
        {
            ActionType randAction = ActionLibrary.ChanceFilteredPriorityBasedSelection(i);
            Action a = randAction.AttemptAction();
            if (a != null)
            {
                randAction.Execute(a);
            }
        }

        if (PersonTown.Singleton.aliveResidents.Count == 0) // Town is dead
            return;
    }
}