using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Person
{
    private int age;
    public int Age
    {
        get
        {
            return age;
        }
        set
        {
            age = value;
        }
    }

    private string name;
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    // We will have things here eventually.
    // private var currentLocation;
    // private var relationships;
    


    Person sigOther;
    List<Person> siblings;
    List<Person> children;
    Person[] parents;
    private static double conceptionRate = 0.25;

    private bool biologicalSex;
    public bool isMale()
    {
        return biologicalSex;
    }

    public bool isFemale()
    {
        return !biologicalSex;
    }

    public Person(string nameAtBirth, List<Person> currSiblings)
    {
        age = 0;
        name = nameAtBirth;
        sigOther = null;
        siblings = new List<Person>();
        children = null;
        parents = new Person[2];
        System.Random rng = new System.Random();
        if(rng.Next(1) == 1)
        {
            biologicalSex = true;
        }
        else
        {
            biologicalSex = false;
        }


        foreach(Person p in currSiblings)
        {
            siblings.Add(p);
        }
    }

    public static Person createChild(Person p1, params Person[] otherParents)
    {
        // createChild(p1, p1.sigOther)
        Person p2 = otherParents[0];

        if(otherParents.Length >= 1)
        {
            // Humans should only have two biological parents
            return null;
        }


        int minAge = 18;
        if(p2.Age >= minAge && p1.Age >= minAge)
        {
            // if both individuals are above the minimum age, then they may have a child
            if(p1.isFemale() && p2.isMale() || p1.isMale() && p2.isFemale())
            {
                System.Random rng = new System.Random();
                double birthChance = rng.NextDouble();
                if(birthChance <= conceptionRate)
                {

                    string tempNameGen = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
                    List<Person> currSiblings = null;
                    if(p1.children != null)
                    {
                        if(p2.children != null)
                        {
                            var siblingsCollection =
                                from child in p1.children
                                where !(p2.children.Contains(child))
                                select child;

                            siblingsCollection = siblingsCollection.Union(from child in p2.children where !(p1.children.Contains(child)) select child);

                            currSiblings = siblingsCollection.ToList<Person>();
                        }

                        currSiblings = p1.children;
                    }
                    else if(p2.children != null)
                    {
                        currSiblings = p2.children;
                    }

                    Person bornChild = new Person(tempNameGen, currSiblings);
                }
            }

        }

        return null;
        
    }
}