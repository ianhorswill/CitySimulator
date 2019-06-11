using System;
using UnityEngine;

public class GraphEdge : MonoBehaviour
{
    public GraphNode StartNode;
    public GraphNode EndNode;
    public string Label;
    public float Length = 35;
    public Color Color = Color.green;
    private TextMesh labelText;

    public static float Gain = 0.5f;

    public static Material Shader;

    public void Start()
    {
        labelText = gameObject.AddComponent<TextMesh>();
        labelText.text = Label;
        labelText.alignment = TextAlignment.Center;
        labelText.color = Color;
        if (Shader == null)
            Shader = new Material(UnityEngine.Shader.Find("GUI/Text Shader"));
    }

    public void Update()
    {
        var startPos = StartNode.transform.position;
        var endPos = EndNode.transform.position;
        var newPosition = 0.5f * (startPos + endPos);
        //transform.position = newPosition;
        var offset = endPos - startPos;
        var newRotation = Mathf.Atan2(offset.y, offset.x);
        //if (newRotation<0)
        //    newRotation = -newRotation;
        transform.SetPositionAndRotation(newPosition, Quaternion.Euler(0, 0 , Radians2Degrees(newRotation)));
        labelText.color = ProperColor;
    }

    float Radians2Degrees(float rad)
    {
        return rad * 180 / Mathf.PI;
    }

    public void FixedUpdate()
    {
        // Points from start to end
        var offset = EndNode.transform.position - StartNode.transform.position;
        var distance = offset.magnitude;
        // Positive means too close together, so push farther apart; zero means right distance
        var distanceError = Length - distance;
        // If distanceError positive, this pushes nodes apart.
        var forceVector = (distanceError * Gain) * offset.normalized;
        StartNode.rBody.AddForce(-forceVector);
        EndNode.rBody.AddForce(forceVector);
    }

    private Color ProperColor
    {
        get
        {
            var s = Graph.SelectedNode;
            return (s == null || StartNode == s || EndNode == s) ? Color : 0.5f * Color;
        }
    }

    public void OnRenderObject()
    {
        var startPos = StartNode.transform.position;
        var endPos = EndNode.transform.position;
        var unit = (endPos - startPos).normalized;
        // ReSharper disable once IdentifierTypo
        var perp = 0.2f* new Vector3(unit.y, -unit.x, 0);
        var corner1 = endPos - unit + perp;
        var corner2 = endPos - unit - perp;

        // Very inefficient
        Shader.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(ProperColor);
        // Body of arrow
        GL.Vertex(startPos);
        GL.Vertex(endPos);
        GL.End();

        GL.Begin(GL.TRIANGLES);
        GL.Color(ProperColor);
        // Arrowhead
        GL.Vertex(corner1);
        GL.Vertex(endPos);

        GL.Vertex(corner2);
        GL.End();
    }
}
