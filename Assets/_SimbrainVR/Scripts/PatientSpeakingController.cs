using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientSpeakingController : MonoBehaviour
{
    public float lookAtWeight = 0.9f;
    public bool isMakingEyeContact;
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void SpeakAnimation(float delay)
    {
        _animator.SetBool("isSpeaking", true);
        Invoke("StopSpeaking", delay);
    }
    void StopSpeaking()
    {
        _animator.SetBool("isSpeaking", false);
    }

    public void Speak(PatientDialogueOption dialogueOption)
    {
        switch (dialogueOption)
        {
            case PatientDialogueOption.WhatHappened:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientWhatHappened);
                SpeakAnimation(8f);
                break;
            case PatientDialogueOption.MedicalHistory:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientMedicalHistory);
                SpeakAnimation(8f);
                break;

            case PatientDialogueOption.Medication:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientMedication);
                SpeakAnimation(12f);
                break;

            case PatientDialogueOption.Allergies:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientAllergies);
                SpeakAnimation(6f);
                break;

            case PatientDialogueOption.DrinkSmokeDrugs:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientDrugs);
                SpeakAnimation(10f);
                break;

            case PatientDialogueOption.FamilyHistory:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientFather);
                SpeakAnimation(8f);
                break;

            case PatientDialogueOption.FamilyMember:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientFamily);
                SpeakAnimation(8f);
                break;

            case PatientDialogueOption.DontKnow:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientDontKnow);
                SpeakAnimation(8f);
                break;

            default:
                break;
        }

    }

    void MadeEyeContact()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            isMakingEyeContact = (hit.collider.name == "patientCollider");
        }

    }

    
    void OnAnimatorIK()
    {
        if (Camera.main != null)
        {
            _animator.SetLookAtWeight(lookAtWeight);
            _animator.SetLookAtPosition(Camera.main.transform.position);
        } 
    } 

    private void Update()
    {
        MadeEyeContact();
    }

}
