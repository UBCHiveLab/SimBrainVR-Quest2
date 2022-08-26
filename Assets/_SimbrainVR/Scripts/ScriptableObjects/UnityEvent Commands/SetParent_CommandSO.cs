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

            if (forceNewLocalPosition)
                objectToChild.localPosition = newLocalPosition;

        }

        nextParent = null;
    }
    public void ParentFirstRigidbodyToSecondRigidbody(Collider collider1, Collider collider2)
    {
        if (collider1.attachedRigidbody == null)
            return;

        if (collider2.attachedRigidbody == null)
            return;

        Transform newChild = collider1.attachedRigidbody.transform;
        Transform newParent = collider2.attachedRigidbody.transform;

        SetParentParameterForNextCommand(newParent);
        Activate(newChild);
    }

}
