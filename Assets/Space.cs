using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    static public int MAX_STARTING_PLOTS = 9;
    static public int MAX_STARTING_STREETS = 8;
    static public int MAX_PLOTS = 9;
    static public int MAX_STREETS = 8;
    public GameObject[] PlotsList;
    public GameObject[] StreetsList;
    int gridLen = 3;

    // Start is called before the first frame update
    void Start()
    {
        PlotsList = new GameObject[MAX_PLOTS];
        StreetsList = new GameObject[MAX_STREETS];
        // make starting plots and streets
        for (int y = 0; y < gridLen; y++)
        {
            for (int x = 0; x < gridLen; x++)
            {
                int idx = (gridLen * y) + x;
                PlotsList[idx] = MakePlot(x, y, idx);
            }
        }

        for (int dir = 0; dir < 2; dir++) 
        {
            for (int lvl = 0; lvl < gridLen + 1; lvl++)
            {
                StreetsList[((gridLen + 1) * dir) + lvl] = MakeStreet(lvl, dir);
            }
        }

        setupPlotNeighbors();
        setupStreetConnections();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject MakePlot(int x, int y, int idx)
    {
        GameObject newPlotObj = new GameObject("Plot" + idx);
        newPlotObj.transform.parent = gameObject.transform;
        Plot newPlot = newPlotObj.AddComponent<Plot>();
        newPlot.x_pos = x;
        newPlot.y_pos = y;
        return newPlotObj;
    }

    GameObject MakeStreet(int lvl, int dir)
    {
        string dir_name = dir == 0 ? "North" : "East";
        GameObject newStreetObj = new GameObject("Street " + dir_name + lvl);
        newStreetObj.transform.parent = gameObject.transform;
        Street newStreet = newStreetObj.AddComponent<Street>();
        newStreet.direction = dir;
        return newStreetObj;
    }

    // hard-coded method for setting up plot neighbors
    // will likely need to change if we want new plots to be created during simulation
    void setupPlotNeighbors()
    {
        for(int i = 0; i < gridLen * gridLen; i++)
        {
            GameObject[] currNeighbors = PlotsList[i].GetComponent<Plot>().neighbor_plots;
            int curX = i / gridLen;
            int curY = i % gridLen;
            GameObject north = (curY == gridLen - 1) ? null : PlotsList[Pos2Idx(curX, curY + 1)];
            GameObject east = (curX == gridLen - 1) ? null : PlotsList[Pos2Idx(curX + 1, curY)];
            GameObject south = (curY == 0) ? null : PlotsList[Pos2Idx(curX, curY - 1)];
            GameObject west = (curX == 0) ? null : PlotsList[Pos2Idx(curX - 1, curY)];
            currNeighbors[0] = north;
            currNeighbors[1] = east;
            currNeighbors[2] = south;
            currNeighbors[3] = west;
        }
    }

    // hard-coded method for setting street connections
    // will likely need to change if we want new plots to be created during simulation
    void setupStreetConnections()
    {
        for (int i = 0; i < 2 * (gridLen + 1); i++)
        {
            Street currStreet = StreetsList[i].GetComponent<Street>();
            int perp_streets_idx = (i < gridLen + 1) ? gridLen + 1 : 0;
            for (int j = 0; j < gridLen + 1; j++)
            {
                currStreet.connected_streets[j] = StreetsList[perp_streets_idx];
                perp_streets_idx++;
            }
        }
    }

    int Pos2Idx(int x, int y)
    {
        return gridLen * y + x;
    } 
}
