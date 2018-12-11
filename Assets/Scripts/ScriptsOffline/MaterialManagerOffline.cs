using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MaterialManagerOffline : MonoBehaviour
{
    private Dictionary<int, Material> materials;
    private string pathMaterials = "Materials/Cubes/";
    [SerializeField] private new MeshRenderer renderer;

    private void Initialisation()
    {
        materials = new Dictionary<int, Material>();
        materials.Add(-2, Resources.Load(pathMaterials + "Cube-2") as Material);
        materials.Add(-1, Resources.Load(pathMaterials + "Cube-1") as Material);
        materials.Add(0, Resources.Load(pathMaterials + "Cube0") as Material);
        materials.Add(1, Resources.Load(pathMaterials + "Cube1") as Material);
        materials.Add(2, Resources.Load(pathMaterials + "Cube2") as Material);
    }

    public void UpdateMaterial(int weight)
    {
        if (materials == null)
        {
            Initialisation();
        }
        renderer.material = materials[weight];
    }
}
