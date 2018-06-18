using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Abstract Sprite/Sprite Part Rectangle (Long)")]
public class AbstractSpriteLongRectangle : AbstractSprite
{
    public int segmentCount = 6;
    public Color rectColor;

    private SpriteVertex[] rectVertices;

    // Use this for initialization
    void Start()
    {
        if (gameObject.transform.childCount == 0)
        {
            isInitialized = false;

            points = new List<SpriteVertex>();
            pointIndex = new List<int>();
            rectVertices = new SpriteVertex[(segmentCount + 1) * 2];
            GameObject[] vertObjects = new GameObject[(segmentCount+1)*2];

            for (int i = 0; i < ((segmentCount + 1) * 2); i++)
            {
                vertObjects[i] = new GameObject();
                vertObjects[i].name = i.ToString();
                rectVertices[i].position = vertObjects[i].transform;
                rectVertices[i].position.parent = gameObject.transform;
            }

            for (int i = 0; i < vertObjects.Length; i++)
            {
                if (i < vertObjects.Length / 2) rectVertices[i].position.localPosition = new Vector3((1.0f / segmentCount) * i, 0.5f * (1.0f / segmentCount), 0);
                else rectVertices[i].position.localPosition = new Vector3((1.0f / segmentCount) * (i - segmentCount), -0.5f * (1.0f / segmentCount), 0);
            }

            for (int i = 0; i < rectVertices.Length; i++)
            {
                rectVertices[i].col = rectColor;
                points.Add(rectVertices[i]);
            }

            for (int i = 0; i < segmentCount; i++)
            {
                pointIndex.Add(i);
                pointIndex.Add(i + segmentCount + 2);
                pointIndex.Add(i + segmentCount + 1);
                pointIndex.Add(i);
                pointIndex.Add(i + 1);
                pointIndex.Add(i + segmentCount + 2);
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
                colors[Mathf.Clamp(targetMesh.vertexCount - v - 1, 0, targetMesh.vertexCount)] = rectColor;
            }
            else
            {
                colors.Add(rectColor);
            }
        }
        targetMesh.SetColors(colors);
    }
}