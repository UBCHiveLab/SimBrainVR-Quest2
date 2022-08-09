using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MedicalLight : MonoBehaviour
{

    [SerializeField]
    GameObject spotLight;
    public LayerMask layerToAvoidHitting;
    PatientController _patientRef;

    bool inputB, inputA;

    private void OnDisable()
    {
        if (_patientRef != null) _patientRef.Idle();

    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            inputB = !inputB;
            spotLight.SetActive(inputB);
        }

    }

    private void FixedUpdate()
    {
        if (!spotLight.activeSelf) return;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerToAvoidHitting))
        {

            if (hit.collider.name == "PatientRightEye")
            {
                PatientController patientController = hit.collider.transform.parent.GetComponent<PatientController>();
                _patientRef = patientController;
                patientController.PupilDecreaseBoth();
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                ClinicalLogger.Instance.hasShoneLight = true;

            }
            else if (hit.collider.name == "PatientLeftEye")
            {
                PatientController patientController = hit.collider.transform.parent.GetComponent<PatientController>();
                _patientRef = patientController;
                patientController.PupilDecreaseBoth();
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

                ClinicalLogger.Instance.hasShoneLight = true;
            }
            else
            {
                if(_patientRef!=null) _patientRef.PupilIncreaseBoth();
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }

}
