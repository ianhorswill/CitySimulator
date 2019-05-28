using System.Collections.Generic;
using UnityEngine;
using System;

public class Space : SimulatorComponent
{
    private const int MaxPlots = 100;
    private const int GridLen = 10;
    public static int NumPlots = 0;
    
    public const float DrawScale = 2.0f;
    
    public Plot[,] plots_array;
    public Street[,] streets_list;
    readonly List<Plot> empty_plots = new List<Plot>();
    readonly List<Plot> occupied_plots = new List<Plot>();
    
    public static Space Singleton;
    
    public Space(){ Singleton = this; }

    // Initialize is called before the first frame update
    public override void Initialize()
    {
        plots_array = new Plot[GridLen, GridLen];
        streets_list = new Street[GridLen+1,2];

        generate_plots();
        generate_streets();

        setup_plot_neighbors();
        connect_streets();
    }
    
    private void generate_plots()
    {
        for (int i = 0; i < MaxPlots / 2; i++)
        {
            int x = Random.Integer(0, GridLen);
            int y = Random.Integer(0, GridLen);
        
            if (plots_array[x, y] == null)
            {
                make_plot(x, y);
            }
        }
    }

    private void generate_streets()
    {
        for (int dir = 0; dir < 2; dir++) // 0 : N and 1 : E
        {
            for (int level = 0; level < GridLen + 1; level++)
            {
                Street.street_direction street_dir = (dir == 0)
                    ? Street.street_direction.NS
                    : Street.street_direction.EW;
                streets_list[level, dir] = make_street(level, street_dir);
            }
        }
    }

    public Plot get_random_plot()
    {
        int x = Random.Integer(0, GridLen);
        int y = Random.Integer(0, GridLen);

        return plots_array[x,y] ?? get_random_plot();
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
        foreach(Plot p in plots_array)
        {
            if (best_plot == null || plot_eval(p) > best_plot_val)
            {
                best_plot = p;
                best_plot_val = plot_eval(p);
            }
        }
        return best_plot;
    }

    void make_plot(int x, int y)
    {
        Plot newPlot = new Plot(x, y);
        
        plots_array[x, y] = newPlot;
        empty_plots.Add(newPlot);

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
        foreach (Plot p in plots_array)
        {
            if (p == null) continue;
            
            int x = p.x_pos;
            int y = p.y_pos;
            
            Plot north = (y == GridLen - 1) ? null : plots_array[x, y + 1];
            Plot east = (x == GridLen - 1) ? null : plots_array[x + 1, y];
            Plot south = (y == 0) ? null : plots_array[x, y - 1];
            Plot west = (x == 0) ? null : plots_array[x-1, y];
            
            p.neighbor_plots[0] = north;
            p.neighbor_plots[1] = east;
            p.neighbor_plots[2] = south;
            p.neighbor_plots[3] = west;
        }
    }

    // hard-coded method for setting street connections
    // will likely need to change if we want new plots to be created during simulation
    void connect_streets()
    {
        for (int level = 0; level < (GridLen + 1); level++)
        {
            for (int dir = 0; dir < 2; dir++)
            {
                Street curr_street = streets_list[level,dir];
                int other_dir = (dir == 0) ? 1 : 0;
                for (int j = 0; j < GridLen + 1; j++)
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
        // draw all square plots
        foreach (Plot p in plots_array)
        {
            if (p != null)
            {
                Vector2 midpoint = p.world_midpoint_coords();
                Rect p_rect = new Rect(midpoint.x - (0.5f * DrawScale),
                    midpoint.y - (0.5f * DrawScale),
                    DrawScale,
                    DrawScale);
                Draw.Rect(p_rect, p.color, -1);
            }
        }

        // draw all streets
        for (int level = 0; level < GridLen + 1; level++)
        {
            Rect hori_rect = new Rect(0, DrawScale * level, DrawScale * (GridLen + 0.1f), DrawScale * 0.1f);
            Rect vert_rect = new Rect(DrawScale * level, 0, DrawScale * 0.1f, DrawScale * (GridLen + 0.1f));
            Draw.Rect(vert_rect, Color.black);
            Draw.Rect(hori_rect, Color.black);
        }
    }
}