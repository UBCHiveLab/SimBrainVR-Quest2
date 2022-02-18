using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClinicSceneController : MonoBehaviour
{
    [SerializeField] GameObject clinic;
    [SerializeField] GameObject reception;

    [SerializeField] EricNurse nurse;

    [SerializeField] GameObject playerVRController;
    [SerializeField] Vector3 clinicPos;

    bool hasTransitioned;

    private void Start()
    {
        clinic.SetActive(false);
    }


    public void TransitionToClinicRoom()
    {
        if (!hasTransitioned)
        {
            hasTransitioned = true;
            reception.SetActive(false);
            clinic.SetActive(true);
            playerVRController.transform.position = clinicPos;

            nurse.InitiateSequence();
        }
       
    }

    private void Update()
    {
        if((OVRInput.GetDown(OVRInput.RawButton.X)))
        {
            
            TransitionToClinicRoom();
        }
    }
}
