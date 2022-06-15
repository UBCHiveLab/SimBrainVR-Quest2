using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptionist : MonoBehaviour
{


    public AudioSource start_clip;
    public Transform rightHandTargetStartPos, rightHandTargetPos;
    public float lerpVal = 0.05f;
    Animator _anim;
    bool isHoldingClipboard;

    void Start()
    {
        _anim = GetComponent<Animator>();
        Invoke("IntroClip", 3.5f);
    }

    void IntroClip()
    {
        _anim.SetBool("isStanding", true);
        start_clip.Play();
        isHoldingClipboard = true;
        StartCoroutine(SmoothRaiseArms());
    }

    IEnumerator SmoothRaiseArms()
    {
        yield return new WaitForSeconds(1.0f);

        while (Vector3.Distance(rightHandTargetStartPos.position, rightHandTargetPos.position) > 0.06f)
        {
            yield return new WaitForEndOfFrame();
            rightHandTargetStartPos.position = Vector3.Lerp(rightHandTargetStartPos.position, rightHandTargetPos.position, lerpVal);
        }
    }


    void OnAnimatorIK()
    {
        if (isHoldingClipboard)
        {
            _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            _anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandTargetStartPos.position);

            if (Camera.main != null)
            {
                _anim.SetLookAtWeight(0.8f);
                _anim.SetLookAtPosition(Camera.main.transform.position);
            }
        }
    }


}
