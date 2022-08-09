using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingForce : MonoBehaviour
{

    public float speed = 1f;
    public float maxAngleZ = 100f;
    float timer = 0f;
    int phase = 0;


    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > 1f)
        {
            phase++;
            phase %= 4;            //Keep the phase between 0 to 3.
            timer = 0f;
        }

        switch (phase)
        {
            case 0:
                transform.Rotate(0f, 0f, speed * (1 - timer));  //Speed, from maximum to zero.
                if(transform.localEulerAngles.z > maxAngleZ)
                {
                    phase = 1;
                    break;
                }
                break;
            case 1:
                transform.Rotate(0f, 0f, -speed * timer);       //Speed, from zero to maximum.
                break;

        }
    }

    public void ResetPendelum()
    {
        timer = 0;
        phase = 0;
        transform.localEulerAngles = new Vector3(0, -90f, 0);
        enabled = false;
    }
}


/*
 * 
    public float timer = 0f;
    public float speed = 1f;
    public int phase = 0;
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > 1f)
        {
            phase++;
            phase %= 4;            //Keep the phase between 0 to 3.
            timer = 0f;
        }

        switch (phase)
        {
            case 0:
                transform.Rotate(0f, 0f, speed * (1 - timer));  //Speed, from maximum to zero.
                break;
            case 1:
                transform.Rotate(0f, 0f, -speed * timer);       //Speed, from zero to maximum.
                break;
            case 2:
                transform.Rotate(0f, 0f, -speed * (1 - timer)); //Speed, from maximum to zero.
                break;
            case 3:
                transform.Rotate(0f, 0f, speed * timer);        //Speed, from zero to maximum.
                break;
        }
    }

*/