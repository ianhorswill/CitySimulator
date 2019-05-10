﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Provides static methods for drawing 2d lines and rectangles.
/// Add an instance of this component to your main camera, then call Draw.Line and Draw.Rect as you like.
/// </summary>
public class Draw : MonoBehaviour
{
    public Material Shader;
    private static Draw singleton;
    public GUIStyle TextStyle;

    public Draw()
    {
        singleton = this;
        textQueue = new PrimitiveQueue<TextInfo>(-1,
            textInfo =>
            {
                var screenPoint = Camera.current.WorldToScreenPoint(textInfo.Position);
                GUI.Label(new Rect(screenPoint.x, Screen.height-screenPoint.y, 0, 0),
                    textInfo.TextToDraw,
                    textInfo.Style);
            });
    }

    /// <summary>
    /// Fills in Shader, if not already set.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public void Awake()
    {
        if (Shader == null)
        {
            Shader = new Material(UnityEngine.Shader.Find("GUI/Text Shader"));
        }
    }

    // This is just for testing
    //public void Update()
    //{
    //    Text("foo", new Vector2(1, 1));
    //    Text("bar", new Vector2(1, -1));
    //    for (var x = -10; x <= 10; x += 5)
    //        Rect(new Rect(new Vector2(x / 10f, 0), new Vector2(0.1f, 0.1f)), new Color(05f + x / 20f, 0, 0.5f - x / 20f), -1);
    //    for (var y = -100; y <= 100; y += 5)
    //        Line(new Vector2(0, 0), new Vector2(1, y / 100.0f), new Color(05f + y / 200f, 0.5f - y / 200f, 0));
    //}

    // ReSharper disable once UnusedMember.Global
    public void OnGUI()
    {
        if (Event.current.type == EventType.Repaint)
            textQueue.DrawAll();
    }

    // ReSharper disable once UnusedMember.Global
    public void OnRenderObject()
    {
        Shader.SetPass(0);
        rectQueue.DrawAll();
        lineQueue.DrawAll();
    }

    readonly PrimitiveQueue<LineInfo> lineQueue = new PrimitiveQueue<LineInfo>(GL.LINES,
        lineInfo =>
        {
            GL.Color(lineInfo.Color);
            GL.Vertex3(lineInfo.Start.x, lineInfo.Start.y, lineInfo.Depth);
            GL.Vertex3(lineInfo.End.x, lineInfo.End.y, lineInfo.Depth);
        });

    /// <summary>
    /// Draw a line on the screen
    /// </summary>
    /// <param name="start">Starting point of the line</param>
    /// <param name="end">Endpoint of the line</param>
    /// <param name="color">Color of the line</param>
    /// <param name="depth">Z=depth of the line</param>
    public static void Line(Vector2 start, Vector2 end, Color color, float depth = 0)
    {
        singleton.lineQueue.Enqueue(new LineInfo(start, end, color, depth));
    }

    struct LineInfo
    {
        public readonly Vector2 Start, End;
        public readonly Color Color;
        public readonly float Depth;

        public LineInfo(Vector2 start, Vector2 end, Color color, float depth)
        {
            Start = start;
            End = end;
            Color = color;
            Depth = depth;
        }
    }

    readonly PrimitiveQueue<RectInfo> rectQueue = new PrimitiveQueue<RectInfo>(GL.QUADS,
        rectInfo =>
        {
            GL.Color(rectInfo.Color);
            GL.Vertex3(rectInfo.RectToDraw.xMin, rectInfo.RectToDraw.yMax, rectInfo.Depth);
            GL.Vertex3(rectInfo.RectToDraw.xMax, rectInfo.RectToDraw.yMax, rectInfo.Depth);
            GL.Vertex3(rectInfo.RectToDraw.xMax, rectInfo.RectToDraw.yMin, rectInfo.Depth);
            GL.Vertex3(rectInfo.RectToDraw.xMin, rectInfo.RectToDraw.yMin, rectInfo.Depth);

            //GL.Color(rectInfo.Color);
            //GL.Vertex3(rectInfo.Rect.xMin, rectInfo.Rect.yMin, rectInfo.Depth);
            //GL.Vertex3(rectInfo.Rect.xMax, rectInfo.Rect.yMin, rectInfo.Depth);
            //GL.Vertex3(rectInfo.Rect.xMax, rectInfo.Rect.yMax, rectInfo.Depth);
            //GL.Vertex3(rectInfo.Rect.xMin, rectInfo.Rect.yMax, rectInfo.Depth);

        });

    /// <summary>
    /// Draw a rectangle on the screen
    /// </summary>
    /// <param name="rect">Rectangle to draw</param>
    /// <param name="color">Color of the rectangle</param>
    /// <param name="depth">Z depth to draw it at</param>
    public static void Rect(Rect rect, Color color, float depth = 0)
    {
        singleton.rectQueue.Enqueue(new RectInfo(rect, color, depth));
    }

    struct RectInfo
    {
        public readonly Rect RectToDraw;
        public readonly Color Color;
        public readonly float Depth;

        public RectInfo(Rect rectToDraw, Color color, float depth)
        {
            RectToDraw = rectToDraw;
            Color = color;
            Depth = depth;
        }
    }

    readonly PrimitiveQueue<TextInfo> textQueue;

    /// <summary>
    /// Draw Text in the specified style
    /// </summary>
    /// <param name="text">String to display</param>
    /// <param name="position">Position in world coordinates</param>
    /// <param name="style">Unity GUIStyle for drawing it</param>
    public static void Text(string text, Vector2 position, [CanBeNull] GUIStyle style = null)
    {
        singleton.textQueue.Enqueue(new TextInfo(text, position, style??singleton.TextStyle));
    }

    struct TextInfo
    {
        public readonly string TextToDraw;
        public readonly Vector2 Position;
        public readonly GUIStyle Style;
        
        public TextInfo(string textToDraw, Vector2 position, GUIStyle style)
        {
            TextToDraw = textToDraw;
            Position = position;
            Style = style;
        }
    }

    class PrimitiveQueue<TPrimitiveInfo>
    {
        readonly Queue<TPrimitiveInfo> queue = new Queue<TPrimitiveInfo>();
        private readonly Action<TPrimitiveInfo> drawOperation;
        private readonly int drawMode;

        public PrimitiveQueue(int drawMode, Action<TPrimitiveInfo> drawOperation)
        {
            this.drawMode = drawMode;
            this.drawOperation = drawOperation;
        }

        public void Enqueue(TPrimitiveInfo p)
        {
            queue.Enqueue(p);
        }

        public void DrawAll()
        {
            if (queue.Count > 0)
            {
                if (drawMode>=0)
                    GL.Begin(drawMode);
                while (queue.Count > 0)
                    drawOperation(queue.Dequeue());
                if (drawMode >=0)
                    GL.End();
            }
        }
    }
}
