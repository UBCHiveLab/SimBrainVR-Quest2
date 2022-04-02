using System.Collections.Generic;
using UnityEngine;

/**
 * Holds active specimen data, including links to the mesh/texture/material and parsed ContentBlockData once the asset bundle is loaded.
 */
public class SpecimenData
{
    public string id;
    public string name;
    public string organ;
    public int version;
    public Mesh mesh;
    public Material material;
    public float scale;
    public float yPos;
    public List<AnnotationData> annotations;
    public GameObject prefab;
    public SpecimenRequestData request;
    public string imgUrl;

    public bool dataLoaded => mesh != null && material != null || prefab != null;
}
