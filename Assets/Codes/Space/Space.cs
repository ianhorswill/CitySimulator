using System.Collections.Generic;
using UnityEngine;
using System;

public class Space : SimulatorComponent
{
	private const int GridLen = 10;
	private const float FullFactor = 0.5f;
	private static int MaxPlots = 0;
	private static int NumPlots = 0;
	
	public const float DrawScale = 2.0f;
	
	public Plot[,] PlotsArray;
	public Street[,] StreetsList;
	readonly List<Plot> emptyPlots = new List<Plot>();
	readonly List<Plot> occupiedPlots = new List<Plot>();
	
	public static Space Singleton;
	
	public Space(){ Singleton = this; }

	// Initialize is called before the first frame update
	public override void Initialize()
	{
		PlotsArray = new Plot[GridLen, GridLen];
		StreetsList = new Street[GridLen+1,2];

		calc_max_plots();

		generate_plots();
		generate_streets();

		connect_streets();
	}

	private void calc_max_plots()
	{
		MaxPlots = (int) ((GridLen * GridLen) * FullFactor);
	}

	public override void Visualize()
	{
		// draw all square plots
		foreach (Plot p in PlotsArray)
		{
			if (p == null) continue;
			Vector2 midpoint = p.world_midpoint_coords();
			Rect p_rect = new Rect(midpoint.x - (0.5f * DrawScale),
				midpoint.y - (0.5f * DrawScale),
				DrawScale,
				DrawScale);
			Draw.Rect(p_rect, p.color, -1);
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

	private void generate_plots()
	{
		int x = Random.Integer(0, GridLen);
		int y = Random.Integer(0, GridLen);
		
		Sprawl(make_plot(x, y));
	}

	private void Sprawl(Plot current)
	{
		int x, y;

		int childX = x = current.x_pos;
		int childY = y = current.y_pos;

		for(int i = 0; i < 4; i++)
		{
			Plot p = current.neighbor_plots[i];

			if (p == null)
			{
				switch (i)
				{
					// North
					case 0:
						childX = x;
						childY = (y == GridLen - 1) ? -1 : y + 1;
						current.neighbor_plots[i] = make_plot(childX, childY);
						break;
					
					// East
					case 1:
						childX = (x == GridLen - 1) ? -1 : x + 1;
						childY = y;
						current.neighbor_plots[i] = make_plot(childX, childY);
						break;
					
					// South
					case 2:
						childX = x;
						childY = (y == 0) ? -1 : y - 1;
						current.neighbor_plots[i] = make_plot(childX, childY);
						break;
					
					// West
					case 3:
						childX = (x == 0) ? -1 : x - 1;
						current.neighbor_plots[i] = make_plot(childX, childY);
						childY = y;
						break;
					
					default:
						Debug.Log("uh oh spaghettio you shouldn't be here");
						break;
				}
			}
		}
		
		if (NumPlots < MaxPlots) Sprawl(PickRandomNeighbor(current));
	}

	public Plot PickRandomNeighbor(Plot center)
	{
		return center.neighbor_plots[Random.Integer(0, 4)] ?? PickRandomNeighbor(center);

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
				StreetsList[level, dir] = make_street(level, street_dir);
			}
		}
	}

	public Plot get_random_plot()
	{
		int x = Random.Integer(0, GridLen);
		int y = Random.Integer(0, GridLen);

		return PlotsArray[x,y] ?? get_random_plot();
	}

	/// <summary>
	/// Returns a random plot with no institutions on it. Returns null if no 
	/// empty plots.
	/// </summary>
	public Plot get_random_empty_plot()
	{
		if (emptyPlots.Count == 0)
		{
			return null;
		}
		return Random.RandomElement(emptyPlots);
	}

	/// <summary>
	/// Returns the plot with the highest value when evaluated by plot_eval.
	/// If there is a tie, the first-considered tied plot is always returned
	/// </summary>
	public Plot get_best_plot(Func<Plot, float> plot_eval)
	{
		Plot best_plot = null;
		float best_plot_val = 0;
		foreach(Plot p in PlotsArray)
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
		if (x < 0 || y < 0) return null;
		
		Plot newPlot = new Plot(x, y);
		
		PlotsArray[x, y] = newPlot;
		emptyPlots.Add(newPlot);
		
		NumPlots++;

		return newPlot;

	}

	Street make_street(int lvl, Street.street_direction dir)
	{
		string dir_name = dir == Street.street_direction.NS ? "N. " : "E. ";
		Street new_street = new Street(dir, dir_name + lvl + " Street");

		return new_street;
	}

	// hard-coded method for setting street connections
	// will likely need to change if we want new plots to be created during simulation
	void connect_streets()
	{
		for (int level = 0; level < (GridLen + 1); level++)
		{
			for (int dir = 0; dir < 2; dir++)
			{
				Street curr_street = StreetsList[level,dir];
				int other_dir = (dir == 0) ? 1 : 0;
				for (int j = 0; j < GridLen + 1; j++)
				{
					curr_street.connected_streets.Add(StreetsList[j,other_dir]);
				}
			}
		}
		
	}

	public void mark_occupied(Plot p)
	{
		emptyPlots.Remove(p);
		occupiedPlots.Add(p);
	}

	public void mark_unoccupied(Plot p)
	{
		occupiedPlots.Remove(p);
		emptyPlots.Add(p);
	}
}