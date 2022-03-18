using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClinicSceneController : MonoBehaviour
{

    [SerializeField] EricNurse nurse;
    [SerializeField] DoctorController doctor;
    [SerializeField] GameObject patient;


    private void Start()
    {
        StartCoroutine(InitiateClinicSequence());
    }

    private IEnumerator InitiateClinicSequence()     //todo - clean up this sequence so that not the logic is here, could be issues later.
    {
        
        //Nurse start
        nurse._agent.SetDestination(nurse.final.position);
        nurse._animator.SetBool("isWalking", true);
        yield return new WaitForSeconds(0.2f);

        while (nurse._agent.remainingDistance > 0.2f) yield return new WaitForSeconds(0.05f);

        nurse._animator.SetBool("isWalking", false);
        nurse._agent.updateRotation = false;
        nurse.transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));

        nurse._animator.SetBool("isSpeaking", true);
        SoundManager.Instance.PlaySound(SoundManager.Instance.ericIntroClip);
        yield return new WaitForSeconds(17f);
        nurse._animator.SetBool("isSpeaking", false);
        //Nurse end
        

        //Doctor Start
        doctor._agent.SetDestination(doctor.starting.position);
        doctor._animator.SetBool("isWalking", true);
        yield return new WaitForSeconds(0.2f);
        doctor._agent.updateRotation = false;
        while (doctor._agent.remainingDistance > 0.2f) yield return new WaitForSeconds(0.02f);
        doctor._animator.SetBool("isWalking", false);
        doctor._animator.Play("IdleStand");

        doctor.isSpeakingToPatient = true;
        doctor.transform.LookAt(new Vector3(patient.transform.position.x, transform.position.y, patient.transform.position.z));
        SoundManager.Instance.PlaySound(SoundManager.Instance.doctorIntro);
        yield return new WaitForSeconds(7f);

        patient.GetComponent<PatientSpeakingController>().SpeakAnimation(1.5f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.patientSureGoAhead);


        VoiceRecognitionClinic.Instance.hasStartedAskingQuestions = true;

        doctor.isSpeakingToPatient = false;
        doctor._animator.Play("Walk");
        doctor._animator.SetBool("isWalking", true);
        doctor._agent.SetDestination(doctor.final.position);
        yield return new WaitForSeconds(0.15f);

        while (doctor._agent.remainingDistance > 0.2f) yield return new WaitForSeconds(0.01f);
        doctor._animator.SetBool("isWalking", false);
        doctor._animator.Play("IdleStand");

        doctor._agent.updateRotation = false;
        doctor.transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
        //Doctor end
    
    }


    private void Update()
    {
        //if((OVRInput.GetDown(OVRInput.RawButton.X)))TransitionToClinicRoom();
        
    }
}
