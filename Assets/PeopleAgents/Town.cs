using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface Settlement
{
    PersonAgent[] agents  {get; set;}
    //PersonAgent[] settlers {get; set;} - Make a component isSettler as a tag?
    //PersonAgent[] departed {get; set;} - Make a component hasDeparted as a tag?
    //PersonAgent[] deceased {get; set;} - Make a component hasDied as a tag?
    
    //Companies[] companies {get; set;} 
    varLocationStructure Location { get; set; }
    int foundingYear {get; set;}
    void death();
    void birth();
    void departed();
     
    
}

public class PersonTown : Settlement
{
    public Person[] Agents {get; set;}
    public static void death(){
        //Must Query all living inhabitants and select one to become deceased...?
        var livingPeople = from Agent in Agents
                           where Agent.getLivingStatus() == true
                           select Agent;

        //Now that have all living Agents, somehow select one to die?
        //Randomly?
        Random r = new Random();
        Person selectedToDie = livingPeople.ElementAt(r.Next(0, livingPeople.Count()));

        //Set flag of selectedAgent to non-living or deceased?
        //Is Agent a GameObject?
        selectedAgent.getComponent<livingStatus>().status = "Dead";

        // Life event does further processing?
    }

    public static void birth(){
        //Select Parent Randomly
        var livingPeople =  from Agent in Agents
                            where Agent.getLivingStatus() == true
                            select Agent;
        Random r = new Random();
        Person selectedParent = livingPeople.ElementAt(r.Next(0, livingPeople.Count()));
        //Idk who's the other parent
        Person otherParent = new Parent();
        Person newborn = selectedParent.createChild(selectedParent, otherParent);
        Agents.Add(newborn);
    }


}


