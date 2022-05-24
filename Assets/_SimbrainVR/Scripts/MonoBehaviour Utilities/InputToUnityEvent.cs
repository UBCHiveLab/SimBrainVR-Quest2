using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputToUnityEvent : MonoBehaviour
{
    [SerializeField] private KeyCode key = KeyCode.JoystickButton0;

    public UnityEvent OnSinglePress = default;

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            OnSinglePress?.Invoke();
        }
    }

    public void ChangeToLeftHand()
    {
        key = KeyCode.JoystickButton3;
    }
    public void ChangeToRightHand()
    {
        key = KeyCode.JoystickButton1;

    }
}
