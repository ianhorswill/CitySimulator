using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Codes.Institution;

public class Plot : System.Object
{
    public int x_pos, y_pos; // x, y coords
    public Plot[] neighbor_plots = new Plot[4]; // stored N,E,S,W
    public bool occupied;
    public List<Institution> institutions = new List<Institution>();
    private Space space;
    public Color color = Color.green;

    public Plot(int x, int y)
    {
        occupied = false; 
        x_pos = x;
        y_pos = y;
        space = Space.Singleton;
    }

    public Vector2 get_coords()
    {
        Vector2 coord = new Vector2(x_pos, y_pos);
        return coord; 
    }

    int distance_between_plots(Plot plot_1, Plot plot_2)
    {
        int x_diff = Math.Abs(plot_1.x_pos - plot_2.x_pos);
        int y_diff = Math.Abs(plot_1.y_pos - plot_2.y_pos);
        return x_diff + y_diff;
    }

    public List<Plot> get_neighbor_plots()
    {
        List<Plot> neighbors = new List<Plot>(neighbor_plots);
        neighbors.RemoveAll(plot => plot == null);
        return neighbors;
    }

    public List<Institution> get_nearby_institutions()
    {
        List<Institution> nearby_institutions = institutions;
        foreach (Plot neighbor_plot in neighbor_plots) {
            if (neighbor_plot != null)
            {
                nearby_institutions.AddRange(neighbor_plot.institutions);
            }
        }
        return nearby_institutions;
    }

    public List<Street> get_bordering_streets()
    {
        return new List<Street> { space.streets_list[x_pos, 0],
                                  space.streets_list[x_pos + 1, 0],
                                  space.streets_list[y_pos, 1],
                                  space.streets_list[y_pos + 1, 1]};
    }

    public void set_color(Color plot_color)
    {
        color = plot_color;
    }

    public Vector2 world_midpoint_coords()
    {
        float x_coord = (x_pos + 0.5f) * Space.Singleton.draw_scale;
        float y_coord = (y_pos + 0.5f) * Space.Singleton.draw_scale;
        return new Vector2(x_coord, y_coord);
    }

    public Vector2 world_random_coords()
    {
        float x_coord = (x_pos + Random.Float(-0.25f, 0.25f)) * Space.Singleton.draw_scale;
        float y_coord = (y_pos + Random.Float(-0.25f, 0.25f)) * Space.Singleton.draw_scale;
        return new Vector2(x_coord, y_coord);
    }

    public void add_institution(Institution ittn)
    {
        if (institutions.Count > 0)
        {
            return;
        }
        space.mark_occupied(this);
        institutions.Add(ittn);
    }

    /// <summary>
    /// remove institution from plot and mark the plot as unoccupied
    /// </summary>
    /// <param name="ittn">institution to be removed</param>
    /// <returns>true if inst removed from plot false otherwise</returns>
    public bool remove_institution(Institution ittn)
    {
        space.mark_unoccupied(this);
        return institutions.Remove(ittn);  
    }

    public override string ToString()
    {
        return base.ToString() + "(" + x_pos + ", " + y_pos + ")";
    }
}
