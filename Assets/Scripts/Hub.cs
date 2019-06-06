using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour
{

    public Material normalBoxMaterial;
    public Material wrongChoiceBoxMaterial;

    public Transform boxParent;

    private void Start()
    {
        RandomizePortals();
    }

    public void WrongChoiceMade()
    {
        foreach (var meshRenderer in boxParent.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.sharedMaterial = wrongChoiceBoxMaterial;
        }

        Invoke("ResetNormalMaterial", 0.5f);
    }

    public void ResetNormalMaterial()
    {
        foreach (var meshRenderer in boxParent.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.sharedMaterial = normalBoxMaterial;
        }
    }

    public void RandomizePortals()
    {
        Portal[] allPortals = GetComponentsInChildren<Portal>();

        for (int i = 0; i < 3; i++)
        {
            Portal portal1 = allPortals[i];
            Portal portal2 = allPortals[Random.Range(0, 3)];

            bool isRight1 = portal1.isRight;
            portal1.isRight = portal2.isRight;
            portal2.isRight = isRight1;

            Material mat1 = portal1.GetComponent<MeshRenderer>().sharedMaterial;
            portal1.GetComponent<MeshRenderer>().sharedMaterial = portal2.GetComponent<MeshRenderer>().sharedMaterial;
            portal2.GetComponent<MeshRenderer>().sharedMaterial = mat1;
        }
    }
}
