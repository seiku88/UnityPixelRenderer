using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Abstract Sprite/Sprite Part Ellipse")]
public class AbstractSpriteEllipse : AbstractSprite
{
    public Color colorBorder;
    public Color colorCenter;

    private SpriteVertex[] ellipseVertices;

    void Start()
    {
        if (gameObject.transform.childCount == 0)
        {
            isInitialized = false;

            points = new List<SpriteVertex>();
            pointIndex = new List<int>();
            ellipseVertices = new SpriteVertex[17];
            GameObject[] vertObjects = new GameObject[16];

            for (int i = 0; i < 17; i++)
            {
                if (i == 0) ellipseVertices[0].position = gameObject.transform;
                else
                {
                    vertObjects[i - 1] = new GameObject();
                    vertObjects[i - 1].name = i.ToString();
                    ellipseVertices[i].position = vertObjects[i - 1].transform;
                    ellipseVertices[i].position.parent = gameObject.transform;
                }
            }

            for (int i = 1; i <= vertObjects.Length; i++)
            {
                ellipseVertices[i].position.localPosition = new Vector3(Mathf.Cos(((360.0f / 16.0f) * (i-1))*Mathf.Deg2Rad), Mathf.Sin(((360.0f / 16.0f) * (i - 1)) * Mathf.Deg2Rad), 0);

                pointIndex.Add(0);
                pointIndex.Add(i);
                if (i < vertObjects.Length) pointIndex.Add((i+1));
                else pointIndex.Add(1);
            }

            for (int i = 0; i < ellipseVertices.Length; i++)
            {
                if (i > 0) ellipseVertices[i].col = colorBorder;
                else ellipseVertices[i].col = colorCenter;
                points.Add(ellipseVertices[i]);
            }

            isInitialized = true;
        }
    }

    public override void Draw(Mesh targetMesh, Vector3 basePos)
    {
        lateColor = true;
        base.Draw(targetMesh, basePos);

        List<Color> colors = new List<Color>();
        if (targetMesh.vertexCount > 0) targetMesh.GetColors(colors);

        for (int v = 0; v < points.Count; v++)
        {
            if (colors.Count == targetMesh.vertexCount)
            {
                if (v < points.Count - 1) colors[Mathf.Clamp(targetMesh.vertexCount - v - 1, 0, targetMesh.vertexCount)] = colorBorder;
                else colors[targetMesh.vertexCount - v - 1] = colorCenter;
            }
            else
            {
                if (v < points.Count - 1) colors.Add(colorBorder);
                else colors.Add(colorCenter);
            }
        }
        targetMesh.SetColors(colors);
    }
}
