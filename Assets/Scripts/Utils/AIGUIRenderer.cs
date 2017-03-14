using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[AddComponentMenu("UI/AIGUIRenderer")]
[ExecuteInEditMode]
public class AIGUIRenderer : Graphic
{
    public class Shape
    {
        public Vector2[] points;
        public Color color;

        public Vector2 CalculateCenter()
        {
            Vector2 center = Vector2.zero;
            for(int i = 0; i < points.Length; i++)
            {
                center += points[i];
            }
            center /= points.Length;
            
            return center;
        }
    }

    public TextAsset file;
    public float lockRatio = 0f;
    public bool lockToWidth = false;

    private PolygonCollider2D clickCollider;

    protected virtual Shape[] ReadShapes(out Vector2[] colliderPoints)
    {
        List<Shape> shapes = new List<Shape>();

        string all = file.text;
        string[] sep1 = all.Split('$');

        string[] collPoints = sep1[1].Split('}');
        List<Vector2> ccollPointsVect = new List<Vector2>();
        for(int i = 0; i < collPoints.Length; i++)
        {
            string[] xy = collPoints[i].Split('!');
            float x = float.Parse(xy[0]);
            float y = float.Parse(xy[1]);
            ccollPointsVect.Add(new Vector2(x, y));
        }
        colliderPoints = ccollPointsVect.ToArray();

        string[] shapeList = sep1[0].Split('*');
        for(int i = 0; i < shapeList.Length; i++)
        {
            Shape sha = new Shape();
            string[] colPoints = shapeList[i].Split('a');
            string[] col = colPoints[0].Split(':');
            string[] points = colPoints[1].Split('}');
            sha.color = new Color(float.Parse(col[0]) /255f, float.Parse(col[1])/255f, float.Parse(col[2])/255f);

            List<Vector2> pointsList = new List<Vector2>();
            for(int k = 0; k < points.Length; k++)
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

    protected UIVertex[] SetVbo(Vector2[] vertices, Color color)
    {
        UIVertex[] vbo = new UIVertex[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vert = UIVertex.simpleVert;
            vert.color = color * this.color;
            vert.position = vertices[i];
           // vert.uv0 = uvs[i];
            vbo[i] = vert;
        }
        return vbo;
    }

    protected Vector2 SizeVectorForRect(Vector2 v)
    {
        Vector2 pivot = rectTransform.pivot;
        Rect rect = rectTransform.rect;

        v.x -= pivot.x;
        v.y -= pivot.y*-1 * lockRatio;

        v.x *= rect.width;
        if(lockToWidth)
            v.y *= rect.width * lockRatio;
        else
            v.y *= rect.height * lockRatio;
        return v;
    }

    protected Vector2[] SizeAllVectors(Vector2[] vects)
    {
        for(int i = 0; i < vects.Length; i++)
        {
            vects[i] = SizeVectorForRect(vects[i]);
        }
        return vects;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Vector2[] collPoints;
        Shape[] shapes = ReadShapes(out collPoints);
        vh.Clear();

        if (raycastTarget)
        {
            if (clickCollider == null)
            {
                clickCollider = gameObject.GetComponent<PolygonCollider2D>();
                if (clickCollider == null)
                    clickCollider = gameObject.AddComponent<PolygonCollider2D>();

                clickCollider.points = SizeAllVectors(collPoints);
                // CleanCollider();
            }
        }
        else
        {
            DestroyImmediate(GetComponent<PolygonCollider2D>());
        }
        

        Vector2 center;
        Vector2 p1;
        Vector2 p2;
        List<UIVertex> verts = new List<UIVertex>();
        for (int s = 0; s < shapes.Length; s++)
        {
            center = shapes[s].CalculateCenter();
            center = SizeVectorForRect(center);

            for(int i = 0; i < shapes[s].points.Length; i++)
            {
                p1 = SizeVectorForRect(shapes[s].points[i]);
                if (i == shapes[s].points.Length - 1)
                    p2 = shapes[s].points[0];
                else
                    p2 = shapes[s].points[i + 1];
                p2 = SizeVectorForRect(p2);

                //   Debug.DrawLine(p1, p1 + Vector2.left * 50f, Color.red, 20f);
                verts.AddRange(SetVbo(new Vector2[] { center, p1, p2 }, shapes[s].color));
                //vh.AddUIVertexQuad(SetVbo(new Vector2[] { center, center, p1, p2 }, shapes[s].color));
            }          
        }
        vh.AddUIVertexTriangleStream(verts);
    }

    public override bool Raycast(Vector2 sp, Camera eventCamera)
    {
        return clickCollider.OverlapPoint(sp);
    }
}
