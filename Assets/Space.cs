using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    static public int MAX_PLOTS = 100;
    static public int MAX_STREETS = 1000;
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
                PlotsList[idx] = MakePlot(x+x, y+y, idx);

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
        newPlotObj.AddComponent<SpriteRenderer>();
        newPlotObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("grass03");

        Plot newPlot = newPlotObj.AddComponent<Plot>();
        newPlotObj.transform.position = new Vector3(x, y, 0);
        newPlotObj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        return newPlotObj;
    }

    GameObject MakeStreet(int lvl, int dir)
    {
        string dir_name = dir == 0 ? "N." : "E. ";

        GameObject newStreetObj = new GameObject(dir_name + lvl + " Street");
        newStreetObj.transform.parent = gameObject.transform;
        newStreetObj.AddComponent<SpriteRenderer>();
        newStreetObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("gravel");

        Street newStreet = newStreetObj.AddComponent<Street>();
        Vector3 posRef = PlotsList[lvl + gridLen].transform.position;
        newStreetObj.transform.position = new Vector3(-1, 0, 0) + posRef;
        newStreetObj.transform.localScale = new Vector3(0.56f, 4.20f, 1f);


        if (dir == 1)
        {
            //Temporary fix for placing final E. street
            if (lvl == gridLen)
            {
                posRef = PlotsList[(lvl * gridLen) - gridLen + 1].transform.position;
                newStreetObj.transform.position = new Vector3(0, 1, 0) + posRef;
                newStreetObj.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                newStreet.direction = dir;
                return newStreetObj;
            }

            newStreetObj.transform.localRotation = Quaternion.Euler(0, 0, 90f);
            posRef = PlotsList[(lvl * gridLen) + 1].transform.position;
            newStreetObj.transform.position = new Vector3(0, -1, 0) + posRef;

        }

        //Temporary fix for placing final N. street
        if(lvl == gridLen)
        {
            Debug.Log(lvl + gridLen);
            Debug.Log(((lvl * 2) - 1));
            posRef = PlotsList[(lvl * 2) - 1].transform.position;
            newStreetObj.transform.position = new Vector3(1, 0, 0) + posRef;
            newStreetObj.transform.localScale = new Vector3(0.56f, 4.20f, 1f);
        }

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
