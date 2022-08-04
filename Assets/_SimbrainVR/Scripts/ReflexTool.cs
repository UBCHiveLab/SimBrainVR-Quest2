using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflexTool : MonoBehaviour
{

    int rightKneeCounter, leftKneeCounter = 0;
    public int hitLimit = 5;

    private void OnCollisionEnter(Collision collision)
    {
        //print(collision.collider.name);

        if(collision.collider.name == "rightKnee")
        {
            rightKneeCounter++;
        }

        if (collision.collider.name == "leftKnee")
        {
            leftKneeCounter++;
        }

        if(rightKneeCounter >= hitLimit)
        {
            rightKneeCounter = 0;
            MotorTest.Instance.TendonReflexTest(true);
        }

        if(leftKneeCounter >= hitLimit)
        {
            leftKneeCounter = 0;
            MotorTest.Instance.TendonReflexTest(false);
        }
    }
}
