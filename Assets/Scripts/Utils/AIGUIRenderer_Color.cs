using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIGUIRenderer_Color : AIGUIRenderer
{

    Color GetRandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        return new Color(r, g, b, 1f);
    }

    protected override Shape[] ReadShapes(out Vector2[] colliderPoints)
    {
        List<Shape> shapes = new List<Shape>();

        string all = file.text;
        string[] sep1 = all.Split('$');

        string[] collPoints = sep1[1].Split('}');
        List<Vector2> ccollPointsVect = new List<Vector2>();
        for (int i = 0; i < collPoints.Length; i++)
        {
            string[] xy = collPoints[i].Split('!');
            float x = float.Parse(xy[0]);
            float y = float.Parse(xy[1]);
            ccollPointsVect.Add(new Vector2(x, y));
        }
        colliderPoints = ccollPointsVect.ToArray();

        string[] shapeList = sep1[0].Split('*');
        for (int i = 0; i < shapeList.Length; i++)
        {
            Shape sha = new Shape();
            string[] colPoints = shapeList[i].Split('a');
            //string[] col = colPoints[0].Split(':');
            string[] points = colPoints[1].Split('}');
            //sha.color = new Color(float.Parse(col[0]) / 255f, float.Parse(col[1]) / 255f, float.Parse(col[2]) / 255f);
            sha.color = GetRandomColor();

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
}
