using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Abstract Sprite/Sprite Part Triangle")]
public class AbstractSpriteTriangle : AbstractSprite
{
    public Color[] triColors;

    private SpriteVertex[] triVertices;

    // Use this for initialization
    void Start()
    {
        if (gameObject.transform.childCount == 0)
        {
            isInitialized = false;

            triVertices = new SpriteVertex[3];
            GameObject[] vertObjects = new GameObject[3];
            triColors = new Color[3];

            for (int i = 0; i < 3; i++)
            {
                vertObjects[i] = new GameObject();
                vertObjects[i - 1].name = i.ToString();
                triVertices[i].position = vertObjects[i].transform;
                triVertices[i].position.parent = gameObject.transform;
            }

            triVertices[0].position.localPosition = new Vector3(0, 1, 0);
            triVertices[1].position.localPosition = new Vector3(Mathf.Sqrt(3) * 0.5f, -0.5f, 0);
            triVertices[2].position.localPosition = new Vector3(-Mathf.Sqrt(3) * 0.5f, -0.5f, 0);

            for (int i = 0; i < triVertices.Length; i++)
            {
                triVertices[i].col = triColors[i];
                points.Add(triVertices[i]);
            }

            int[] tmpindex = new int[3] { 0, 1, 2 };
            pointIndex.AddRange(tmpindex);

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
                colors[Mathf.Clamp(targetMesh.vertexCount - v - 1, 0, targetMesh.vertexCount)] = triColors[v];
            }
            else
            {
                colors.Add(triColors[v]);
            }
        }
        targetMesh.SetColors(colors);
    }
}
