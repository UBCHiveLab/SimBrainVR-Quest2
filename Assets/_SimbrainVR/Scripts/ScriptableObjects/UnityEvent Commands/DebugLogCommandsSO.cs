using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugLog_", menuName = "ScriptableObjects/UnityEvent Commands/Debug Log Command SO")]
public class DebugLogCommandsSO : ScriptableObject
{
    public void LogSimpleString(string value)
    {
        Debug.Log(value);
    }

}
