using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInput : MonoBehaviour
{

    public Camera eventCamera = null;
    public OVRInput.Button clickButton = OVRInput.Button.One;
    public OVRInput.Controller controller = OVRInput.Controller.All; 

   protected void Awake()
   {
        
   }

    public bool GetMouseButton(int button)
    {
        return OVRInput.Get(clickButton, controller);
    }
    public bool GetMouseButtonDown(int button)
    {
        return OVRInput.GetDown(clickButton, controller); 
    }

    public bool GetMouseButtonUp(int button)
    {
        return OVRInput.GetUp(clickButton, controller);
    }
    public Vector2 mousePosition
    {
        get
        {
            return new Vector2(eventCamera.pixelWidth /2, eventCamera.pixelHeight /2);
        }
    }
}
