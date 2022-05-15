using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanTween_FlashAlphaSprite : MonoBehaviour
{
    [SerializeField] private bool activateOnEnable = false;
    [SerializeField] private bool deactivateOnDisabled = false;
    [SerializeField] private GameObject objectToFlash = default;
    [SerializeField] private float tweenDuration = 1f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.once;
    [SerializeField] private bool useCustomCurve = false;
    [SerializeField] private AnimationCurve optionalCustomAnimationCurve = AnimationCurve.Constant(0f, 1f, 1f);
    [SerializeField] private float setFrom = 0f;
    [SerializeField] private float targetValue = 1f;
    [SerializeField] private bool loop = true;
    [SerializeField] private UnityEvent OnTweenCompleted = default;

    private int currentTweenId;

    private void OnEnable()
    {
        if (activateOnEnable)
            Activate();
    }
    private void OnDisable()
    {
        if (deactivateOnDisabled)
            Deactivate();
    }
    public void Activate()
    {
        LeanTween.cancel(currentTweenId);
        if (!useCustomCurve)
        {
            if (loop)
            {
                currentTweenId = LeanTween.alpha(objectToFlash, targetValue, tweenDuration)
                    .setEase(tweenType)
                    .setFrom(setFrom)
                    .setLoopPingPong()
                    .setOnComplete(() =>
                    {
                        OnTweenCompleted?.Invoke();
                    })
                    .uniqueId;

            }
            else
            {
                currentTweenId = LeanTween.alpha(objectToFlash, targetValue, tweenDuration)
                    .setEase(tweenType)
                    .setFrom(setFrom)
                    .setOnComplete(() =>
                    {
                        OnTweenCompleted?.Invoke();
                    })
                    .uniqueId;

            }

        }
        else
        {
            if (loop)
            {
                currentTweenId = LeanTween.alpha(objectToFlash, targetValue, tweenDuration)
                    .setEase(optionalCustomAnimationCurve)
                    .setFrom(setFrom)
                    .setLoopPingPong()
                    .setOnComplete(() =>
                    {
                        OnTweenCompleted?.Invoke();
                    })
                    .uniqueId;

            }
            else
            {
                currentTweenId = LeanTween.alpha(objectToFlash, targetValue, tweenDuration)
                    .setEase(optionalCustomAnimationCurve)
                    .setFrom(setFrom)
                    .setOnComplete(() =>
                    {
                        OnTweenCompleted?.Invoke();
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
