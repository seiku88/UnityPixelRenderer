using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Abstract Sprite/Sprite Part Ellipse")]
public class AbstractSpriteEllipse : AbstractSprite
{
    public Color colorBorder;
    public Color colorCenter;

    private SpriteVertex[] ellipseVertices = null;

    // Use this for initialization
    void Start ()
    {
        if (gameObject.transform.childCount == 0)
        {
            points.Clear();
            pointIndex.Clear();

            if (ellipseVertices == null) ellipseVertices = new SpriteVertex[7];
            GameObject[] vertObjects = new GameObject[6];

            for (int i = 0; i < 7; i++)
            {
                if (i == 0) ellipseVertices[0].position = gameObject.transform;
                else
                {
                    vertObjects[i - 1] = new GameObject();
                    vertObjects[i - 1].hideFlags = HideFlags.HideInHierarchy;
                    ellipseVertices[i].position = vertObjects[i - 1].transform;
                    ellipseVertices[i].position.parent = gameObject.transform;
                }
            }

            ellipseVertices[0].col = colorCenter;
            ellipseVertices[1].position.localPosition = new Vector3(0, 1, 0);
            ellipseVertices[2].position.localPosition = new Vector3(Mathf.Sqrt(3) * 0.5f, 0.5f, 0);
            ellipseVertices[3].position.localPosition = new Vector3(Mathf.Sqrt(3) * 0.5f, -0.5f, 0);
            ellipseVertices[4].position.localPosition = new Vector3(0, -1, 0);
            ellipseVertices[5].position.localPosition = new Vector3(-Mathf.Sqrt(3) * 0.5f, -0.5f, 0);
            ellipseVertices[6].position.localPosition = new Vector3(-Mathf.Sqrt(3) * 0.5f, 0.5f, 0);

            if (points.Count == 0)
            {
                for (int i = 0; i < ellipseVertices.Length; i++)
                {
                    if (i > 0) ellipseVertices[i].col = colorBorder;
                    points.Add(ellipseVertices[i]);
                }

                int[] tmpindex = new int[18] { 1, 2, 0, 0, 2, 3, 0, 3, 4, 5, 0, 4, 6, 0, 5, 6, 1, 0 };
                pointIndex.AddRange(tmpindex);
            }
        }
    }

    public void Draw(Mesh targetMesh, Vector3 basePos)
    {
        for (int v = 0; v < points.Count; v++)
        {
            //Set Colors runtime
        }
        base.Draw(targetMesh, basePos);
    }
}
