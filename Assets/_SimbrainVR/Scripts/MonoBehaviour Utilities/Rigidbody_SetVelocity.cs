using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigidbody_SetVelocity : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbodyToUse = default;

    public void SetVelocityToZero()
    {
        rigidbodyToUse.velocity = Vector3.zero;
    }

}
