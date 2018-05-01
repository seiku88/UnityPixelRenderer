using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SpriteType
{
    ELLIPSE,
    RECTANGLE,
    TRIANGLE,
    POINT
}

public class AbstractSprite : MonoBehaviour
{
    public SpriteType type;
    public Vector2 size;
    private float maxSize;

    public Color spriteColor;
    public Vector2 localPixelPos;

    public bool CheckPixel(Vector2Int targetPoint, Vector3 worldPivotPos, Vector2Int rendererSize)
    {
        maxSize = size.x > size.y ? (size.x * Mathf.Abs(transform.lossyScale.x) * 0.5f) : (size.y * Mathf.Abs(transform.lossyScale.y) * 0.5f);
        localPixelPos = new Vector2((transform.position.x - worldPivotPos.x + 0.5f) * rendererSize.x, ((transform.position.y - worldPivotPos.y + 0.5f) * rendererSize.y));
        if (transform.lossyScale.x < 0) localPixelPos = new Vector2(rendererSize.x - localPixelPos.x, localPixelPos.y);
        if (transform.lossyScale.y < 0) localPixelPos = new Vector2(localPixelPos.x, rendererSize.y - localPixelPos.y);

        if (targetPoint.x >= (localPixelPos.x - maxSize) && targetPoint.x <= (localPixelPos.x + maxSize) && targetPoint.y >= (localPixelPos.y - maxSize) && targetPoint.y <= (localPixelPos.y + maxSize))
        {
            switch (type)
            {
                case SpriteType.ELLIPSE:
                    {
                        float cosa = 0, sina = 0;
                        if (transform.lossyScale.x >= 0) cosa = Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
                        else cosa = Mathf.Cos((transform.rotation.eulerAngles.z+180) * Mathf.Deg2Rad);
                        if (transform.lossyScale.y >= 0) sina = Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
                        else sina = Mathf.Sin(transform.rotation.eulerAngles.z+180 * Mathf.Deg2Rad);

                        float a = Mathf.Pow(cosa * (targetPoint.x - localPixelPos.x) + sina * (targetPoint.y - localPixelPos.y), 2);
                        float b = Mathf.Pow(sina * (targetPoint.x - localPixelPos.x) - cosa * (targetPoint.y - localPixelPos.y), 2);
                        float ellipse = (a / ((size.x * Mathf.Abs(transform.lossyScale.x)) / 2 * (size.x * Mathf.Abs(transform.lossyScale.x)) / 2)) + (b / ((size.y * Mathf.Abs(transform.lossyScale.y)) / 2 * (size.y * Mathf.Abs(transform.lossyScale.y)) / 2));

                        if (ellipse < 1) return true;
                        else return false;
                    }
                case SpriteType.RECTANGLE:
                    {
                        //TO DO: Add rotation for rectangle
                        if (targetPoint.x >= (localPixelPos.x - (size.x*0.5f)) && targetPoint.x <= (localPixelPos.x + (size.x * 0.5f)) && targetPoint.y >= (localPixelPos.y - (size.y * 0.5f)) && targetPoint.y <= (localPixelPos.y + (size.y * 0.5f))) return true;
                        else return false;
                    }
                case SpriteType.POINT:
                    {
                        if (localPixelPos.x == targetPoint.x && localPixelPos.y == targetPoint.y) return true;
                        else return false;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        
        return false;
    }
}
