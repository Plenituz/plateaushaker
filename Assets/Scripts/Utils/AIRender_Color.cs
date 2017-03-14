using UnityEngine;
using System.Collections.Generic;

public class AIRender_Color : AIRenderer {

    Color GetRandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        return new Color(r, g, b, 1f);
    }

    protected override Shape[] ReadShapes()
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
            //string[] col = colPoints[0].Split(':');
            string[] points = colPoints[1].Split('}');
            sha.color = GetRandomColor();//new Color(float.Parse(col[0]) / 255f, float.Parse(col[1]) / 255f, float.Parse(col[2]) / 255f);

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
