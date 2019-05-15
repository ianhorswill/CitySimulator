using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : Object
{
    public enum street_direction { NS, EW }
    public string streetName;
    public int direction; // [0-1 -> NS EW]
    public List<Street> connected_streets = new List<Street>();

    public Street(int street_dir, string name)
    {
        direction = street_dir;
        streetName = name;
    }
}
