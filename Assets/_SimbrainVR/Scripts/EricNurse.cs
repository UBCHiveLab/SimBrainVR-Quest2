using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EricNurse : MonoBehaviour
{

    public Transform starting, final;
    public NavMeshAgent _agent;
    public Animator _animator;

    public Transform lightSwitch;
    public GameObject lightSource;

    public bool updateRot = false;
    public GameObject dialogue;
    
    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();

        _agent = GetComponent<NavMeshAgent>();

    }


    public void InitiateSequence()
    {
        _agent.SetDestination(final.position);
        StartCoroutine(EricNurseSequence());
    }

    public void Wave()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.ericHello);
    }

    public void ToggleLight(bool isOn)
    {
        _agent.SetDestination(lightSwitch.position);
        StartCoroutine(ToggleLightSequence(isOn));
    }

    private IEnumerator ToggleLightSequence(bool isOn)
    {
        _animator.SetBool("isWalking", true);
        yield return new WaitForSeconds(0.2f);

        while (_agent.remainingDistance > 0.2f)
        {
            yield return new WaitForSeconds(0.02f);
        }

        lightSource.SetActive(isOn);
        _agent.SetDestination(final.position);
        yield return new WaitForSeconds(0.2f);
        while (_agent.remainingDistance > 0.2f)
        {
            yield return new WaitForSeconds(0.02f);
        }
        _animator.SetBool("isWalking", false);
    }

    private IEnumerator EricNurseSequence()     //todo - clean up this sequence so that not the logic is here, could be issues later.
    {
        _animator.SetBool("isWalking", true);
        yield return new WaitForSeconds(0.2f);
        while (_agent.remainingDistance > 0.2f)
        {
            yield return new WaitForSeconds(0.05f);
        }

        _animator.SetBool("isWalking", false);
        _agent.updateRotation = false;
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));

        _animator.SetBool("isSpeaking", true);
        SoundManager.Instance.PlaySound(SoundManager.Instance.ericIntroClip);
        yield return new WaitForSeconds(17f);
        _animator.SetBool("isSpeaking", false);

        GameObject.Find("DoctorGin").GetComponent<DoctorController>()._animator.SetBool("isSpeaking", true);
        SoundManager.Instance.PlaySound(SoundManager.Instance.doctorIntro);
        yield return new WaitForSeconds(7f);
        VoiceRecognitionClinic.Instance.hasStartedAskingQuestions = true;
        GameObject.Find("DoctorGin").GetComponent<DoctorController>()._animator.SetBool("isSpeaking", false);

        SoundManager.Instance.PlaySound(SoundManager.Instance.patientSureGoAhead);
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
        if (Camera.main != null)
        {
            _animator.SetLookAtWeight(0.45f);
            _animator.SetLookAtPosition(Camera.main.transform.position);

        }
    }

}
