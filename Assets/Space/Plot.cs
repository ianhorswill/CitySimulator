using System.Collections;
using System.Collections.Generic;
using System;

public class Plot : Object
{
    public int x_pos, y_pos; // x, y coords
    public Plot[] neighbor_plots = new Plot[4]; // stored N,E,S,W
    public bool occupied;
    //public GameObject[] building_list;


    public Plot(int x, int y)
    {
        x_pos = x;
        y_pos = y; 
    }

    int distance_between_plots(Plot plot_1, Plot plot_2)
    {
        int x_diff = Math.Abs(plot_1.x_pos - plot_2.x_pos);
        int y_diff = Math.Abs(plot_1.y_pos - plot_2.y_pos);
        return x_diff + y_diff;
    }
}