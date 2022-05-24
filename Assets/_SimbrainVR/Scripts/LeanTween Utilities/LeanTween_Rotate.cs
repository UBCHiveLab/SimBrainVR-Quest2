using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanTween_Rotate : MonoBehaviour
{
    [SerializeField] private bool activateOnStart = default;
    [SerializeField] private bool ignoreTimeScale = default;
    [SerializeField] private GameObject objectToMove = default;
    [SerializeField] private float angle = 0f;

    [SerializeField] private float tweenDuration = 1f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.linear;

    [SerializeField] private bool loop = false;
    [SerializeField] private bool useLoopCount = false;
    [SerializeField] private int loopCount = 10;
    //[SerializeField] private int framerate = 12;
    [SerializeField] private float delay = 0f;

    [SerializeField] private UnityEvent OnTweenCompleted = default;
    [SerializeField] private UnityEvent OnTweenActivated = default;
    [HideInInspector] public UnityEvent<LeanTween_Rotate> OnTweenCompletedPublic = default;

    private int currentTweenId;

    private void Start()
    {
        if (activateOnStart)
            Activate();

    }

    private void OnDestroy()
    {
        LeanTween.cancel(currentTweenId);
    }

    public void Activate()
    {
        LeanTween.cancel(currentTweenId);


        if (loop)
        {
            if (!useLoopCount)
            {
                currentTweenId =
                    LeanTween.rotateZ(objectToMove, angle, tweenDuration)
                        .setEase(tweenType)
                        .setDelay(delay)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setLoopPingPong()
                        .setOnComplete(() =>
                        {
                            OnTweenCompleted?.Invoke();
                            OnTweenCompletedPublic?.Invoke(this);                            
                        })
                        .uniqueId;


            }
            else
            {
                currentTweenId =
                    LeanTween.rotateZ(objectToMove, angle, tweenDuration)
                        .setEase(tweenType)
                        .setDelay(delay)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setLoopPingPong(loopCount)
                        .setOnComplete(() =>
                        {
                            OnTweenCompleted?.Invoke();
                            OnTweenCompletedPublic?.Invoke(this);
                        })
                        .uniqueId;


            }
        }
        else
        {
            currentTweenId =
                LeanTween.rotateZ(objectToMove, angle, tweenDuration)
                    .setEase(tweenType)
                    .setDelay(delay)
                    .setIgnoreTimeScale(ignoreTimeScale)
                    .setOnComplete(() =>
                    {
                        OnTweenCompleted?.Invoke();
                        OnTweenCompletedPublic?.Invoke(this);
                    })
                    .uniqueId;

        }

        OnTweenActivated?.Invoke();
    }

    public void Deactivate()
    {
        LeanTween.cancel(currentTweenId);

    }

}
