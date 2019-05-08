using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    Space mySpace;
    // Start is called before the first frame update
    void Start()
    {
        mySpace = new Space();
        mySpace.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
