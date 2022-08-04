using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FundoscopyTrigger : MonoBehaviour
{
    public bool isRight, isLeft;


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "rightRetinaTrigger")
        {
            isRight = true;
            isLeft = false;
        }

        if (other.name == "leftRetinaTrigger")
        {
            isLeft = true;
            isRight = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("trigger exited");
        isRight = false;
        isLeft = false;
    }
}
