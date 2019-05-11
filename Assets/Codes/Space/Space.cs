using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Space : UnityEngine.Object
{
    static public int MAX_PLOTS = 100;
    static public int MAX_STREETS = 1000;
    public Plot[,] plots_list;
    public Street[,] streets_list;
    //float streetStretch = 1.505f;
    public int grid_len = 3;

    // Start is called before the first frame update
    public void init()
    {
        plots_list = new Plot[grid_len, grid_len];
        streets_list = new Street[grid_len+1,2];

        // make starting plots and streets
        for (int y = 0; y < grid_len; y++)
        {
            for (int x = 0; x < grid_len; x++)
            {
                plots_list[x,y] = make_plot(x, y);
            }
        }

        for (int dir = 0; dir < 2; dir++) // 0 : N and 1 : E
        {
            for (int level = 0; level < grid_len + 1; level++)
            {
                streets_list[level,dir] = make_street(level, dir);
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
        Plot newPlot = new Plot(x, y, this);

        return newPlot;
    }

    Street make_street(int lvl, int dir)
    {
        string dir_name = dir == 0 ? "N. " : "E. ";
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
                    curr_street.connected_streets[j] = streets_list[j,other_dir];
                }
            }
        }
        
    }

    public void draw_map()
    {
        float scale = 2f;

        // draw all square plots
        for (int x = 0; x < grid_len; x++)
        {
            for (int y = 0; y < grid_len; y++)
            {
                Draw.Rect(new Rect(scale * x, scale * y, scale, scale), Color.green, -1);
            }
        }

        // draw all streets
        for (int level = 0; level < grid_len + 1; level++)
        {
            Rect hori_rect = new Rect(0, scale * level, scale * (grid_len + 0.1f), scale * 0.1f);
            Rect vert_rect = new Rect(scale * level, 0, scale * 0.1f, scale * (grid_len + 0.1f));
            Draw.Rect(vert_rect, Color.black);
            Draw.Rect(hori_rect, Color.black);
        }
    }
}
