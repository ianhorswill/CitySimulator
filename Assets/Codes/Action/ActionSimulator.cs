using System;
using System.Collections.Generic;
using Codes.Institution;

public class ActionSimulator : SimulatorComponent
{
    //public List<Action> action_history = new List<Action>();

    Person RandomlyChoosePeople()
    {
        return PersonTown.Singleton.aliveResidents.RandomElement();
    }

    public override void Step()
    {
        for (int i = 0; i < 10; i++)
        {
            ActionType randAction = ActionLibrary.ChanceFilteredPriorityBasedSelection(i);
            Action a;
            if (randAction != null)
                a = randAction.Instantiate();
            else
                continue;
            
            //Debug.Log(a);
            if (a != null)
            {
                randAction.Execute(a);
            }
            //Logger.Log(a.ToString());
        }

        if (PersonTown.Singleton.aliveResidents.Count == 0) // Town is dead
            return;
    }
}