using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "_Vector3", menuName = "ScriptableObjects/GlobalVariables/New Vector3")]
public class Vector3SO : ScriptableObject
{
    private Vector3 currentValue = default;

    public UnityEvent OnValueChanged = default; //triggered only when value changed. NOT triggered when value is reset. NOT triggered when value is set to the same current value

    public Vector3 Value
    {
        get => currentValue;
        set
        {
            HasBeenSet = true;

            Vector3 oldValue = Value;

            currentValue = value;

            if (oldValue != currentValue)
            {
                OnValueChanged?.Invoke();
            }

        }
    }

    public bool HasBeenSet
    {
        get;
        private set;
    }

    public void ResetObject()
    {
        currentValue = default;

        HasBeenSet = false;
    }

    public void SetValueToTransform(Transform transformToSet)
    {
        Value = transformToSet.position;
    }
}
