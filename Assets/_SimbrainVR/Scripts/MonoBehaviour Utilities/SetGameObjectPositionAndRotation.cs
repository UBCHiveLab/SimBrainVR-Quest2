using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameObjectPositionAndRotation : MonoBehaviour
{
    [SerializeField] private Transform positionHint = default;
    [SerializeField] private Transform rotationHint = default;

    public void SetPosition(Transform transformToChange)
    {
        transformToChange.position = positionHint.position;
    }

    public void SetRotation(Transform transformToChange)
    {
        transformToChange.rotation = positionHint.rotation;
    }
}
