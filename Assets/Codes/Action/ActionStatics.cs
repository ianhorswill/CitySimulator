using System;
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
    
    private static int seed = Environment.TickCount;
    // Logger.Log("seed", seed.ToString());
    public static System.Random randomNumberGenerator = new System.Random(seed);
    
    private static DateTime world_start = new DateTime(1850, 1, 1, 10, 0, 0);
    private static TimeSpan day_duration = new TimeSpan(0, 12, 0, 0);
    public DateTime current_time = world_start;
    
    public void advance_time() {
        current_time = current_time.Add(day_duration)
    }
}
