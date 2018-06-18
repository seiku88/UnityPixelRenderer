using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Abstract Sprite/Sprite Renderer")]
public class AbstractSpriteRenderer : MonoBehaviour
{
    public bool updateInRealTime = false;
    public bool pixelSnap = true;
    public float pixelPerUnit = 128;

    public List<AbstractSpriteGroup> sprites;

    private Mesh targetMesh;
    private MeshFilter targetFilter;
    private MeshRenderer targetRenderer;

    // Use this for initialization
    void Start()
    {
        if (GetComponent<MeshFilter>() == null) gameObject.AddComponent<MeshFilter>();
        if (GetComponent<MeshRenderer>() == null) gameObject.AddComponent<MeshRenderer>();
        targetFilter = GetComponent<MeshFilter>();
        targetRenderer = GetComponent<MeshRenderer>();

        targetMesh = new Mesh();
        targetFilter.mesh = targetMesh;
        targetMesh.MarkDynamic();
        targetRenderer.material = new Material(Shader.Find("Hidden/AbstractSpriteBaseShader"));

        sprites = new List<AbstractSpriteGroup>();
        UpdateGroupList();
    }

    protected void UpdateGroupList()
    {
        sprites.Clear();
        sprites.InsertRange(0, gameObject.transform.GetComponentsInChildren<AbstractSpriteGroup>());

        for (int i = 0; i < sprites.Count; i++)
        {
            sprites[i].Init(this);
        }
    }

    protected void OnTransformChildrenChanged()
    {
        UpdateGroupList();
    }

    // Update is called once per frame
    void Update()
    {
        //Pixel Snap
        if (pixelSnap) transform.position.Set((Mathf.Round(transform.parent.position.x * pixelPerUnit) / pixelPerUnit) - transform.parent.position.x, (Mathf.Round(transform.parent.position.y * pixelPerUnit) / pixelPerUnit) - transform.parent.position.y, transform.position.z);

        if (updateInRealTime)
        {
            targetMesh.Clear();
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Draw(targetMesh);
            }

            targetMesh.RecalculateNormals();
        }
    }
}
