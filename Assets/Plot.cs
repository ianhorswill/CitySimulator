using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class Plot : MonoBehaviour
{
    public int x_pos, y_pos; // x, y coords
    public GameObject[] neighbor_plots; // stored N,E,S,W
    public bool occupied;
    public GameObject[] building_list; 

    void Awake()
    {
        neighbor_plots = new GameObject[4];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int distance_between_plots(Plot plot_1, Plot plot_2)
    {
        int x_diff = Math.Abs(plot_1.x_pos - plot_2.x_pos);
        int y_diff = Math.Abs(plot_1.y_pos - plot_2.y_pos);
        return x_diff + y_diff; 
    }
}
