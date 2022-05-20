using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanTween_MoveUI : MonoBehaviour
{
    [SerializeField] private bool useInDebug = false;
    [SerializeField] private bool activateOnStart = default;
    [SerializeField] private bool activateOnEnabled = false;
    [SerializeField] private bool deactivateOnDisable = false;
    [SerializeField] private GameObject destroyOnCompleted = null;
    [SerializeField] private bool ignoreTimeScale = default;
    [SerializeField] private RectTransform objectToMove = default;
    [SerializeField] private Vector2 signedOffsetToMove = new Vector2(0f, 10f);
    [SerializeField] private bool useTransformHints = false;
    [SerializeField] private RectTransform originPositionTransformHint = default;
    [SerializeField] private RectTransform targetPositionTransformHint = default;
    [SerializeField] private bool moveToStartingPositionFromOffset = false;
    [SerializeField] private Vector2 offsetFromStartingPosition = new Vector2(0f, 0f);
    [SerializeField] private bool randomizeOffset = false;
    [SerializeField] private bool useIntensityForRandom = false;
    [SerializeField] private Vector2 randomMinOffset = new Vector2(0f, 0f);
    [SerializeField] private Vector2 randomMaxOffset = new Vector2(0f, 0f);
    [SerializeField] private float tweenDuration = 1f;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.linear;
    [SerializeField] private bool useCustomCurve = false;
    [SerializeField] private AnimationCurve customCurve = default;
    [SerializeField] private bool loop = false;
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool useLoopCount = false;
    [SerializeField] private int loopCount = 10;
    public UnityEvent RightBeforeTweenActivated = default;
    [SerializeField] private UnityEvent OnTweenActivated = default;
    [SerializeField] private UnityEvent OnTweenCompleted = default;
    [HideInInspector] public UnityEvent<LeanTween_MoveUI> OnTweenCompletedPublic = default;


    private int currentTweenId;
    private Vector2 initialPosition;
    private Vector2 positionOnStart;
    private Vector2 positionOnAwake;
    private Vector2 positionBeforeFirstActivation;

    private bool firstActivation = true;

    private float randomIntensity = 1f;

    private void Awake()
    {
        if (objectToMove != null)
            positionOnAwake = objectToMove.anchoredPosition;

    }

    private void Start()
    {
        if (objectToMove != null)
            positionOnStart = objectToMove.anchoredPosition;

        if (activateOnStart)
            Activate();


    }

    private void OnDestroy()
    {
        Deactivate();
    }

    private void OnEnable()
    {
        if (activateOnEnabled)
            Activate();
    }


    private void OnDisable()
    {
        if (deactivateOnDisable)
        {
            Deactivate();

        }
    }

    public void SetDelay(float newDelay)
    {
        delay = newDelay;
    }

    public void ActivateAfterFrame()
    {
        StartCoroutine(ActivateAfterFrameCoroutine());
    }
    private IEnumerator ActivateAfterFrameCoroutine()
    {
        yield return new WaitForEndOfFrame();

        Activate();

    }
    public void Activate()
    {
        if (firstActivation)
        {
            firstActivation = false;
            positionBeforeFirstActivation = objectToMove.anchoredPosition;
        }

        if (useInDebug)
            Debug.Log("activating " + name + " " + objectToMove.anchoredPosition);

        LeanTween.cancel(currentTweenId);

        RightBeforeTweenActivated?.Invoke();

        if (!useTransformHints)
            initialPosition = objectToMove.anchoredPosition;
        else
        {
            objectToMove.anchoredPosition = originPositionTransformHint.anchoredPosition;
            initialPosition = originPositionTransformHint.anchoredPosition;
            signedOffsetToMove = targetPositionTransformHint.anchoredPosition - originPositionTransformHint.anchoredPosition;
        }

        if (moveToStartingPositionFromOffset && !useTransformHints)
        {
            Vector2 positionToSetFrom = positionOnAwake + offsetFromStartingPosition;
            objectToMove.anchoredPosition = positionToSetFrom;
            initialPosition = positionToSetFrom;
            signedOffsetToMove = -1f * offsetFromStartingPosition;

        }

        if (randomizeOffset && !useTransformHints) //NOT IMPLEMENTED if using with "move to starting position from offset" bool set to true
        {
            signedOffsetToMove = Vector2.Lerp(randomMinOffset, randomMaxOffset, Random.value) * (useIntensityForRandom ? randomIntensity : 1f);
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
                                OnTweenCompleted?.Invoke();
                                OnTweenCompletedPublic?.Invoke(this);

                                if (destroyOnCompleted != null)
                                    Destroy(destroyOnCompleted);
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
                                OnTweenCompleted?.Invoke();
                                OnTweenCompletedPublic?.Invoke(this);

                                if (destroyOnCompleted != null)
                                    Destroy(destroyOnCompleted);
                            })
                            .uniqueId;


                }
            }
            else
            {
                if (useInDebug)
                    Debug.Log("move from position " + objectToMove.anchoredPosition + " to " + (initialPosition + signedOffsetToMove).ToString());

                currentTweenId =
                    LeanTween.move(objectToMove, initialPosition + signedOffsetToMove, tweenDuration)
                        .setEase(tweenType)
                        .setDelay(delay)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setOnComplete(() =>
                        {
                            if (useInDebug)
                                Debug.Log("move tween completed " + name);

                            OnTweenCompleted?.Invoke();
                            OnTweenCompletedPublic?.Invoke(this);

                            if (destroyOnCompleted != null)
                                Destroy(destroyOnCompleted);
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

                                if (destroyOnCompleted != null)
                                    Destroy(destroyOnCompleted);
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

                                if (destroyOnCompleted != null)
                                    Destroy(destroyOnCompleted);
                            })
                            .uniqueId;


                }
            }
            else
            {
                if (useInDebug)
                    Debug.Log("move from position " + objectToMove.anchoredPosition + " to " + (initialPosition + signedOffsetToMove).ToString());

                currentTweenId =
                    LeanTween.move(objectToMove, initialPosition + signedOffsetToMove, tweenDuration)
                        .setEase(customCurve)
                        .setDelay(delay)
                        .setIgnoreTimeScale(ignoreTimeScale)
                        .setOnComplete(() =>
                        {
                            if (useInDebug)
                                Debug.Log("move tween completed " + name);

                            OnTweenCompleted?.Invoke();
                            OnTweenCompletedPublic?.Invoke(this);

                            if (destroyOnCompleted != null)
                                Destroy(destroyOnCompleted);
                        })
                        .uniqueId;

            }
        }

        OnTweenActivated?.Invoke();
    }

    public void ReduceIntensityByFactor(float factor)
    {
        randomIntensity = Mathf.Clamp(0, 1f, randomIntensity * (1f - factor));
    }

    public void Deactivate()
    {
        if (useInDebug)
            Debug.Log("tween deactivated " + name);

        LeanTween.cancel(currentTweenId);

    }

    public void SetObjectToMove(RectTransform newObject)
    {
        objectToMove = newObject;
        positionOnAwake = objectToMove.anchoredPosition;
    }
    public void TeleportObjectToStartingPosition()
    {
        if (positionOnStart != null)
        {
            Deactivate();
            objectToMove.anchoredPosition = positionOnStart;

        }
    }
    public void TeleportObjectToPositionBeforeFirstActivation()
    {
        if (positionBeforeFirstActivation != null)
        {
            Deactivate();
            objectToMove.anchoredPosition = positionBeforeFirstActivation;

        }
    }
    public void SetObjectToDestroyOnComplete(GameObject objectToDestroy)
    {
        destroyOnCompleted = objectToDestroy;
    }

}
