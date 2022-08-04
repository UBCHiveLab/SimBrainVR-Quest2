using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingMotion : MonoBehaviour
{
    public Transform pointA, pointB;
    Vector3 targetPos;
    public float speed = 1f;
    public float minDistance = 0.1f;


    private void Awake()
    {
        targetPos = pointB.position;
    }


    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.LerpUnclamped(transform.position, targetPos, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, pointB.position) <= minDistance)
        {
            targetPos = pointA.position;
        }
        else if(Vector3.Distance(transform.position, pointA.position) <= minDistance)
        {
            targetPos = pointB.position;
        }

    }
}
