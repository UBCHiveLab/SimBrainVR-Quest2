using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorTest : MonoBehaviour  //perhaps rename handtest to something more general? motor test script.
{

    public float handWeight = 1f;
    [SerializeField] Transform leftHandTargetPos;  //todo - set this up in start instead of inspector.
    [SerializeField] Transform rightHandTargetPos;
    public bool isHoldingHandsUp;
    Animator _animator;

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
    }

    public void ToggleRaiseArms(bool state)
    {
        isHoldingHandsUp = state;
    }


    void OnAnimatorIK()
    {

        if (isHoldingHandsUp)
        {

            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
            _animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTargetPos.position);

            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
            _animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTargetPos.position);
        }


    }



}


//todo 
//coroutine to raise arm (lerp) then set bool at the end. coroutine should have boolean to prevent running multiple times.