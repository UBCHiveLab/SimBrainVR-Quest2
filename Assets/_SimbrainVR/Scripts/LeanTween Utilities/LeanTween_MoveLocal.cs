using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanTween_MoveLocal : MonoBehaviour
{
    [SerializeField] private bool activateOnStart = default;
    [SerializeField] private bool activateOnEnable = default;
    [SerializeField] private bool deactivateOnDisable = default;
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
    [SerializeField] private UnityEvent OnTweenCompleted = default;
    [HideInInspector] public UnityEvent<LeanTween_MoveLocal> OnTweenCompletedPublic = default;


    private int currentTweenId;
    private Vector2 initialPosition;

    private void Start()
    {
        if (activateOnStart)
            Activate();

    }

    private void OnEnable()
    {
        if (activateOnEnable)
            Activate();

    }

    private void OnDisable()
    {
        if (deactivateOnDisable)
            Deactivate();


    }

    private void OnDestroy()
    {
        LeanTween.cancel(currentTweenId);
    }

    public void Activate()
    {
        LeanTween.cancel(currentTweenId);

        if (!useTransformHints)
            initialPosition = objectToMove.transform.localPosition;
        else
        {
            objectToMove.transform.localPosition = originPositionTransformHint.localPosition;
            initialPosition = originPositionTransformHint.localPosition;
            signedOffsetToMove = targetPositionTransformHint.localPosition - originPositionTransformHint.localPosition;
        }

        if (!useCustomCurve)
        {
            if (loop)
            {
                if (!useLoopCount)
                {
                    currentTweenId =
                        LeanTween.moveLocal(objectToMove, initialPosition + signedOffsetToMove, tweenDuration)
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
                        LeanTween.moveLocal(objectToMove, initialPosition + signedOffsetToMove, tweenDuration)
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
                    LeanTween.moveLocal(objectToMove, initialPosition + signedOffsetToMove, tweenDuration)
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
                            OnTweenCompleted?.Invoke();
                            OnTweenCompletedPublic?.Invoke(this);
                        })
                        .uniqueId;

            }

        }

    }

    public void Deactivate()
    {
        LeanTween.cancel(currentTweenId);

    }

}
