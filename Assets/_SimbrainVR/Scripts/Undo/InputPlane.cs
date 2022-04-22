using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlane : MonoBehaviour
{
    Camera mainCam;
    RaycastHit hitInfo;
    public Transform cubePrefab;

    private void Awake()
    {
        mainCam = Camera.main; 
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition); 
            if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                Color c = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                ICommand command = new PlaceCubeCommand(hitInfo.point, c, cubePrefab);
                CommandInvoker.AddCommand(command);
                //CubePlacer.PlaceCube(hitInfo.point, c, cubePrefab); 
            }
        }
    }
}
