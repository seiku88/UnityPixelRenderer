using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Abstract Sprite/Sprite Group")]
public class AbstractSpriteGroup : MonoBehaviour
{
    private AbstractSpriteRenderer baseRenderer;
    public List<AbstractSprite> parts;

    private bool isInitialized = false;

    public void Init(AbstractSpriteRenderer mRenderer)
    {
        parts = new List<AbstractSprite>();

        if (!isInitialized)
        {
            baseRenderer = mRenderer;
            isInitialized = true;
        }

        UpdatePartList();
    }

    protected void UpdatePartList()
    {
        parts.Clear();
        parts.InsertRange(0, gameObject.transform.GetComponentsInChildren<AbstractSprite>());
    }

    protected void OnTransformChildrenChanged()
    {
        UpdatePartList();
    }

    public void Draw(Mesh targetMesh)
    {
        if (isInitialized)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i] != null) parts[i].Draw(targetMesh, baseRenderer.transform.position);
            }
        }
    }
}
