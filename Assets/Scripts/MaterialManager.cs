using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MaterialManager : NetworkBehaviour
{
    private Dictionary<int, Material> materials;
    private string pathMaterials = "Materials/Cubes/";
    private Weight weight;
    [SyncVar] private int materialIndex;
    [SerializeField] private new MeshRenderer renderer;
    private new Rigidbody rigidbody;


    private void Start()
    {
        Initialisation();
        weight = GetComponent<Weight>();
        rigidbody = GetComponent<Rigidbody>();
        UpdateMaterial();
        ChangeMaterial();
    }

    private void Initialisation()
    {
        materials = new Dictionary<int, Material>();
        materials.Add(-2, Resources.Load(pathMaterials + "Cube-2") as Material);
        materials.Add(-1, Resources.Load(pathMaterials + "Cube-1") as Material);
        materials.Add(0, Resources.Load(pathMaterials + "Cube0") as Material);
        materials.Add(1, Resources.Load(pathMaterials + "Cube1") as Material);
        materials.Add(2, Resources.Load(pathMaterials + "Cube2") as Material);
        materials.Add(3, Resources.Load(pathMaterials + "CubeFige") as Material);
    }

    private void Update()
    {
        UpdateMaterial();
        ChangeMaterial();
    }

    public void UpdateMaterial()
    {
        if (rigidbody.isKinematic)
            materialIndex = 3;
        else
            materialIndex = weight.CurrentWeight;
    }

    private void ChangeMaterial()
    {
        renderer.material = materials[materialIndex];
    }
}
