using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusCameraFade : Singleton<OculusCameraFade>
{
    public GameObject obj;
    
    [Tooltip("Fade duration")]
    public float fadeTime = 0.7f;

    [Tooltip("Screen color at maximum fade")]
    public Color fadeColor = new Color(0.01f, 0.01f, 0.01f, 1.0f);

    /// <summary>
    /// The render queue used by the fade mesh. Reduce this if you need to render on top of it.
    /// </summary>
    public int renderQueue = 5000;

    private float uiFadeAlpha = 0;

    private MeshRenderer fadeRenderer;
    private MeshFilter fadeMesh;
    private Material fadeMaterial = null;
    private bool isFading = false;

    public float currentAlpha { get; private set; }
    
    void Awake()
    {
        // create the fade material
        fadeMaterial = new Material(Shader.Find("Oculus/Unlit Transparent Color"));
        fadeMesh = obj.AddComponent<MeshFilter>();
        fadeRenderer = obj.AddComponent<MeshRenderer>();

        var mesh = new Mesh();
        fadeMesh.mesh = mesh;

        Vector3[] vertices = new Vector3[4];

        float width = 2f;
        float height = 2f;
        float depth = 1f;

        vertices[0] = new Vector3(-width, -height, depth);
        vertices[1] = new Vector3(width, -height, depth);
        vertices[2] = new Vector3(-width, height, depth);
        vertices[3] = new Vector3(width, height, depth);

        mesh.vertices = vertices;

        int[] tri = new int[6];

        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;

        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;

        mesh.triangles = tri;

        Vector3[] normals = new Vector3[4];

        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;

        mesh.normals = normals;

        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);

        mesh.uv = uv;

        SetFadeLevel(0);
    }
    
    void OnDestroy()
    {
        if (fadeRenderer != null)
            Destroy(fadeRenderer);

        if (fadeMaterial != null)
            Destroy(fadeMaterial);

        if (fadeMesh != null)
            Destroy(fadeMesh);
    }

    public void DoFade(Action midTimeAction)
    {
        StartCoroutine(FadeCoroutine(midTimeAction));
    }

    public IEnumerator FadeCoroutine(Action midTimeAction)
    {
        isFading = true;
        yield return StartCoroutine(Fade(0,1));
        if (midTimeAction != null)
        {
            midTimeAction();
        }
        yield return StartCoroutine(Fade(1,0));
        isFading = false;
    }
    
    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTime));
            SetMaterialAlpha();
            yield return new WaitForEndOfFrame();
        }
    }
    
    public void SetFadeLevel(float level)
    {
        currentAlpha = level;
        SetMaterialAlpha();
    }
    
    private void SetMaterialAlpha()
    {
        Color color = fadeColor;
        color.a = Mathf.Max(currentAlpha, uiFadeAlpha);
        isFading = color.a > 0;
        if (fadeMaterial != null)
        {
            fadeMaterial.color = color;
            fadeMaterial.renderQueue = renderQueue;
            fadeRenderer.material = fadeMaterial;
            fadeRenderer.enabled = isFading;
        }
    }
}
