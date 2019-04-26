using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
    public string streetName;
    public int direction; // [0-1 -> NS EW]
    static int CONNECTED_STREETS_MAX = 10; // arbitrary max of 10
    public GameObject[] connected_streets;

    void Awake()
    {
        connected_streets = new GameObject[CONNECTED_STREETS_MAX];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
