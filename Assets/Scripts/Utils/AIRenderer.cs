using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Shape
{
    public Vector2[] points;
    public Color color = Color.white;

    public Vector2 CalculateCenter()
    {
        Vector2 center = Vector2.zero;
        for (int i = 0; i < points.Length; i++)
        {
            center += points[i];
        }
        center /= points.Length;

        return center;
    }
}

[ExecuteInEditMode]
public class AIRenderer : MonoBehaviour {
    protected List<Vector3[]> verts = new List<Vector3[]>();
    protected List<Vector3[]> vertsScaled = new List<Vector3[]>();
    protected List<Color> cols = new List<Color>();
    protected Vector3 basePos = new Vector3(564512f, 1321f, 5456);
    protected Vector3 baseScale = new Vector3(5645645f, 231321f, 7847f);
    protected Quaternion baseRot = Quaternion.identity;
    protected Material mat;

    public Color color = Color.white;
    public string shapePath = "Icon";
    public float lockRatio = 1f;
    public bool invertCull = false;
    public AIRendererCam cameraAI;
    public int orderInLayer = 0;

    void Awake()
    {
        //populate the meshfilter
        CreateMat();
        OnPopulateMesh();
        if (cameraAI == null)
            cameraAI = Camera.main.GetComponent<AIRendererCam>();
        cameraAI.Add(RenderObject, orderInLayer);
    }

    void OnDisable()
    {
        cameraAI.Remove(orderInLayer, RenderObject);
    }

    void OnDestroy()
    {
        OnDisable();
    }

    void CreateMat()
    {
        if (mat == null)
        {
            Shader shader = Shader.Find("Unlit/UnlitVert");
            mat = new Material(shader);
            mat.SetColor("_Color", color);
        }
    }

    public void SetColor(Color col)
    {
        CreateMat();
        color = col;
        mat.SetColor("_Color", col);
    }

    void OnDrawGizmos()
    {
        //RenderObject();
    }

    public void RenderObject()
    {
        if(transform.position != basePos || transform.localScale != baseScale || transform.rotation != baseRot)
        {
            ScaleVerts();
            basePos = transform.position;
            baseScale = transform.localScale;
            baseRot = transform.rotation;
        }

        mat.SetPass(0);
        GL.invertCulling = invertCull;

        GL.PushMatrix();
        GL.Begin(GL.TRIANGLES);
       
        
        for (int i = 0; i < vertsScaled.Count; i++)
        {
            Vector3[] vs = vertsScaled[i];
            GL.Color(cols[i]);
            for (int k = 0; k < vs.Length; k++)
            {
                Vector3 v = vs[k];
                GL.Vertex(v);
              //  Debug.DrawLine(v, v + new Vector3(1f, 0f), Color.white);
            }
        }
        GL.End();
        GL.PopMatrix();
        GL.invertCulling = false;
    }

    protected virtual Shape[] ReadShapes()
    {
        List<Shape> shapes = new List<Shape>();

        TextAsset f = Resources.Load<TextAsset>(shapePath);

        //string all = File.ReadAllText("Assets/Resources/" + shapePath + ".txt");
        string all = f.text;
        string[] sep1 = all.Split('$');

        string[] shapeList = sep1[0].Split('*');
        for (int i = 0; i < shapeList.Length; i++)
        {
            Shape sha = new Shape();
            string[] colPoints = shapeList[i].Split('a');
            string[] col = colPoints[0].Split(':');
            string[] points = colPoints[1].Split('}');
            sha.color = new Color(float.Parse(col[0]) / 255f, float.Parse(col[1]) / 255f, float.Parse(col[2]) / 255f);

            List<Vector2> pointsList = new List<Vector2>();
            for (int k = 0; k < points.Length; k++)
            {
                string[] xy = points[k].Split('!');
                Vector2 v = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
                pointsList.Add(v);
            }
            sha.points = pointsList.ToArray();
            shapes.Add(sha);
        }

        shapes.Reverse();
        return shapes.ToArray();
    }

    protected Vector3 SizeVectorForRect(Vector3 v, Vector3 p, Vector3 scale, Quaternion rotation)
    {
        //place point
        v.x += p.x / scale.x;
        v.y += p.y / (scale.y * lockRatio);
        v.z = p.z;
        //scale point
        v.x *= scale.x;
        v.y *= scale.y * lockRatio;
        //rotate point
        v = RotatePointAroundPivot(v, p, rotation);
        return v;
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 position, Quaternion rotation)
    {
        Vector3 direction = point - position; // get point direction relative to pivot
        direction = rotation * direction; // rotate it
        point = direction + position; // calculate rotated point
        return point; // return it
    }

protected void ScaleVerts()
    {
        vertsScaled.Clear();
        Vector3 position = transform.position;
        Vector3 scale = transform.localScale;
        Quaternion rotation = transform.rotation;

        for (int i = 0; i < verts.Count; i++)
        {
            Vector3[] current = verts[i];
            Vector3[] scaled = new Vector3[current.Length];
            for(int k = 0; k < current.Length; k++)
            {
                scaled[k] = SizeVectorForRect(current[k], position, scale, rotation);
            }
            vertsScaled.Add(scaled);
        }
    }

    protected virtual void OnPopulateMesh()
    {
        Shape[] shapes = ReadShapes();

        Vector2 center;
        Vector2 p1;
        Vector2 p2;
        
        for (int s = 0; s < shapes.Length; s++)
        {
            center = shapes[s].CalculateCenter();
            //center = SizeVectorForRect(center);

            for (int i = 0; i < shapes[s].points.Length; i++)
            {
                p1 = /*SizeVectorForRect(*/shapes[s].points[i];//);
                if (i == shapes[s].points.Length - 1)
                    p2 = shapes[s].points[0];
                else
                    p2 = shapes[s].points[i + 1];
                //p2 = SizeVectorForRect(p2);

               // Debug.DrawLine(p1, p1 + Vector2.left * 50f, Color.red, 20f);
                verts.Add(new Vector3[] { center, p1, p2 });
                cols.Add(shapes[s].color);
            }
        }
    }
}
