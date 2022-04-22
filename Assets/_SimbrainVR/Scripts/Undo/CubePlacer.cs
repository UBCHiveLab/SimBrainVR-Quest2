using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubePlacer
{
    public static void PlaceCube(Vector3 position, Color color, Transform cube)
    {
        Transform newCube = GameObject.Instantiate(cube, position, Quaternion.identity);
        newCube.GetComponentInChildren<MeshRenderer>().material.color = color; 
    }
}
