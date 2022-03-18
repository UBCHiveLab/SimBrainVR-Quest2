using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 mousePos;
    public Material material;
    private int currLines; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Right now, If I press the A button and then move the joystick - I am able to have a dot 
        //Changes to be made - once I press, it should start line and end when released
        //Make sure the line is continuos - look into that (maybe the video of the guy with the 4 video tutorials (How to Draw shapes in Unity - by BLANKdev)
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (line == null)
            {
                CreateLine(); 
            }
            mousePos = Camera.main.ScreenToWorldPoint(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick));
            line.SetPosition(0, mousePos);
            line.SetPosition(1, mousePos);
            Debug.Log("pressed down " + mousePos); 
        }
        else if ((OVRInput.GetUp(OVRInput.Button.One)) && line)
        {
            mousePos = Camera.main.ScreenToWorldPoint(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick));
          //line.SetPosition(1, mousePos);
           //ine = null;
          //currLines++;
            Debug.Log("pressed up " + mousePos);
        }
        /*
        else if (OVRInput.Get(OVRInput.Button.One))
        {
            mousePos = Camera.main.ScreenToWorldPoint(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick));
            Debug.Log("joystick movement" + mousePos);

        }
        */
    }

    void CreateLine()
    {
        line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        line.useWorldSpace = true;
        line.numCapVertices = 50; 
    }
}
 