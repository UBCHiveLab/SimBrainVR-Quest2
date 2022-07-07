using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowards : MonoBehaviour
{
    [SerializeField] private Transform transformToUse = default;
    [SerializeField] private bool turnX = false;
    [SerializeField] private bool turnY = false;
    [SerializeField] private bool turnZ = false;
    [SerializeField] private Vector3 offset = Vector3.zero;

    public void Activate(Vector3SO vector3)
    {
        Vector3 direction = vector3.Value - transformToUse.position;

        Vector3 lookRot = Quaternion.LookRotation(direction).eulerAngles;

        if (!turnX)
            lookRot.x = transform.eulerAngles.x;

        if (!turnY)
            lookRot.y = transform.eulerAngles.y;

        if (!turnZ)
            lookRot.z = transform.eulerAngles.z;
        
        transformToUse.eulerAngles = lookRot + offset;
    }

}
