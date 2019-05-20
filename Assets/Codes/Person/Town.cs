using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PersonTown : SimulatorComponent
{
    public override void Initialize()
    {
        Watch("Display population", () => $"Population: {aliveResidents.Count}");
        Watch("Display deceased", () => $"Deceased: {deceased.Count}");
        StopWhen("Population died off", () =>
            aliveResidents.Count == 0 &&
            Simulator.CurrentTime > Simulator.WorldStart + new TimeSpan(30, 0, 0, 0));
    }
    //LINQ version: Just do one array/list, "Person[] people {get; set;}" or "var people = new List<Person>();" and perform queries on them

    //OOP version: Series of arrays that hold people pertaining to each property: original settlers, departed townsfolk, current alive residents, and deceased residents.
    public readonly List<Person> settlers = new List<Person>();
    public readonly List<Person> departed = new List<Person>();
    public readonly List<Person> aliveResidents = new List<Person>();
    public readonly  List<Person> deceased = new List<Person>();

    private static int deathProbability = 1;
    private static int birthProbability = 8;


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

        int randPeopleCount = 10;
        for(int i = 0; i < randPeopleCount; i++)
        {
            Person newRandPerson = Person.generateRandomPerson();
            initialSettlerTest.Add(newRandPerson);
        }

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
        if(baby != null)
        {
            Log("Successful birth", baby.name);
            //Logger.Log("person", ("Successful birth occurred! Details:"), baby.toString());
            aliveResidents.Add(baby);



        }

    }

    //Every timestep this method is called.
    public override void Step() {
        /* Birth, right now can only occur once per step */

        //Select Parent Randomly: LINQ
        /*
        var livingPeople =  from Agent in Agents
                            where Agent.getLivingStatus() == true
                            select Agent;
        */
        //OOP Method:
        StopWhen("No one is alive", ()=> aliveResidents.Count == 0);


        var noSigOtherFem = from women in aliveResidents
                            where (women != null && women.isFemale() && women.sigOther == null)
                            select women;

        var noSigOtherMale = from men in aliveResidents
                             where (men != null && men.isMale() && men.sigOther == null)
                             select men;

        if (noSigOtherMale != null && noSigOtherFem != null)
        {
            foreach (Person pm in noSigOtherMale)
            {
                foreach (Person pf in noSigOtherFem)
                {
                    if (pm.sigOther != null)
                    {
                        continue;
                    }
                    else
                    {
                        if (pf.sigOther == null)
                        {
                            pm.sigOther = pf;
                            pf.sigOther = pm;
                        }
                    }
                }
            }
        }   

        int birthDice = Random.Integer(100);
        if(birthDice < PersonTown.birthProbability){

            bool found = false;

            //Maximum Attempts a birth event will be attempted, should lend itself to being more sparce/random in accordance to the population.  The value of 5 is arbitrary.
            //Approach can be varied depending on whether a baby MUST be found, in that case would do randoms until success.
            int maxAttempts = 5;
            Person baby = null;

            // May want to consider changing how we decide to choose a "selected parent"
            // Currently, part of the reason birth rates are so low is due to the fact that this will also randomly
            // attempt to choose children.
            // Going to see how changing this selection to only be of "ofAgeIndividuals" effects this.
            while(found != true && maxAttempts>0){
                var ofAgeIndividuals = from people in aliveResidents
                                       where (people.age >= 18)
                                       select people;

                //Person selectedParent = aliveResidents.ElementAt(Random.Integer(aliveResidents.Count));
                Person selectedParent = null;
                var possibleIndex = Random.Integer(ofAgeIndividuals.Count());

                if(ofAgeIndividuals.Count() != 0)
                {
                    selectedParent = ofAgeIndividuals.ElementAt(possibleIndex);
                }

                // Debug.LogFormat("Selected {0} with sigOther {1}", selectedParent.name, selectedParent.sigOther.name);

                Person newborn = null;

                if (selectedParent != null && selectedParent.sigOther != null)
                {
                   newborn = Person.createChild(selectedParent, selectedParent.sigOther);
                }

                if (newborn != null){
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

    /* If there exists a school and there are people under 18, sets their current occupation to the school */
        var under18NotInSchool = from child in aliveResidents
                            where (child != null && child.age <= 18 && child.workStatus.workplace == null)
                            select child;
        //Right now since construction companies are the most fleshed out, choose to send students to one; later when framework supports schools change this.
        ConstructionCompany randomSchoolIfAny = InstitutionManager.GetRandomConstructionCompany();

        if (under18NotInSchool != null && randomSchoolIfAny != null)
        {
            foreach (Person pc in under18NotInSchool)
            {
                pc.workStatus.getNewJob(randomSchoolIfAny, 0);        
                pc.personalEducation.is_student = true; 
            }
        } 

        //If someone turns 19 and they were in school, they "graduate" and lose the school occupation status and their education field is updated.
        var is19InSchool = from newAdult in aliveResidents
                            where (newAdult != null && newAdult.age <= 18 && newAdult.workStatus.workplace.getType().Equals("ConstructionCompany"))
                            select newAdult;
        foreach (Person pa in is19InSchool)
        {
            pa.workStatus.loseJob();
            pa.is_high_school_graduate = true;
            pa.is_student = false;
        }        

    }



    public override void Visualize()
    {
        int nameCount = 0;
        int nameLimit = 500;

        var tempAliveResidents = new List<Person>(aliveResidents);
        foreach (var p in tempAliveResidents)
        {
            if (nameCount <= nameLimit)
            {
                var currCoords = p.currentLocation.world_midpoint_coords();
                Draw.Text(p.name, currCoords);
                nameCount++;
            }
            else
            {
                break;
            }

        }

    }
}




