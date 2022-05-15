using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LeanTween_ImageColorLerp : MonoBehaviour
{
    [SerializeField] private bool activateOnStart = false;
    [SerializeField] private bool activateOnEnable = false;
    [SerializeField] private bool deactivateOnDisable = false;
    [SerializeField] private bool ignoreTimeScale = false;

    [SerializeField] private Image imageToLerp = default;

    [SerializeField] private Color startFrom = Color.white;
    [SerializeField] private Color target = Color.white;
    [SerializeField] private float duration = 1f;

    [SerializeField] private LeanTweenType easeType = LeanTweenType.easeInSine;

    [SerializeField] private UnityEvent OnLerpCompleted = default;
    [HideInInspector] public UnityEvent<LeanTween_ImageColorLerp> OnLerpCompletedPublic = default;

    [SerializeField] private bool loop = false;
    [SerializeField] private bool useLoopCount = false;
    [SerializeField] private int loopCount = 10;

    [SerializeField] private float startValue = 0f;
    [SerializeField] private float targetValue = 1f;

    private int currentTween;

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

    public void Deactivate()
    {
        LeanTween.cancel(currentTween);

    }

    public void Activate()
    {
        Deactivate();

        if (loop)
        {
            if (!useLoopCount)
            {
                currentTween = LeanTween.value(startValue, targetValue, duration)
                    .setEase(easeType)
                    .setIgnoreTimeScale(ignoreTimeScale)
                    .setLoopPingPong()
                    .setOnUpdate((float value) =>
                    {
                        imageToLerp.color = Color.Lerp(startFrom, target, value);                       
                    })
                    .setOnComplete(() =>
                    {
                        OnLerpCompleted?.Invoke();
                        OnLerpCompletedPublic?.Invoke(this);
                    })
                    .uniqueId;
            }
            else
            {
                currentTween = LeanTween.value(startValue, targetValue, duration)
                   .setEase(easeType)
                   .setIgnoreTimeScale(ignoreTimeScale)
                   .setLoopPingPong(loopCount)
                   .setOnUpdate((float value) =>
                   {
                       imageToLerp.color = Color.Lerp(startFrom, target, value);
                   })
                   .setOnComplete(() =>
                   {
                       OnLerpCompleted?.Invoke();
                       OnLerpCompletedPublic?.Invoke(this);
                   })
                   .uniqueId;
            }
        }
        else
        {
            currentTween = LeanTween.value(startValue, targetValue, duration)
                               .setEase(easeType)
                               .setIgnoreTimeScale(ignoreTimeScale)
                               .setOnUpdate((float value) =>
                               {
                                   imageToLerp.color = Color.Lerp(startFrom, target, value);
                               })
                               .setOnComplete(() =>
                               {
                                   OnLerpCompleted?.Invoke();
                               })
                               .uniqueId;
        }
    }
}
