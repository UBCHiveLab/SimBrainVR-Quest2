using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ClinicSceneCommand", menuName = "ScriptableObjects/Clinic_Specific/ClinicSceneCommand")]
public class ClinicSceneCommandSO : ScriptableObject
{
    public void PrintTestMsg()
    {
        Debug.Log("test message");
    }
}
