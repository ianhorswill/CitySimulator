using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface PersonAgent : GameObject
{
    int age { get; set; }
    string name { get; set; }

    var currentLocation { get; set; }
    var relationships { get; set; }


    PersonAgent sigOther { get; set; }
    PersonAgent[] siblings { get; set; }
    PersonAgent[] parents{ get; set; }

    public static PersonAgent createChild(PersonAgent p1, params PersonAgent[] otherParents) {
        /*
         * createChild takes in at least one PersonAgent, p1
         * Depending on the constraints of what implements PersonAgent, it can take in other PersonAgents as additional args
         * 
         * Based on whatever restrictions the implemented class has, it should then return a new PersonAgent
         * 
         */

    } 
}



public class Person : PersonAgent, NameManager
{


    public static PersonAgent createChild(PersonAgent p1, params PersonAgent[] otherParents)
    {
        // Assuming "Person" is a humant variant, otherParents should be of length 1
        // e.g. a call for two humans, p1 and p2, creating a child should look like
        // var createdChild = createChild(p1, p2)

        
    }
}


