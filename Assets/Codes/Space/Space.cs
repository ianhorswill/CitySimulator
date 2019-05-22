﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Space : SimulatorComponent
{
    public static int MAX_PLOTS = 100;
    public static int MAX_STREETS = 1000;
    public Plot[,] plots_list;
    public Street[,] streets_list;
    //float streetStretch = 1.505f;
    public int grid_len = 10;
    public static Space Singleton;
    List<Plot> empty_plots =
            new List<Plot>();
    List<Plot> occupied_plots =
            new List<Plot>();
    public float draw_scale = 2;

    public Space()
    {
        Singleton = this; 
    }

    // Start is called before the first frame update
    public override void Initialize()
    {
        plots_list = new Plot[grid_len, grid_len];
        streets_list = new Street[grid_len+1,2];

        // make starting plots and streets
        for (int y = 0; y < grid_len; y++)
        {
            for (int x = 0; x < grid_len; x++)
            {
                Plot new_plot = make_plot(x, y);
                plots_list[x, y] = new_plot;
                empty_plots.Add(new_plot);
            }
        }

        for (int dir = 0; dir < 2; dir++) // 0 : N and 1 : E
        {
            for (int level = 0; level < grid_len + 1; level++)
            {
                Street.street_direction street_dir = (dir == 0)
                    ? Street.street_direction.NS 
                    : Street.street_direction.EW;
                streets_list[level,dir] = make_street(level, street_dir);
            }
        }

        setup_plot_neighbors();
        setup_street_connections();
    }

    public Plot get_random_plot()
    {
        int rand_x = Random.Integer(0, grid_len);
        int rand_y = Random.Integer(0, grid_len);

        return plots_list[rand_x, rand_y];
    }

    /// <summary>
    /// Returns a random plot with no institutions on it. Returns null if no 
    /// empty plots.
    /// </summary>
    public Plot get_random_empty_plot()
    {
        if (empty_plots.Count == 0)
        {
            return null;
        }
        return Random.RandomElement(empty_plots);
    }

    /// <summary>
    /// Returns the plot with the highest value when evaluated by plot_eval.
    /// If there is a tie, the first-considered tied plot is always returned
    /// </summary>
    public Plot get_best_plot(Func<Plot, float> plot_eval)
    {
        Plot best_plot = null;
        float best_plot_val = 0;
        foreach(Plot p in plots_list)
        {
            if (best_plot == null || plot_eval(p) > best_plot_val)
            {
                best_plot = p;
                best_plot_val = plot_eval(p);
            }
        }
        return best_plot;
    }

    Plot make_plot(int x, int y)
    {
        Plot newPlot = new Plot(x, y);

        return newPlot;
    }

    Street make_street(int lvl, Street.street_direction dir)
    {
        string dir_name = dir == Street.street_direction.NS ? "N. " : "E. ";
        Street new_street = new Street(dir, dir_name + lvl + " Street");

        return new_street;
    }

    // hard-coded method for setting up plot neighbors
    // will likely need to change if we want new plots to be created during simulation
    void setup_plot_neighbors()
    {
        for(int x = 0; x < grid_len; x++)
        {
            for (int y = 0; y < grid_len; y++)
            {
                Plot curr_plot = plots_list[x, y];
                Plot north = (y == grid_len - 1) ? null : plots_list[x, y + 1];
                Plot east = (x == grid_len - 1) ? null : plots_list[x + 1, y];
                Plot south = (y == 0) ? null : plots_list[x, y - 1];
                Plot west = (x == 0) ? null : plots_list[x-1, y];
                curr_plot.neighbor_plots[0] = north;
                curr_plot.neighbor_plots[1] = east;
                curr_plot.neighbor_plots[2] = south;
                curr_plot.neighbor_plots[3] = west;
            }
        }
    }

    // hard-coded method for setting street connections
    // will likely need to change if we want new plots to be created during simulation
    void setup_street_connections()
    {
        for (int level = 0; level < (grid_len + 1); level++)
        {
            for (int dir = 0; dir < 2; dir++)
            {
                Street curr_street = streets_list[level,dir];
                int other_dir = (dir == 0) ? 1 : 0;
                for (int j = 0; j < grid_len + 1; j++)
                {
                    curr_street.connected_streets.Add(streets_list[j,other_dir]);
                }
            }
        }
        
    }

    public void mark_occupied(Plot p)
    {
        empty_plots.Remove(p);
        occupied_plots.Add(p);
    }

    public void mark_unoccupied(Plot p)
    {
        occupied_plots.Remove(p);
        empty_plots.Add(p);
    }

    public override void Visualize()
    {
        float plot_side_len = 1 * draw_scale;

        // draw all square plots
        foreach (Plot p in plots_list)
        {
            Vector2 midpoint = p.world_midpoint_coords();
            Rect p_rect = new Rect(midpoint.x - (0.5f * plot_side_len),
                                   midpoint.y - (0.5f * plot_side_len),
                                   plot_side_len,
                                   plot_side_len);
            Draw.Rect(p_rect, p.color, -1);
        }

        // draw all streets
        for (int level = 0; level < grid_len + 1; level++)
        {
            Rect hori_rect = new Rect(0, draw_scale * level, draw_scale * (grid_len + 0.1f), draw_scale * 0.1f);
            Rect vert_rect = new Rect(draw_scale * level, 0, draw_scale * 0.1f, draw_scale * (grid_len + 0.1f));
            Draw.Rect(vert_rect, Color.black);
            Draw.Rect(hori_rect, Color.black);
        }
    }
}
