using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoctorController : MonoBehaviour
{
    public Transform starting, final;
    public NavMeshAgent _agent;
    public Animator _animator;
    public bool isSpeakingToPatient;
    public bool updateRot = false;

    public GameObject dialogue;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();

        _agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        if (updateRot)
        {

            transform.LookAt(Camera.main.transform.position, -Vector3.up);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
    }

    void OnAnimatorIK()
    {
        if (isSpeakingToPatient) return;

        if (Camera.main != null)
        {
            _animator.SetLookAtWeight(0.45f);
            _animator.SetLookAtPosition(Camera.main.transform.position);
        }
    }
}
