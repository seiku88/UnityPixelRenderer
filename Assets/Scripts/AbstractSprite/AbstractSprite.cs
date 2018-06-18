using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpriteVertex
{
    public Transform position;
    public Color col;

    public void SetColor(Color newColor) { this.col = newColor; }
}

[AddComponentMenu("Abstract Sprite/Sprite Part")]
public class AbstractSprite : MonoBehaviour
{
    protected bool isInitialized = true;
    protected bool lateColor = false;

    public bool showPoints = true;//Will show point transforms in hierachy

    public List<SpriteVertex> points;
    public List<int> pointIndex;

    public void UpdatePointVisibility()
    {
        for (int p = 0; p < points.Count; p++)
        {
            if (!showPoints)
            {
                if (points[p].position.gameObject.GetComponent<AbstractSprite>() == null)
                {
                    points[p].position.hideFlags = HideFlags.HideInHierarchy;
                    return;
                }
            }

            points[p].position.hideFlags = HideFlags.None;
        }
    }

    public virtual void Draw(Mesh targetMesh, Vector3 basePos)
    {
        if (isInitialized)
        {
            if (pointIndex.Count % 3 == 0)
            {
                List<Vector3> vertices = new List<Vector3>();
                List<int> indices = new List<int>();
                List<Color> colors = new List<Color>();

                if (targetMesh.vertexCount > 0)
                {
                    targetMesh.GetVertices(vertices);
                    targetMesh.GetColors(colors);
                    indices = new List<int>(targetMesh.GetIndices(0));
                }

                for (int i = 0; i < pointIndex.Count; i++)
                {
                    indices.Add(pointIndex[i] + vertices.Count);
                }

                for (int v = 0; v < points.Count; v++)
                {
                    vertices.Add(points[v].position.position - basePos);
                }

                if (!lateColor)
                {
                    for (int v = 0; v < points.Count; v++)
                    {
                        colors.Add(points[v].col);
                    }
                }

                targetMesh.SetVertices(vertices);
                targetMesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
                if (!lateColor) targetMesh.SetColors(colors);

                UpdatePointVisibility();//Temporal placement, will move to other function later
            }
            else Debug.LogWarning("Index count aren't dividing by 3");
        }
    }
}
