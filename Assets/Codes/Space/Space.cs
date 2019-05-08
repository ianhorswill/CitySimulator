//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : Object
{
    static public int MAX_PLOTS = 100;
    static public int MAX_STREETS = 1000;
    public Plot[,] PlotsList;
    public Street[,] StreetsList;
    float streetStretch = 1.505f;
    public int gridLen = 3;

    // Start is called before the first frame update
    public void Init()
    {
        PlotsList = new Plot[gridLen,gridLen];
        StreetsList = new Street[gridLen+1,2];

        // make starting plots and streets
        for (int y = 0; y < gridLen; y++)
        {
            for (int x = 0; x < gridLen; x++)
            {
                PlotsList[x,y] = MakePlot(x+x, y+y);
            }
        }

        for (int dir = 0; dir < 2; dir++) // 0 : N and 1 : E
        {
            for (int level = 0; level < gridLen + 1; level++)
            {
                StreetsList[level,dir] = MakeStreet(level, dir);
            }
        }

        setupPlotNeighbors();
        setupStreetConnections();
    }


    Plot MakePlot(int x, int y)
    {
        Plot newPlot = new Plot(x, y, this);

        return newPlot;
    }

    Street MakeStreet(int lvl, int dir)
    {
        string dir_name = dir == 0 ? "N. " : "E. ";
        Street new_street = new Street(dir, dir_name + lvl + " Street");

        return new_street;
    }

    // hard-coded method for setting up plot neighbors
    // will likely need to change if we want new plots to be created during simulation
    void setupPlotNeighbors()
    {
        for(int x = 0; x < gridLen; x++)
        {
            for (int y = 0; y < gridLen; y++)
            {
                Plot curr_plot = PlotsList[x, y];
                Plot north = (y == gridLen - 1) ? null : PlotsList[x, y + 1];
                Plot east = (x == gridLen - 1) ? null : PlotsList[x + 1, y];
                Plot south = (y == 0) ? null : PlotsList[x, y - 1];
                Plot west = (x == 0) ? null : PlotsList[x-1, y];
                curr_plot.neighbor_plots[0] = north;
                curr_plot.neighbor_plots[1] = east;
                curr_plot.neighbor_plots[2] = south;
                curr_plot.neighbor_plots[3] = west;
            }
        }
    }

    // hard-coded method for setting street connections
    // will likely need to change if we want new plots to be created during simulation
    void setupStreetConnections()
    {
        for (int level = 0; level < (gridLen + 1); level++)
        {
            for (int dir = 0; dir < 2; dir++)
            {
                Street currStreet = StreetsList[level,dir];
                int other_dir = (dir == 0) ? 1 : 0;
                for (int j = 0; j < gridLen + 1; j++)
                {
                    currStreet.connected_streets[j] = StreetsList[j,other_dir];
                }
            }
        }
        
    }

    int Pos2Idx(int x, int y)
    {
        return gridLen * y + x;
    } 
}
