﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : System.Object
{
    public enum street_direction { NS, EW }
    public string streetName;
    public street_direction direction; // [0-1 -> NS EW]
    public List<Street> connected_streets = new List<Street>();
    private Space space;

    public Street(street_direction dir, string name)
    {
        direction = dir;
        streetName = name;
        space = Space.Singleton;
    }

    public override string ToString()
    {
        return base.ToString() + "(" + streetName + ")";
    }
}
