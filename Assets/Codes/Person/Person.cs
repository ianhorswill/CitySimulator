using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Person
{
    public int age {get; set;}
    /// <summary>
    /// The name of the person in the format of "firstname lastname"
    /// </summary>
    public string name
    {
        get { return firstName + " " + lastName; }
        set { }
    }
    
    public string firstName;
    public string GenerateRandomFirstName()
    {
        NameManager.sex sex = biologicalSex ? NameManager.sex.male : NameManager.sex.female;
        return NameManager.getFirstname(sex);
    }
    
    public string lastName;
    public string GenerateRandomLastName()
    {
        List<string> parentNames = new List<string>();
        if (parents != null)
        {
            for (int i = 0; i < parents.Length; i++)
            {
                if (parents[i] != null) parentNames.Add(parents[i].lastName);
            }
        }
        return NameManager.getSurname(parentNames);
    }

    public bool alive {get; set;}

    public int id {get; set;}

    // We will have things here eventually.
    // private var currentLocation;
    // private var relationships;
    private Dictionary<Person, List<int>> relationshipDict = new Dictionary<Person, List<int>>();

    public void updateRelationshipSpark()
    {
        
    }
    public void updateRelationshipCharge()
    {
        
    }
    
    public Person sigOther;
    public List<Person> siblings;
    public  List<Person> children;
    public Person[] parents;
    private static double conceptionRate = 1.0; // Should be lower, but setting higher for the sake of it.
    
    /// <summary>
    /// The gender of the person: if biologicalSex is true, the person is Male; if false, female.
    /// </summary>
    private bool biologicalSex;
    public bool isMale()
    {
        return biologicalSex;
    }
    public bool isFemale()
    {
        return !biologicalSex;
    }
    
    /// <summary>
    /// Interface function to generate a person with randomly properties
    /// </summary>
    public static Person generateRandomPerson()
    {
        Person p = new Person("",null);
        System.Random rng = new System.Random();
        p.biologicalSex = (rng.Next(0, 2) == 1);
        p.firstName = NameManager.getFirstname(p.biologicalSex ? NameManager.sex.male : NameManager.sex.female);
        p.lastName = NameManager.getSurname(null);
        return p;
    }
    
    /// <summary>
    /// Basic Constructor for newborns, takes in string nameAtBirth, and list of current siblings
    /// string nameAtBirth can be an empty string. the constructor will assign a randomly generated name.
    /// </summary>
    public Person(string nameAtBirth, List<Person> currSiblings)
    {
        age = 0;
        sigOther = null;
        siblings = new List<Person>();
        children = null;
        parents = new Person[2];
        System.Random rng = new System.Random();
        if(rng.Next(0, 2) == 1)
        {
            biologicalSex = true;
        }
        else
        {
            biologicalSex = false;
        }
        
        if(currSiblings != null)
        {
            foreach (Person p in currSiblings)
            {
                siblings.Add(p);
            }
        }
        string[] nameSplit = nameAtBirth.Split(' ');
        this.firstName = (nameAtBirth.Length != 0) ? nameSplit[0] : GenerateRandomFirstName();
        this.lastName = (nameSplit.Length > 1) ? nameSplit[1] : GenerateRandomLastName();
    }
    /// <summary>
    /// Constructor for adults, those who may enter the town or are settlers
    /// string nameAtBirth can be an empty string. the constructor will assign a randomly generated name.
    /// </summary>
    public Person (string name, List<Person> currSiblings, int age, Person sigOther, List<Person> children, Person[] parents, bool biologicalSex){
        this.age = age;
        this.sigOther = sigOther;
        this.siblings = currSiblings;
        this.children = children;
        this.parents = parents;
        this.biologicalSex = biologicalSex;
        string[] nameSplit = name.Split(' ');
        
        this.firstName = (name.Length != 0) ? nameSplit[0] : GenerateRandomFirstName();
        this.lastName = (nameSplit.Length > 1) ? nameSplit[1] : GenerateRandomLastName();
    }
    
    /// <summary>
    /// Interface function for creating a child
    /// </summary>
    public static Person createChild(Person p1, params Person[] otherParents)
    {
        // createChild(p1, p1.sigOther)
        Person p2 = otherParents[0];
        if(otherParents.Length > 1)
        {
            // Humans should only have two biological parents
            return null;
        }


        int minAge = 0; // Change this to like 18 when we actually have age incrementing properly
        if(p1 == null || p2 == null)
        {
            return null;
        }

        if(p2.age >= minAge && p1.age >= minAge)
        {
            // if both individuals are above the minimum age, then they may have a child
            // Debug.LogFormat("Adam and Eve Results: {0}, {1}, {2}", p1.isFemale(), p2.isMale(), p1.isFemale() && p2.isMale());
            // Debug.LogFormat("Other Result: {0}, {1}, {2}", p1.isMale(), p2.isFemale(), p1.isMale() && p2.isFemale());
            if((p1.isFemale() && p2.isMale()) || (p1.isMale() && p2.isFemale()))
            {
                System.Random rng = new System.Random();
                double birthChance = rng.NextDouble();
                // Debug.LogFormat("Chance of birth: {0}/{1}", birthChance, conceptionRate);
           
                if(birthChance <= conceptionRate)
                {
                    
                    // string tempNameGen = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
                    List<Person> currSiblings = null;
                    if(p1.children != null)
                    {
                        if(p2.children != null)
                        {
                            var siblingsCollection =
                                from child in p1.children
                                where !(p2.children.Contains(child))
                                select child;

                            siblingsCollection = siblingsCollection.Union(from child in p2.children select child);

                            currSiblings = siblingsCollection.ToList<Person>();
                        }

                        currSiblings = p1.children;
                    }
                    else if(p2.children != null)
                    {
                        currSiblings = p2.children;
                    }

                    Person bornChild = new Person("", currSiblings);
                    bornChild.parents = new Person[] { p1, p2 };
                    return bornChild;
                }
            }

        }
        return null;  
    }

    public string getNamesFromListOfPersons(List<Person> listOfPersons)
    {
        if (listOfPersons == null || listOfPersons.Count == 0) return "None";
        string names = "";
        for (int i = 0; i < listOfPersons.Count; i++)
        {
            names += listOfPersons[i];
            names += ", ";
        }
        return names;
    }
    public override string ToString()
    {
        string parentNames = "";
        if ((parents == null) || (parents.Length == 0))
            parentNames = "None";
        else
        {
            for (int i = 0; i < parents.Length; i++)
            {
                parentNames += parents[i].name;
                parentNames += ", ";
            }
        }
        return string.Format("Person Name: {0}, Age: {1}, Sex: {2}, Significant Other: {3}, Parents: {4}, Siblings: {5}, Children: {6}",
            name, age, biologicalSex ? "M" : "F", sigOther == null ? "N/A" : sigOther.name, 
            parentNames, getNamesFromListOfPersons(siblings), getNamesFromListOfPersons(children));
    }
}