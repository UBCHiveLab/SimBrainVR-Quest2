using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatientDialogueOption { WhatHappened, MedicalHistory, Medication, Allergies, DrinkSmokeDrugs, FamilyHistory, FamilyMember, DontKnow }

//used for eye test (swinging light test)
public class PatientController : MonoBehaviour
{
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Idle()
    {
        _animator.Play("Idle");
    }

    public void PupilDecreaseBoth()
    {
        _animator.SetBool("decrease", true);
    }

    public void PupilIncreaseBoth()
    {
        _animator.SetBool("decrease", false);
    }

    public void PupilIncreaseR()
    {
        _animator.Play("PupilIncreaseR");

    }

    public void PupilDecreaseR()
    {
        _animator.Play("PupilDecrease_R");
    }

    public void PupilIncreaseL()
    {
        _animator.Play("PupilIncreaseL");
    }

    public void PupilDecreaseL()
    {
        _animator.Play("PupilDecrease");
    }




}
