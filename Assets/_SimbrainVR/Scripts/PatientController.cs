using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientController : MonoBehaviour
{

    public Animator _animator;


    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Idle()
    {
        _animator.Play("Idle");
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
        _animator.Play("PupilDecrease_L");
    }

}
