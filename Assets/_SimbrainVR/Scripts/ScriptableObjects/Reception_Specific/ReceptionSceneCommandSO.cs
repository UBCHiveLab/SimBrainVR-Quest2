using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ReceptionSceneCommand", menuName = "ScriptableObjects/Reception_Specific/ReceptionSceneCommand")]
public class ReceptionSceneCommandSO : ScriptableObject
{
    
    public void TransitionToClinic()
    {
        ReceptionTrigger receptionTrigger = GameObject.Find("DoorHandleTrigger").GetComponent<ReceptionTrigger>();
        receptionTrigger.GoToClinicFromReception();
    }

}
