using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transform_Reference : MonoBehaviour
{
    //Component to be extended to support using Scriptable Objects as tags

    [SerializeField] private Transform referredTransformObject = default;

    public Transform TransformFromRef
    {
        get => referredTransformObject;
    }

    private void Start()
    {
        
    }

}
