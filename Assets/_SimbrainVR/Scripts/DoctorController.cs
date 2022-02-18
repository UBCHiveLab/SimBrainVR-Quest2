using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoctorController : MonoBehaviour
{
    public Transform starting, final;
    NavMeshAgent _agent;
    public Animator _animator;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();

        _agent = GetComponent<NavMeshAgent>();

    }


    void OnAnimatorIK()
    {
        if (Camera.main != null)
        {
            _animator.SetLookAtWeight(0.8f);
            _animator.SetLookAtPosition(Camera.main.transform.position);
        }
    }
}
