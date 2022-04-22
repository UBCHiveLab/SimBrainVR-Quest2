using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCubeCommand : ICommand
{
    Vector3 position;
    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Undo()
    {
       // CubePlacer.PlaceCube(position, color, cube); 
    }

    
}
