using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindPalace_LinkableObject : MonoBehaviour
{
    [SerializeField] private MindPalaceWorldStateSO mindPalaceWorldState = default;

    [SerializeField] private Transform referencePosition = default;

    public virtual Transform ReferencePosition
    {
        get
        {
            if (referencePosition != null)
                return referencePosition;

            return transform;
        }
    }

    public void Select()
    {
        mindPalaceWorldState.HandleGrabbableSelected(this);
    }

}
