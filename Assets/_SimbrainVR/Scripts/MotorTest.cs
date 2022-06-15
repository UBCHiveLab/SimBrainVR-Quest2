using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MotorTest : MonoBehaviour  //perhaps rename handtest to something more general? motor test script.
{

    public float ikWeight = 1f;
    [SerializeField] Transform leftHandTargetPos, leftHandTargetFinalPos;  //todo - set this up in start instead of inspector.
    [SerializeField] Transform rightHandTargetPos, rightHandTargetFinalPos;

    [SerializeField] Transform leftLegTargetPos;  //todo - set this up in start instead of inspector.
    [SerializeField] Transform rightLegTargetPos;

    public bool isHoldingHandsUp;
    public bool isHoldingLegsUp;

    public Transform bedTransform, pointA, pointB;

    Animator _animator;
    NavMeshAgent _agent;

    private static MotorTest _instance;

    public static MotorTest Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.baseOffset = 0.3f;
    }

    public void ToggleRaiseArms(bool state)
    {
        if (state)
        {
            StartCoroutine(SmoothRaiseArms());
        }
        else
        {
            isHoldingHandsUp = false;
        }
    }

    public void ToggleLegs(bool state)
    {
        isHoldingLegsUp = state;
    }


    bool walkCoroutineStarted, sitCoroutineStarted;


    public void TakeWalk()
    {
        _agent.enabled = true;
        _agent.baseOffset = 0f;
        StartCoroutine(Walk());
    }

    public void TakeSeat()
    {
        StartCoroutine(SitDown());
    }

    IEnumerator SitDown()
    {
        if (!sitCoroutineStarted)
        {
            sitCoroutineStarted = true;

            _animator.SetBool("isStanding", false);
            _agent.SetDestination(bedTransform.position);
            yield return new WaitForSeconds(0.1f);

            while (_agent.remainingDistance > 0.2f)
            {
                yield return new WaitForEndOfFrame();
            }

            _agent.baseOffset = 0.3f;
            _agent.enabled = false;
            transform.LookAt(new Vector3(pointA.position.x, transform.position.y, pointA.position.z));
            _animator.SetBool("isWalking", false);

            sitCoroutineStarted = false;
        }
       
    }
    IEnumerator Walk()
    {
        if (!walkCoroutineStarted)
        {
            walkCoroutineStarted = true;
            _animator.SetBool("isStanding", false);

            if (Vector3.Distance(transform.position, bedTransform.position) <= 0.5f)
            {
                print("here111");
                _animator.SetBool("isWalking", true);
                yield return new WaitForSeconds(.9f);
                _agent.SetDestination(pointA.position);
            }

            if (Vector3.Distance(transform.position, pointA.position) <= 0.5f)
            {
                _agent.SetDestination(pointB.position);
            }

            if (Vector3.Distance(transform.position, pointB.position) <= 0.5f)
            {
                _agent.SetDestination(pointA.position);
            }


            yield return new WaitForSeconds(0.1f);
            while (_agent.remainingDistance > 0.3f)
            {
                yield return new WaitForSeconds(0.1f);
            }

            _animator.SetBool("isStanding", true);

            _agent.enabled = false;
            transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
            _agent.enabled = true;

            walkCoroutineStarted = false;
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleRaiseArms(true);
        }
        
    }

    IEnumerator SmoothRaiseArms()
    {

        isHoldingHandsUp = true;

        while (Vector3.Distance(rightHandTargetPos.position, rightHandTargetFinalPos.position) > 0.05f)
        {
            yield return new WaitForEndOfFrame();
            rightHandTargetPos.position = Vector3.Lerp(rightHandTargetPos.position, rightHandTargetFinalPos.position, 0.0095f);
            leftHandTargetPos.position = Vector3.Lerp(leftHandTargetPos.position, leftHandTargetFinalPos.position, 0.0095f);
        }
    }

    void OnAnimatorIK()
    {

        if (isHoldingHandsUp)
        {

            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
            _animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTargetPos.position);

            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
            _animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTargetPos.position);
        }

        if (isHoldingLegsUp)
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ikWeight);
            _animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftLegTargetPos.position);

            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikWeight);
            _animator.SetIKPosition(AvatarIKGoal.RightFoot, rightLegTargetPos.position);
        }
    }

    /*
    bool isToggling;
    IEnumerable ToggleHandsPosition(bool isUp)
    {

        if (!isToggling)
        {
            isToggling = true;

            while ()
            {
                
            }


            isToggling = false;

        }

    }*/



}


//todo 
//coroutine to raise arm (lerp) then set bool at the end. coroutine should have boolean to prevent running multiple times.