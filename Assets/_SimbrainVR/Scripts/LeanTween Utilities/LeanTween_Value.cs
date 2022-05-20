using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanTween_Value : MonoBehaviour
{
    [SerializeField] private bool useInDebug = default;
    [SerializeField] private bool dontActivateIfAlreadyTweening = false;
    [SerializeField] private bool activateOnStart = default;
    [SerializeField] private bool activateOnEnable = default;
    [SerializeField] private bool deactivateOnDisable = default;
    [SerializeField] private bool ignoreTimeScale = default;
    [SerializeField] private float minValue = 0f;
    [SerializeField] private float maxValue = 1f;
    [SerializeField] private float tweenDuration = 1f;
    [SerializeField] private Vector2 minMaxDurationWhenRandomized = default;
    [SerializeField] private float delay = 0f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.linear;
    [SerializeField] private bool loop = false;
    [SerializeField] private bool useLoopCount = false;
    [SerializeField] private int loopCount = 10;
    [SerializeField] private UnityEvent<float> OnUpdate = default;
    [SerializeField] private UnityEvent OnTweenActivated = default;
    [SerializeField] private UnityEvent OnTweenCompleted = default;
    [HideInInspector] public UnityEvent<LeanTween_Value> OnTweenCompletedPublic = default;


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

    public bool IsTweening()
    {
        return LeanTween.isTweening(currentTweenId);
    }

    public void Activate()
    {
        if (dontActivateIfAlreadyTweening && IsTweening())
            return;

        LeanTween.cancel(currentTweenId);

        if (useInDebug)
            Debug.Log("tween activated " + name);

        OnTweenActivated?.Invoke();

        if (loop)
        {
            if (!useLoopCount)
            {
                currentTweenId =
                    LeanTween.value(minValue, maxValue, tweenDuration)
                        .setEase(tweenType)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setLoopPingPong()
                        .setDelay(delay)
                        .setOnUpdate((float value) =>
                        {
                            OnUpdate?.Invoke(value);
                        })
                        .setOnComplete(() =>
                        {
                            OnTweenCompleted?.Invoke();
                            OnTweenCompletedPublic?.Invoke(this);

                            if (useInDebug)
                                Debug.Log("tween completed " + name);
                        })
                        .uniqueId;


            }
            else
            {
                currentTweenId =
                    LeanTween.value(minValue, maxValue, tweenDuration)
                        .setEase(tweenType)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setLoopPingPong(loopCount)
                        .setDelay(delay)
                        .setOnUpdate((float value) =>
                        {
                            OnUpdate?.Invoke(value);
                        })
                        .setOnComplete(() =>
                        {
                            OnTweenCompleted?.Invoke();
                            OnTweenCompletedPublic?.Invoke(this);

                            if (useInDebug)
                                Debug.Log("tween completed " + name);
                        })
                        .uniqueId;


            }
        }
        else
        {
            currentTweenId =
                    LeanTween.value(minValue, maxValue, tweenDuration)
                    .setEase(tweenType)
                    .setIgnoreTimeScale(ignoreTimeScale)
                    .setDelay(delay)
                    .setOnUpdate((float value) =>
                    {
                        OnUpdate?.Invoke(value);
                    })
                    .setOnComplete(() =>
                    {
                        OnTweenCompleted?.Invoke();
                        OnTweenCompletedPublic?.Invoke(this);

                        if (useInDebug)
                            Debug.Log("tween completed " + name);
                    })
                    .uniqueId;

        }
    }

    public void Deactivate()
    {
        LeanTween.cancel(currentTweenId);

    }

    public void RandomizeDuration()
    {
        tweenDuration = Random.Range(minMaxDurationWhenRandomized.x, minMaxDurationWhenRandomized.y);
    }
}
