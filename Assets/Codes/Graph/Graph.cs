using System;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public static Graph Singleton;
    public static GraphNode SelectedNode;

    readonly Dictionary<string, GraphNode> nodes = new Dictionary<string, GraphNode>();
    public Rect ScreenInWorldCoordinates;

    public static void ConstraintToScreen(Rigidbody2D r)
    {
        var screen = Singleton.ScreenInWorldCoordinates;
        var p = r.position;
        var changed = false;
        if (p.x > screen.xMax)
        {
            p.x = screen.xMax;
            changed = true;
        }

        if (p.x < screen.xMin)
        {
            p.x = screen.xMin;
            changed = true;
        }

        if (p.y > screen.yMax)
        {
            p.y = screen.yMax;
            changed = true;
        }

        if (p.y < screen.yMin)
        {
            p.y = screen.yMin;
            changed = true;
        }

        if (changed)
            r.MovePosition(p);
    }

    public static void Create()
    {
        if (Singleton != null)
            Singleton.Exit();
        var go = new GameObject("Graph visualization");
        Singleton = go.AddComponent<Graph>();
    }

    public static void AddNode(string name)
    {
        Singleton.FindNode(name);
    }

    public static void SetColor(string nodeName, string color)
    {
        Singleton.FindNode(nodeName).SetColor(ColorNamed(color));
    }

    private GraphNode FindNode(string nodeName)
    {
        if (nodes.ContainsKey(nodeName))
            return nodes[nodeName];

        var child = new GameObject(nodeName);
        child.transform.parent = gameObject.transform;
        
        return nodes[nodeName] = child.AddComponent<GraphNode>();
    }

    public static void AddEdge(string from, string to, string label, string color)
    {
        Singleton.MakeEdge(from, to, label, ColorNamed(color));
    }

    static Color ColorNamed(string name)
    {
        switch (name)
        {
            case "red": return Color.red;
            case "green": return Color.green;
            case "blue": return Color.blue;
            case "white": return Color.white;
            case "black": return Color.black;
            case "gray":
                case "grey":
                return Color.gray;
            case "cyan": return Color.cyan;
            case "magenta": return Color.magenta;
            case "yellow": return Color.yellow;
            default:
                throw new ArgumentException($"Unknown color {name}");
        }
    }

    private void MakeEdge(string from, string to, string label, Color c)
    {
        var child = new GameObject($"{from}->{to}");
        child.transform.parent = gameObject.transform;
        var edge = child.AddComponent<GraphEdge>();
        edge.StartNode = FindNode(from);
        edge.EndNode = FindNode(to);
        edge.Label = label;
        edge.Color = c;
    }

    // Start is called before the first frame update
    internal void Start()
    {
        FindObjectOfType<Draw>().Visible = false;
        FindObjectOfType<SimulatorDriver>().Visible = false;
        Singleton = this;
        var lowerLeft = Camera.main.ScreenToWorldPoint(new Vector2(50, 50));
        var upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width-50, Screen.height-50, 0));
        var difference = upperRight - lowerLeft;
        var middle = lowerLeft + 0.5f * difference;
        ScreenInWorldCoordinates = new Rect(lowerLeft, difference);
    }

    internal void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            Exit();
    }

    private void Exit()
    {
        FindObjectOfType<Draw>().Visible = true;
        FindObjectOfType<SimulatorDriver>().Visible = true;
        Singleton = null;
        Destroy(gameObject);
    }
}
