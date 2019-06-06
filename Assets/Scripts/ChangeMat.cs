using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeMat : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    
    public Material initialMaterial;
    public Material greenMat;
    public Material redMat;
    public Material yellowMat;
    
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        initialMaterial = _meshRenderer.sharedMaterial;
    }

    // Update is called once per frame
    public void OnFocusEnter(BaseEventData data)
    {
        _meshRenderer.sharedMaterial = greenMat;
    }
    public void OnFocusExit(BaseEventData data)
    {
        _meshRenderer.sharedMaterial = initialMaterial;
    }
    
    public void OnInputDown(BaseEventData data)
    {
        _meshRenderer.sharedMaterial = redMat;
    }
    
    public void OnInputClicked(BaseEventData data)
    {
        _meshRenderer.sharedMaterial = yellowMat;
    }
}
