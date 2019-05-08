using System.Collections;
using System.Collections.Generic;
using System;

public class Plot : Object
{
    public int x_pos, y_pos; // x, y coords
    public Plot[] neighbor_plots = new Plot[4]; // stored N,E,S,W
    public bool occupied;
    public List<Institution.Institution> institutions = new List<Institution.Institution>();
    private Space space;

    public Plot(int x, int y, Space the_space)
    {
        x_pos = x;
        y_pos = y;
        space = the_space;
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

    public List<Institution.Institution> get_nearby_institutions()
    {
        List<Institution.Institution> nearby_institutions = institutions;
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
        return new List<Street> { space.StreetsList[x_pos, 0],
                                  space.StreetsList[x_pos + 1, 0],
                                  space.StreetsList[y_pos, 1],
                                  space.StreetsList[y_pos + 1, 1]};
    }
}
