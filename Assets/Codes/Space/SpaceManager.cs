using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    Space my_space;
    // Start is called before the first frame update
    void Start()
    {
        Logger.Log("space", "Creating initial space");
        my_space = new Space();
        my_space.init();
    }

    // Update is called once per frame
    void Update()
    {
        my_space.draw_map();
    }
}
