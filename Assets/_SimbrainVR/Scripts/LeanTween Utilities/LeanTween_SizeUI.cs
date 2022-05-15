using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanTween_SizeUI : MonoBehaviour
{
    [SerializeField] private bool useInDebug = default;
    [SerializeField] private RectTransform objectToUse = default;
    [SerializeField] private bool ignoreTimeScale = false;
    [SerializeField] private bool backToStartingSizeOnDisable = false;
    [SerializeField] private Vector2 offset = Vector2.zero;
    [SerializeField] private float tweenDuration = 1f;
    [SerializeField] private float delay = 0f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.linear;
    [SerializeField] private UnityEvent OnTweenCompleted = default;

    [Header("Optional Looping Settings")]
    [SerializeField] private bool useLoopPingPong = false;
    [SerializeField] private int loopCount = 3;

    private int currentTweenId;
    private Vector3 initialSize;
    private Vector3 startingSize;

    private void Start()
    {
        startingSize = objectToUse.sizeDelta;

    }

    private void OnDisable()
    {
        GoBackToStartingSize(true);
        
    }

    public void Activate()
    {
        //UNUSED and UNTESTED!!

        LeanTween.cancel(currentTweenId);


        initialSize = objectToUse.sizeDelta;

        if (useInDebug)
            Debug.Log("tween activated " + name + " " + initialSize);

        if (!useLoopPingPong)
        {
            currentTweenId =
                LeanTween.scale(objectToUse, initialSize + (Vector3) offset, tweenDuration)
                    .setFrom(initialSize)
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
            currentTweenId =
                LeanTween.scale(objectToUse, initialSize + (Vector3)offset, tweenDuration)
                    .setFrom(initialSize)
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

    public void Deactivate()
    {

        LeanTween.cancel(currentTweenId);

    }
    
    public void GoBackToStartingSize(bool alsoDeactivateTween)
    {
        if (alsoDeactivateTween)
            Deactivate();

        objectToUse.transform.localScale = startingSize;

    }

}
