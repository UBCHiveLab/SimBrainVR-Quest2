using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Set Parent", menuName = "ScriptableObjects/UnityEvent Commands/Set Parent")]
public class SetParent_CommandSO : ScriptableObject
{
    [SerializeField] private bool worldPositionStays = true;
    [SerializeField] private bool forceNewLocalPosition = true;
    [SerializeField] private Vector2 newLocalPosition = Vector2.zero;

    Transform nextParent = default;

    public void SetParentParameterForNextCommand(Transform newParent)
    {
        nextParent = newParent;
    }

    public void Activate(Transform objectToChild)
    {
        if (nextParent != null && objectToChild != null)
        {
            objectToChild.SetParent(nextParent, worldPositionStays);

            objectToChild.localPosition = newLocalPosition;

        }

        nextParent = null;
    }
}
