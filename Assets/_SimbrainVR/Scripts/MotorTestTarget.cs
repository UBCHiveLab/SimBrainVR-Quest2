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
        if (Vector3.Distance(originalPos, ovrHandRight.position) < 0.22f)
        {
            transform.position = ovrHandRight.position;
        }else if (Vector3.Distance(originalPos, ovrHandLeft.position) < 0.22f)
        {
            transform.position = ovrHandLeft.position;
        }
        else
        {
            transform.position = originalPos;
        }
    }
}
