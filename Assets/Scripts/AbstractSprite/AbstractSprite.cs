using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpriteVertex
{
    public Transform position;
    public Color col;
}

[AddComponentMenu("Abstract Sprite/Sprite Part")]
public class AbstractSprite : MonoBehaviour
{
    public List<SpriteVertex> points;
    public List<int> pointIndex;

    public void Draw(Mesh targetMesh, Vector3 basePos)
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

            for (int v = 0; v < points.Count; v++)
            {
                colors.Add(points[v].col);
            }

            targetMesh.SetVertices(vertices);
            targetMesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
            targetMesh.SetColors(colors);
        }
        else Debug.LogWarning("Index count aren't dividing by 3");
    }
}
