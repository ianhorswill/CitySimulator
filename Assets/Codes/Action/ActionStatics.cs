using System.Collections.Generic;
using UnityEngine;

public class ActionStatics
{
    public static int DEATH_PROBABILITY = 5;
    public static int BIRTH_PROBABILITY = 100;
    public static int MIN_AGE = 0;
    
    [SerializeField] public static List<Person> settlers = new List<Person>();
    public static List<Person> departed = new List<Person>();
    [SerializeField] public static List<Person> aliveResidents = new List<Person>();
    public static List<Person> deceased = new List<Person>();
    
    public static System.Random randomNumberGenerator = new System.Random();
    // seed based!
    
    // time?
}
