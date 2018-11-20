using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    private Dictionary<int, Material> materials;
    private string pathMaterials = "Materials/Cubes/";
    private Weight weight;
    private new MeshRenderer renderer;
    private new Rigidbody rigidbody;


    private void Start()
    {
        Initialisation();
        weight = GetComponent<Weight>();
        renderer = GetComponent<MeshRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        UpdateMaterial();
    }
    
    private void Initialisation()
    {
        materials = new Dictionary<int, Material>();
        materials.Add(-2, Resources.Load(pathMaterials + "Cube-2") as Material);
        materials.Add(-1, Resources.Load(pathMaterials + "Cube-1") as Material);
        materials.Add(-0, Resources.Load(pathMaterials + "Cube0") as Material);
        materials.Add(1, Resources.Load(pathMaterials + "Cube1") as Material);
        materials.Add(2, Resources.Load(pathMaterials + "Cube2") as Material);
        materials.Add(3, Resources.Load(pathMaterials + "CubeFige") as Material);       
    }

    private void ChangeWeight(int etat)
    {
        renderer.material = materials[etat];        
    }

    private void Freeze()
    {
        renderer.material = materials[3];
    }

    public void UpdateMaterial()
    {
        if(rigidbody.isKinematic)
        {
            Freeze();
            return;
        }        

        ChangeWeight(weight.CurrentWeight);        
    }
}
