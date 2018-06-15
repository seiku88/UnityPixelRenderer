using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Abstract Sprite/Sprite Group")]
public class AbstractSpriteGroup : MonoBehaviour
{
    private AbstractSpriteRenderer baseRenderer;
    public AbstractSprite[] parts;

    private bool isInitialized = false;

    public void Init(AbstractSpriteRenderer mRenderer)
    {
        if (!isInitialized)
        {
            baseRenderer = mRenderer;
            isInitialized = true;
        }
    }

    public void Draw(Mesh targetMesh)
    {
        if (isInitialized)
        {
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].Draw(targetMesh, baseRenderer.transform.position);
            }
        }
    }
}
