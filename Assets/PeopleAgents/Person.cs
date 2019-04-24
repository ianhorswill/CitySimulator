﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface PersonAgent
{
    int age;
    string name;

    var currentLocation;
    var relationships;


    PersonAgent sigOther;
    PersonAgent[] siblings;
    PersonAgent[] parents;

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
        // createChild(p1, p1.sigOther)
        p2 = otherParents[0];

        
    }
}