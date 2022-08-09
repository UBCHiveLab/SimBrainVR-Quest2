using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflexTool : MonoBehaviour
{

    int rightKneeCounter, leftKneeCounter = 0;
    public int hitLimit = 2;

    private void OnTriggerEnter(Collider other)
    {

        print("reflex tool hit: "  + other.name);
        if(other.name == "rightKnee")
        {
            rightKneeCounter++;
        }

        if (other.name == "leftKnee")
        {
            leftKneeCounter++;
        }

        if (rightKneeCounter >= hitLimit)
        {
            rightKneeCounter = 0;
            MotorTest.Instance.TendonReflexTest(true);
        }

        if (leftKneeCounter >= hitLimit)
        {
            leftKneeCounter = 0;
            MotorTest.Instance.TendonReflexTest(false);
        }
    }
    
}
