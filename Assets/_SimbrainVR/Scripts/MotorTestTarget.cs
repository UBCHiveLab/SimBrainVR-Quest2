using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorTestTarget : MonoBehaviour
{

    [SerializeField] Transform ovrHandRight, ovrHandLeft; //drag in from inspector
    [SerializeField] float maxDist = 0.3f;

    Vector3 originalPos;
    bool hasVibrated;

    void Start()
    {
        originalPos = transform.position;
    }

    
    void Update()
    {
        if (Vector3.Distance(originalPos, ovrHandRight.position) < maxDist)
        {
            transform.position = ovrHandRight.position;
            StartCoroutine(VibrateController(true));
        }
        else if (Vector3.Distance(originalPos, ovrHandLeft.position) < maxDist)
        {
            transform.position = ovrHandLeft.position;
            StartCoroutine(VibrateController(false));
        }else if (Vector3.Distance(originalPos, ovrHandLeft.position) > maxDist && Vector3.Distance(originalPos, ovrHandRight.position) > maxDist)
        {
            transform.position = originalPos;
        }
        else
        {
            hasVibrated = false;
        }

        //else transform.position = originalPos;

    }


    IEnumerator VibrateController(bool isRight)
    {
        if (!hasVibrated)
        {
            hasVibrated = true;

            if (isRight)
            {
                OVRInput.SetControllerVibration(1, .3f, OVRInput.Controller.RTouch);
            }
            else
            {
                OVRInput.SetControllerVibration(1, .3f, OVRInput.Controller.LTouch);
            }

            yield return new WaitForSeconds(0.5f);

            if (isRight)
            {
                OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            }
            else
            {
                OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            }

            //hasVibrated = false;
        }
        
    }
}


//change - for each hand, bind this with bool = isleft, isright
