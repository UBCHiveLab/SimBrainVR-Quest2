using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transform_SetParent : MonoBehaviour
{
    public void Activate(Transform newParent)
    {
        transform.SetParent(newParent, true);
    }

}
