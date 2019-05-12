using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : Object
{
    public enum street_direction { NS, EW }
    public string streetName;
    public int direction; // [0-1 -> NS EW]
    static int CONNECTED_STREETS_MAX = 10; // arbitrary max of 10
    public Street[] connected_streets = new Street[CONNECTED_STREETS_MAX];

    public Street(int street_dir, string name)
    {
        direction = street_dir;
        streetName = name;
    }
}
