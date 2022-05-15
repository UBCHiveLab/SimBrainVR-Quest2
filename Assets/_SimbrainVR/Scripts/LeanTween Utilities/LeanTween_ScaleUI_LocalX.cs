﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanTween_ScaleUI_LocalX : MonoBehaviour
{
    [SerializeField] private bool useInDebug = default;
    [SerializeField] private RectTransform objectToUse = default;
    [SerializeField] private bool activateOnStart = false;
    [SerializeField] private bool activateOnEnable = false;
    [SerializeField] private bool deactivateOnDisable = true;
    [SerializeField] private bool ignoreTimeScale = false;
    [SerializeField] private bool backToStartingScaleOnDeactivated = false;
    [SerializeField] private float target = -1f;
    [SerializeField] private float setFrom = 1f;
    [SerializeField] private float tweenDuration = 1f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.linear;
    [SerializeField] private UnityEvent OnTweenActivated = default;
    [SerializeField] private UnityEvent OnTweenCompleted = default;
    [SerializeField] private float delay = 0f;

    [Header("Optional Looping Settings")]
    [SerializeField] private bool useLoopPingPong = false;
    [SerializeField] private bool useLoopCount = false;
    [SerializeField] private int loopCount = 3;

    private int currentTweenId;
    private Vector3 initialScale;
    private Vector3 startingScale;

    private void Start()
    {
        startingScale = objectToUse.transform.localScale;

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
        GoBackToStartingScale(deactivateOnDisable);

    }

    public void Activate()
    {
        LeanTween.cancel(currentTweenId);

        if (useInDebug)
            Debug.Log("tween activated " + name);

        OnTweenActivated?.Invoke();

        initialScale = objectToUse.transform.localScale;

        if (!useLoopPingPong)
        {
            currentTweenId =
                LeanTween.scaleX(objectToUse.gameObject, target, tweenDuration)
                    .setFrom(setFrom)
                    .setEase(tweenType)
                    .setIgnoreTimeScale(ignoreTimeScale)
                    .setDelay(delay)
                    .setOnComplete(() =>
                    {
                        OnTweenCompleted?.Invoke();

                        if (useInDebug)
                            Debug.Log("tween completed " + name);
                    })
                    .uniqueId;

        }
        else
        {
            if (!useLoopCount)
            {
                currentTweenId =
                    LeanTween.scaleX(objectToUse.gameObject, target, tweenDuration)
                        .setFrom(setFrom)
                        .setEase(tweenType)
                        .setLoopPingPong()
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setDelay(delay)
                        .setOnComplete(() =>
                        {
                            OnTweenCompleted?.Invoke();

                            if (useInDebug)
                                Debug.Log("tween completed " + name);
                        })
                        .uniqueId;



            }
            else
            {

                currentTweenId =
                    LeanTween.scaleX(objectToUse.gameObject, target, tweenDuration)
                        .setFrom(setFrom)
                        .setEase(tweenType)
                        .setLoopPingPong(loopCount)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setDelay(delay)
                        .setOnComplete(() =>
                        {
                            OnTweenCompleted?.Invoke();

                            if (useInDebug)
                                Debug.Log("tween completed " + name);
                        })
                        .uniqueId;


            }
        }
    }

    public void Deactivate()
    {

        LeanTween.cancel(currentTweenId);

        if (backToStartingScaleOnDeactivated)
            GoBackToStartingScale(false);
    }
    
    public void GoBackToStartingScale(bool alsoDeactivateTween)
    {
        if (alsoDeactivateTween)
            Deactivate();

        objectToUse.transform.localScale = startingScale;

    }

}
