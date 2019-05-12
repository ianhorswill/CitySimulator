using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PersonTown : SimulatorComponent
{
    public override void Initialize()
    {
        Watch("Display population", () => $"Population: {aliveResidents.Count}");
        StopWhen("Population died off", () =>
            aliveResidents.Count == 0 &&
            Simulator.CurrentTime > Simulator.WorldStart + new TimeSpan(30, 0, 0, 0));
    }
    //LINQ version: Just do one array/list, "Person[] people {get; set;}" or "var people = new List<Person>();" and perform queries on them

    //OOP version: Series of arrays that hold people pertaining to each property: original settlers, departed townsfolk, current alive residents, and deceased residents.
    [SerializeField] public List<Person> settlers = new List<Person>();
    public List<Person> departed = new List<Person>();
    [SerializeField] public List<Person> aliveResidents = new List<Person>();
    public List<Person> deceased = new List<Person>();

    private static int deathProbability = 5;
    private static int birthProbability = 100;

    //For David: Going to add another constructor to People that allows the creation of adults
    //     public Person (string name, List<Person> currSiblings, int age, Person sigOther, List<Person> children, Person[] parents, bool biologicalSex){

    /// <summary>
    /// So SimulationDriver can find this.
    /// </summary>
    public static PersonTown Singleton;

    //Constructor with settler input
    public PersonTown(List<Person> newSettlers)
    {
        settlers = newSettlers;
        aliveResidents = newSettlers;
        Singleton = this;
    }

    //Default constructor with no settler input / Uses Adam/Eve
    public PersonTown(){
        var initialSettlerTest = new List<Person>();
        Person p1 = new Person("Adam", null, 20, null, null, null, true);
        Person p2 = new Person("Eve", null, 20, p1, null, null, false);
        p1.sigOther = p2;
        initialSettlerTest.Add(p1);
        initialSettlerTest.Add(p2);
        settlers = initialSettlerTest;
        aliveResidents = initialSettlerTest;
        Singleton = this;
    }

    //Is this a life event?  Also may separate the system randomness / choosing from the exact method, instead using a parameter of a Person and just doing the effects on the Lists.
    public void death(Person selectedToDie){


        //Shotty version at removing a person from associated lists
        //Person search = settlers.Find(x => x.id == selectedToDie.id);
        aliveResidents.Remove(selectedToDie);
        deceased.Add(selectedToDie);

        // Life event does further processing...?
    }

    public void birth(Person baby){
        Log("Successful birth", baby.name);
        //Logger.Log("person", ("Successful birth occurred! Details:"), baby.toString());
        aliveResidents.Add(baby);
    }

    //Every timestep this method is called.
    public override void Step(){
        /* Birth, right now can only occur once per step */

        //Select Parent Randomly: LINQ
        /*
        var livingPeople =  from Agent in Agents
                            where Agent.getLivingStatus() == true
                            select Agent;
        */
        //OOP Method:

        var noSigOtherFem = from women in aliveResidents
                            where (women.isFemale() && women.sigOther == null)
                            select women;

        var noSigOtherMale = from men in aliveResidents
                             where (men.isMale() && men.sigOther == null)
                             select men;

        foreach(Person pm in noSigOtherMale)
        {
            foreach(Person pf in noSigOtherFem)
            {
                if(pm.sigOther != null)
                {
                    continue;
                }
                else
                {
                    if(pf.sigOther == null)
                    {
                        pm.sigOther = pf;
                        pf.sigOther = pm;
                    }
                }
            }
        }


        int birthDice = global::Random.Integer(100);
        if(birthDice < PersonTown.birthProbability){

            bool found = false;

            //Maximum Attempts a birth event will be attempted, should lend itself to being more sparce/random in accordance to the population.  The value of 5 is arbitrary.
            //Approach can be varied depending on whether a baby MUST be found, in that case would do randoms until success.
            int maxAttempts = 5;
            Person baby = null;

            while(found != true && maxAttempts>0){
                Person selectedParent = aliveResidents.ElementAt(Random.Integer(aliveResidents.Count));

                // Debug.LogFormat("Selected {0} with sigOther {1}", selectedParent.name, selectedParent.sigOther.name);

                Person newborn = Person.createChild(selectedParent, selectedParent.sigOther);

                if(newborn != null){
                    found = true;
                    baby = newborn;
                }
                maxAttempts--;                
            }
            if(!found){
                Log("Birth Failed, parents not found or exceeded maximum attempts");
            }
            else{
                birth(baby);
            }            
        }

    /* Death,  right now can only occur once per step */
        /*Linq Method
        //Must Query all living inhabitants and select one to become deceased...?
        var livingPeople = from People in people
                           where People.alive == true
                           select Agent;
        */
        
        //OOP Method:
        //Now that have all living Agents, randomly select one to die, at a 10% chance

        int deathDice = Random.Integer(100);
        if(deathDice < deathProbability){
            //Someone is dying
            Person selectedToDie = aliveResidents.ElementAt(Random.Integer(0, aliveResidents.Count()));

            //Set flag of selectedAgent to non-living or deceased?
            //Is Agent a GameObject?
            selectedToDie.alive = false;

            death(selectedToDie);
        }
    }
}


