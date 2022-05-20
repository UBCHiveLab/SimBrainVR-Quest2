using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanTween_FlashAlpha : MonoBehaviour
{
    [SerializeField] private bool useInDebug = false;
    [SerializeField] private CanvasGroup objectToFlash = default;
    [SerializeField] private bool activateOnStart = false;
    [SerializeField] private bool activateOnEnabled = false;
    [SerializeField] private bool deactivateOnDisabled = false;
    [SerializeField] private bool ignoreTimeScale = false;
    [SerializeField] private float tweenDuration = 1f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.once;
    [SerializeField] private bool useCustomCurve = false;
    [SerializeField] private AnimationCurve customCurve = AnimationCurve.EaseInOut(0f, 1f, 0f, 1f);
    [SerializeField] private float setFrom = 0f;
    [SerializeField] private float targetValue = 1f;
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool useLoopCount = false;
    [SerializeField] private int loopCount = 10;
    [SerializeField] private UnityEvent OnTweenActivated = default;
    [SerializeField] private UnityEvent OnTweenCompleted = default;

    private int currentTweenId;

    private void Start()
    {
        if (activateOnStart)
            Activate();

    }
    private void OnEnable()
    {
        if (activateOnEnabled)
            Activate();

    }

    private void OnDisable()
    {
        if (deactivateOnDisabled)
            Deactivate();

    }

    public void Activate()
    {
        if (useInDebug)
            Debug.Log("tween activated " + name);

        LeanTween.cancel(currentTweenId);

        if (useCustomCurve)
        {
            if (loop)
            {
                if (useLoopCount)
                {
                    currentTweenId = LeanTween.alphaCanvas(objectToFlash, targetValue, tweenDuration)
                        .setEase(customCurve)
                        .setFrom(setFrom)
                        .setLoopPingPong(loopCount)
                        .setDelay(delay)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setOnComplete(() =>
                        {
                            if (useInDebug)
                                Debug.Log("tween completed " + name);

                            OnTweenCompleted?.Invoke();
                        })
                        .uniqueId;

                }
                else
                {

                    currentTweenId = LeanTween.alphaCanvas(objectToFlash, targetValue, tweenDuration)
                        .setEase(customCurve)
                        .setFrom(setFrom)
                        .setLoopPingPong()
                        .setDelay(delay)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setOnComplete(() =>
                        {
                            if (useInDebug)
                                Debug.Log("tween completed " + name);

                            OnTweenCompleted?.Invoke();
                        })
                        .uniqueId;
                }

            }
            else
            {
                currentTweenId = LeanTween.alphaCanvas(objectToFlash, targetValue, tweenDuration)
                    .setEase(customCurve)
                    .setFrom(setFrom)
                    .setDelay(delay)
                    .setIgnoreTimeScale(ignoreTimeScale)
                    .setOnComplete(() =>
                    {
                        if (useInDebug)
                            Debug.Log("tween completed " + name);

                        OnTweenCompleted?.Invoke();
                    })
                    .uniqueId;

            }

        }
        else
        {
            if (loop)
            {
                if (useLoopCount)
                {
                    currentTweenId = LeanTween.alphaCanvas(objectToFlash, targetValue, tweenDuration)
                        .setEase(tweenType)
                        .setFrom(setFrom)
                        .setLoopPingPong(loopCount)
                        .setDelay(delay)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setOnComplete(() =>
                        {
                            if (useInDebug)
                                Debug.Log("tween completed " + name);

                            OnTweenCompleted?.Invoke();
                        })
                        .uniqueId;

                }
                else
                {

                    currentTweenId = LeanTween.alphaCanvas(objectToFlash, targetValue, tweenDuration)
                        .setEase(tweenType)
                        .setFrom(setFrom)
                        .setLoopPingPong()
                        .setDelay(delay)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setOnComplete(() =>
                        {
                            if (useInDebug)
                                Debug.Log("tween completed " + name);

                            OnTweenCompleted?.Invoke();
                        })
                        .uniqueId;
                }

            }
            else
            {
                currentTweenId = LeanTween.alphaCanvas(objectToFlash, targetValue, tweenDuration)
                    .setEase(tweenType)
                    .setFrom(setFrom)
                    .setDelay(delay)
                    .setIgnoreTimeScale(ignoreTimeScale)
                    .setOnComplete(() =>
                    {
                        if (useInDebug)
                            Debug.Log("tween completed " + name);

                        OnTweenCompleted?.Invoke();
                    })
                    .uniqueId;

            }

        }


        OnTweenActivated?.Invoke();
    }

    public void Deactivate()
    {
        if (useInDebug)
            Debug.Log("tween deactivated " + name);

        LeanTween.cancel(currentTweenId);

    }

    public bool IsTweening()
    {
        return LeanTween.isTweening(currentTweenId);
    }
    public void SetObjectToFlash(CanvasGroup newObject)
    {
        objectToFlash = newObject;
    }
}
