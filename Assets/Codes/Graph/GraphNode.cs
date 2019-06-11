
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    public Rigidbody2D rBody;
    private TextMesh textMesh;
    private BoxCollider2D hitBox;

    public Color Color = Color.white;

    private bool mouseDrag;

    // ReSharper disable once UnusedMember.Local
    private void Start()
    {
        rBody = gameObject.AddComponent<Rigidbody2D>();
        rBody.gravityScale = 0;
        rBody.drag = 0.75f;
        rBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        textMesh = gameObject.AddComponent<TextMesh>();
        textMesh.alignment = TextAlignment.Center;
        hitBox = gameObject.AddComponent<BoxCollider2D>();
        hitBox.size = new Vector2(5,5);
        textMesh.text = gameObject.name;
        textMesh.color = Color;
        gameObject.transform.position = new Vector3(Random.Float(-10, 30), Random.Float(-10, 30));    
    }

    public void SetColor(Color c)
    {
        Color = c;
        if (textMesh != null)
            textMesh.color = c;
    }

    public void OnGUI()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (hitBox.OverlapPoint(pos))
            {
                mouseDrag = true;
                Graph.SelectedNode = this;
            }
        }
        else if (Event.current.type == EventType.MouseUp)
        {
            mouseDrag = false;
            Graph.SelectedNode = null;
        }
    }

    public void FixedUpdate()
    {
        Graph.ConstraintToScreen(rBody);

        if (mouseDrag)
            rBody.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
