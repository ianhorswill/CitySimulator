using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Is this action related?
/*  Commenting out this interface because uncertain necessity
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
*/

public class PersonTown //: Settlement
{
    //LINQ version: Just do one array/list, "Person[] people {get; set;}" or "var people = new List<Person>();" and perform queries on them

    //OOP version: Series of arrays that hold people pertaining to each property: original settlers, departed townsfolk, current alive residents, and deceased residents.
    public var settlers = new List<Person>();
    public var departed = new List<Person>();
    public var aliveResidents = new List<Person>();
    public var deceased = new List<Person>();

    private int deathProbaility = 10;
    private int birthProbability = 20;

    //For David: Going to add another constructor to People that allows the creation of adults
    //     public Person (string name, List<Person> currSiblings, int age, Person sigOther, List<Person> children, Person[] parents, bool biologicalSex){

    //Constructor with settler input
    public PersonTown(List<Person> newSettlers)
    {
        settlers = newSettlers;
        aliveResidents = newSettlers;
    }

    //Default constructor with no settler input / Uses Adam/Eve
    public PersonTown(){
        var initialSettlerTest = new List<Person>();
        Person p1 = new Person("Adam", null, 20, p2, null, null, true);
        Person p2 = new Person("Eve", null, 20, p1, null, null, false);
        initialSettlerTest.Add(p1);
        initialSettlerTest.Add(p2);
        PersonTown(initialSettlerTest);
    }

    //Is this a life event?  Also may separate the system randomness / choosing from the exact method, instead using a parameter of a Person and just doing the effects on the Lists.
    public static void death(Person selectedToDie){
        

        //Shotty version at removing a person from associated lists
        //Person search = settlers.Find(x => x.id == selectedToDie.id);
        aliveResidents.Remove(selectedToDie);
        deceased.Add(selectedToDie);

        // Life event does further processing...?
    }

    public static void birth(Person baby){
        aliveResidents.Add(baby);
    }

    //Every timestep this method is called.
    public static void step(){
        Random rng = new Random();        
        /* Birth, right now can only occur once per step */

            //Select Parent Randomly: LINQ
            /*
            var livingPeople =  from Agent in Agents
                                where Agent.getLivingStatus() == true
                                select Agent;
            */
        //OOP Method:
        int birthDice = rng.Next(100);
        if(birthDice < birthProbaility){

            bool found = false;

            //Maximum Attempts a birth event will be attempted, should lend itself to being more sparce/random in accordance to the population.  The value of 5 is arbitrary.
            //Approach can be varied depending on whether a baby MUST be found, in that case would do randoms until success.
            int maxAttempts = 5;
            Person baby;

            while(found != true && maxAttempts>0){
                Random r = new Random();
                Person selectedParent = livingPeople.ElementAt(r.Next(0, livingPeople.Count()));
                Person newborn = createChild(selectedParent, selectedParent.sigOther);
                if(newborn != null){
                    found = true;
                    baby = newborn;
                }
                maxAttempts--;                
            }
            if(!found){
                Console.write("Birth Failed, parents not found or exceeded maximum attempts");
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

        int deathDice = rng.Next(100);
        if(deathDice < deathProbaility){
            //Someone is dying
            Person selectedToDie = aliveResidents.ElementAt(rng.Next(0, aliveResidents.Count()));

            //Set flag of selectedAgent to non-living or deceased?
            //Is Agent a GameObject?
            selectedToDie.alive = false;

            death(selectedToDie);
        }
    }
}


