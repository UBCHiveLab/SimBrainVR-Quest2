using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public int rayLength = 100; 
    private bool aboutToDraw = false;
    public LineRenderer line;

    Vector3 startDrawingPos = new Vector3();
    Vector3 endDrawingPos = new Vector3(); 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; // did the ray make contact with an object
        if (OVRInput.Get(OVRInput.Button.One))
        {
            Debug.Log("A is pressed at the firstPos"); 
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength*10))
            {
                Debug.Log("object detected"); 
                aboutToDraw = true;
                startDrawingPos = hit.point;
                line.SetPosition(0, startDrawingPos);
                if (OVRInput.Get(OVRInput.Button.Two))
                {
                    Debug.Log("B is pressed at secondPos");
                    if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength = 10))
                    {
                        endDrawingPos = hit.point;
                        line.SetPosition(1, endDrawingPos);
                        line.positionCount = 2;
                    }
                }
            }
          
        }

    }
}
