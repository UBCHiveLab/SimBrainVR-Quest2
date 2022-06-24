
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls patient speech as well as walking beahviors
public class PatientSpeakingController : MonoBehaviour
{
    public float lookAtWeight = 0.9f;
    public bool isMakingEyeContact;
    public GameObject dialogueBox;
    public bool isLookingAtPlayer;
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SpeakAnimation(float delay)
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
                //SpeakAnimation(7.9f);
                _animator.Play("PatientWhatHappened");
                dialogueBox.SetActive(true);
                dialogueBox.GetComponent<Dialogue>().TypeSpecificLine("I have double vision and can’t open my eye. My eye is stuck in this position. Also.... I have worst headache of my life. ");
                break;
            case PatientDialogueOption.MedicalHistory:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientMedicalHistory);
                //SpeakAnimation(4.5f);
                dialogueBox.SetActive(true);
                dialogueBox.GetComponent<Dialogue>().TypeSpecificLine("I have Hypertension, dyslipidemia, type 2 diabetes");
                _animator.Play("PatientMedicalHistory");
                break;

            case PatientDialogueOption.Medication:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientMedication);
                dialogueBox.SetActive(true);
                dialogueBox.GetComponent<Dialogue>().TypeSpecificLine("I am taking Amlodipine 10 mg PO daily, atorvastatin 20 mg PO daily, metformin 1000 mg PO BID");
                _animator.Play("PatientMedication");
                //SpeakAnimation(8.75f);
                break;

            case PatientDialogueOption.Allergies:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientAllergies);
                _animator.Play("PatientAllergies");
                dialogueBox.SetActive(true);
                dialogueBox.GetComponent<Dialogue>().TypeSpecificLine("when I take penicillin, I get rashes on my body");
                break;

            case PatientDialogueOption.DrinkSmokeDrugs:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientDrugs);
                _animator.Play("PatientSmokingDrugs");
                dialogueBox.SetActive(true);
                dialogueBox.GetComponent<Dialogue>().TypeSpecificLine("I have smoked from 20 years. And Social drinker like 2 times / week.");
                break;

            case PatientDialogueOption.FamilyHistory:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientFather);
                _animator.Play("PatientFamilyHistory");
                dialogueBox.SetActive(true);
                dialogueBox.GetComponent<Dialogue>().TypeSpecificLine("My Father passed away from aneurysm rupture in his 60’s.");
                break;

            case PatientDialogueOption.FamilyMember:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientFamily);
                _animator.Play("PatientHusband");
                dialogueBox.SetActive(true);
                dialogueBox.GetComponent<Dialogue>().TypeSpecificLine("I live with my husband.");
                break;

            case PatientDialogueOption.DontKnow:
                SoundManager.Instance.PlaySound(SoundManager.Instance.patientDontKnow);
                if(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Idle") 
                    SpeakAnimation(2.7f);
                
                
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
            isMakingEyeContact = (hit.collider.tag == "Human");
        }

    }

    
    void OnAnimatorIK()
    {
        if (Camera.main != null && isLookingAtPlayer)
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
