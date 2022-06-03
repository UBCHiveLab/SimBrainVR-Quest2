using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorTestTarget : MonoBehaviour
{

    [SerializeField] Transform ovrHandRight, ovrHandLeft; //drag in from inspector
    Vector3 originalPos;

    void Start()
    {
        originalPos = transform.position;
    }

    
    void Update()
    {
        if (Vector3.Distance(originalPos, ovrHandRight.position) < 0.2f)
        {
            transform.position = ovrHandRight.position;
            OVRInput.SetControllerVibration(1, .3f, OVRInput.Controller.RTouch);
        }
        else if (Vector3.Distance(originalPos, ovrHandLeft.position) < 0.2f)
        {
            transform.position = ovrHandLeft.position;
        }
        else
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            
        }
        //else transform.position = originalPos;

    }
}


//change - for each hand, bind this with bool = isleft, isright
