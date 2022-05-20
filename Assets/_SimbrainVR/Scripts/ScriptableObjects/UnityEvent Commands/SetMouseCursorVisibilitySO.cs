using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Set Mouse Cursor Visibility_Command", menuName = "ScriptableObjects/UnityEvent Commands/Set Mouse Cursor Visibility")]
public class SetMouseCursorVisibilitySO : ScriptableObject
{
    public void SetVisible()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void SetInvisible()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
