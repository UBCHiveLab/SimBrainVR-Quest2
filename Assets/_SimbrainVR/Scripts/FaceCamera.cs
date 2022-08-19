using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform.position);
            transform.Rotate(0, 180f, 0, Space.Self);
        }

    }
}
