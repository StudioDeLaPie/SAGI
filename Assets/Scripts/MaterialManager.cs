﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public Dictionary<int, Material> materials;
    string pathMaterials = "Materials/Cubes/";


    private void Start()
    {
        Initialisation();
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
        GetComponent<MeshRenderer>().material = materials[etat];        
    }

    private void Freeze()
    {
        GetComponent<MeshRenderer>().materials[0] = Resources.Load(pathMaterials + "CubeFige") as Material;
    }

    public void UpdateMaterial()
    {
        if(GetComponent<Rigidbody>().isKinematic)
        {
            Freeze();
            return;
        }        

        ChangeWeight(GetComponent<Weight>().CurrentWeight);        
    }
}
