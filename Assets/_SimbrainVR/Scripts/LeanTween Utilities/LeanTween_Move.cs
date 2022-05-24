using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanTween_Move : MonoBehaviour
{
    [SerializeField] private bool useForDebug = false;
    [SerializeField] private bool activateOnStart = default;
    [SerializeField] private bool ignoreTimeScale = default;
    [SerializeField] private GameObject objectToMove = default;
    [SerializeField] private Vector2 signedOffsetToMove = new Vector2(0f, 10f);
    [SerializeField] private bool useTransformHints = false;
    [SerializeField] private Transform originPositionTransformHint = default;
    [SerializeField] private Transform targetPositionTransformHint = default;
    [SerializeField] private float tweenDuration = 1f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.linear;
    [SerializeField] private bool useCustomCurve = false;
    [SerializeField] private AnimationCurve customCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private bool loop = false;
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool useLoopCount = false;
    [SerializeField] private int loopCount = 10;
    [SerializeField] private UnityEvent OnTweenActivated = default;
    [SerializeField] private UnityEvent OnTweenCompleted = default;
    [HideInInspector] public UnityEvent<LeanTween_Move> OnTweenCompletedPublic = default;

    private bool firstActivation = true;
    private int currentTweenId;
    private Vector2 initialPosition;
    private Vector2 positionBeforeFirstActivation;

    private void Start()
    {
        if (firstActivation)
        {
            positionBeforeFirstActivation = transform.position;
        }

        if (activateOnStart)
            Activate();

    }

    private void OnDestroy()
    {
        LeanTween.cancel(currentTweenId);
    }

    public void GoToPositionBeforeFirstActivation()
    {
        LeanTween.cancel(currentTweenId);
        transform.position = positionBeforeFirstActivation;
    }

    public void Activate()
    {
        if (useForDebug)
            Debug.Log("tween activated " + name);

        if (firstActivation)
        {
            firstActivation = false;
            positionBeforeFirstActivation = transform.position; //override position on start
        }

        LeanTween.cancel(currentTweenId);

        OnTweenActivated?.Invoke();

        if (!useTransformHints)
            initialPosition = objectToMove.transform.position;
        else
        {
            objectToMove.transform.position = originPositionTransformHint.position;
            initialPosition = originPositionTransformHint.position;
            signedOffsetToMove = targetPositionTransformHint.position - originPositionTransformHint.position;
        }

        if (!useCustomCurve)
        {
            if (loop)
            {
                if (!useLoopCount)
                {
                    currentTweenId =
                        LeanTween.move(objectToMove, initialPosition + signedOffsetToMove, tweenDuration)
                            .setEase(tweenType)
                            .setDelay(delay)
                            .setIgnoreTimeScale(ignoreTimeScale)
                            .setLoopPingPong()
                            .setOnComplete(() =>
                            {
                                if (useForDebug)
                                    Debug.Log("tween completed " + name);

                                OnTweenCompleted?.Invoke();
                                OnTweenCompletedPublic?.Invoke(this);

                            })
                            .uniqueId;


                }
                else
                {
                    currentTweenId =
                        LeanTween.move(objectToMove, initialPosition + signedOffsetToMove, tweenDuration)
                            .setEase(tweenType)
                            .setDelay(delay)
                            .setIgnoreTimeScale(ignoreTimeScale)
                            .setLoopPingPong(loopCount)
                            .setOnComplete(() =>
                            {
                                if (useForDebug)
                                    Debug.Log("tween completed " + name);

                                OnTweenCompleted?.Invoke();
                                OnTweenCompletedPublic?.Invoke(this);
                            })
                            .uniqueId;


                }
            }
            else
            {
                currentTweenId =
                    LeanTween.move(objectToMove, initialPosition + signedOffsetToMove, tweenDuration)
                        .setEase(tweenType)
                        .setDelay(delay)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setOnComplete(() =>
                        {
                            if (useForDebug)
                                Debug.Log("tween completed " + name);

                            OnTweenCompleted?.Invoke();
                            OnTweenCompletedPublic?.Invoke(this);
                        })
                        .uniqueId;

            }
        }
        else
        {
            if (loop)
            {
                if (!useLoopCount)
                {
                    currentTweenId =
                        LeanTween.move(objectToMove, initialPosition + signedOffsetToMove, tweenDuration)
                            .setEase(customCurve)
                            .setDelay(delay)
                            .setIgnoreTimeScale(ignoreTimeScale)
                            .setLoopPingPong()
                            .setOnComplete(() =>
                            {
                                if (useForDebug)
                                    Debug.Log("tween completed " + name);

                                OnTweenCompleted?.Invoke();
                                OnTweenCompletedPublic?.Invoke(this);
                            })
                            .uniqueId;


                }
                else
                {
                    currentTweenId =
                        LeanTween.move(objectToMove, initialPosition + signedOffsetToMove, tweenDuration)
                            .setEase(customCurve)
                            .setDelay(delay)
                            .setIgnoreTimeScale(ignoreTimeScale)
                            .setLoopPingPong(loopCount)
                            .setOnComplete(() =>
                            {
                                if (useForDebug)
                                    Debug.Log("tween completed " + name);

                                OnTweenCompleted?.Invoke();
                                OnTweenCompletedPublic?.Invoke(this);
                            })
                            .uniqueId;


                }
            }
            else
            {
                currentTweenId =
                    LeanTween.move(objectToMove, initialPosition + signedOffsetToMove, tweenDuration)
                        .setEase(customCurve)
                        .setDelay(delay)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setOnComplete(() =>
                        {
                            if (useForDebug)
                                Debug.Log("tween completed " + name);

                            OnTweenCompleted?.Invoke();
                            OnTweenCompletedPublic?.Invoke(this);
                        })
                        .uniqueId;

            }

        }

    }

    public void Deactivate()
    {
        if (useForDebug)
            Debug.Log("tween deactivated " + name);

        LeanTween.cancel(currentTweenId);

    }

}
