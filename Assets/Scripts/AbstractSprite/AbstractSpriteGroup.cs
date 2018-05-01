using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractSpriteGroup : MonoBehaviour
{
    private AbstractSpriteRenderer spriteRenderer;
    public int layer = 0;
    public List<AbstractSprite> spriteLayer = null;

    private Color[] cols;

    private Vector2Int point;

    public static int SortByLayer(AbstractSpriteGroup groupA, AbstractSpriteGroup groupB)
    {
        return groupA.layer.CompareTo(groupB.layer);
    }

    public void SetRenderer(AbstractSpriteRenderer asr) { spriteRenderer = asr; }

    public void Draw(ref Color[] cols, Vector3 worldPos)
    {
        //cols = targetTexture.GetPixels();

        for (int i = spriteLayer.Count-1; i >= 0; i--)
        {
            if (spriteLayer[i] != null)
            {
                if (spriteLayer[i].enabled)
                {
                    for (int c = 0; c < cols.Length; c++)
                    {
                        point.Set(c % spriteRenderer.spriteSize.x, c / spriteRenderer.spriteSize.y);
                        if (cols[c] == Color.clear)
                        {
                            if (spriteLayer[i].CheckPixel(point, worldPos, spriteRenderer.spriteSize)) cols[c] = spriteLayer[i].spriteColor;
                        }
                    }
                    //targetTexture.SetPixels(cols);
                }
            }
        }
    }
}
