using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Set Parent to Null", menuName = "ScriptableObjects/UnityEvent Commands/Set Parent to Null")]
public class SetParentToNull_CommandSO : ScriptableObject
{
    public void SetParentToNull(Transform transformToApplyTo)
    {
        transformToApplyTo.SetParent(null);
    }
}
