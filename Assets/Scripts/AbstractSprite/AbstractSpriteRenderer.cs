using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AbstractSpriteRenderer : MonoBehaviour
{
    public bool updateInRealTime = false;
    public bool pixelSnap = true;

    private List<AbstractSpriteGroup> spriteGroups = null;

    public float pixelPerUnit = 128;
    public Vector2Int spriteSize;

    private Sprite targetSprite;
    private Texture2D targetTexture;
    
    private Color[] cols;

    private SpriteRenderer targetRenderer;

    // Use this for initialization
    void Start ()
    {
        targetRenderer = GetComponent<SpriteRenderer>();
        if (targetRenderer == null) Debug.LogError("Target renderer not found");

        //targetTexture = Instantiate(targetRenderer.material.mainTexture) as Texture2D;
        
        targetTexture = new Texture2D(spriteSize.x, spriteSize.y, TextureFormat.ARGB32, false);
        targetSprite = Sprite.Create(targetTexture, new Rect(Vector2.zero, spriteSize), Vector2.one * 0.5f, pixelPerUnit);
        targetSprite.texture.filterMode = FilterMode.Point;
        targetSprite.texture.wrapMode = TextureWrapMode.Clamp;
        targetRenderer.sprite = targetSprite;

        spriteGroups = new List<AbstractSpriteGroup>();
        spriteGroups.AddRange(gameObject.transform.GetComponentsInChildren<AbstractSpriteGroup>());
        spriteGroups.Sort(AbstractSpriteGroup.SortByLayer);

        Color[] cols = targetSprite.texture.GetPixels();

        for (int c = 0; c < cols.Length; c++)
        {
            cols[c] = Color.clear;
        }

        for (int i = 0; i < spriteGroups.Count; i++)
        {
            spriteGroups[i].SetRenderer(this);
            spriteGroups[i].Draw(ref cols, gameObject.transform.position);
        }

        // actually apply all SetPixels, don't recalculate mip levels
        targetSprite.texture.SetPixels(cols);
        targetSprite.texture.Apply(false);
    }

    public void UpdateTexture()
    {
        if (cols == null) cols = targetSprite.texture.GetPixels();

        for (int c = 0; c < cols.Length; c++)
        {
            cols[c] = Color.clear;
        }

        for (int i = 0; i < spriteGroups.Count; i++)
        {
            if (spriteGroups[i].gameObject.activeInHierarchy) spriteGroups[i].Draw(ref cols, gameObject.transform.position);
        }

        targetSprite.texture.SetPixels(cols);
        targetSprite.texture.Apply(false);
    }

    void Update()
    {
        //Pixel Snap
        if (pixelSnap) transform.position.Set((Mathf.Round(transform.parent.position.x * pixelPerUnit) / pixelPerUnit) - transform.parent.position.x, (Mathf.Round(transform.parent.position.y * pixelPerUnit) / pixelPerUnit) - transform.parent.position.y, transform.position.z);

        if (updateInRealTime) UpdateTexture();
    }
}
