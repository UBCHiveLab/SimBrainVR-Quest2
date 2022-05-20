using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanTween_MoveUI_EventListener : MonoBehaviour
{
    [SerializeField] private LeanTween_MoveUI leanTween = default;
    [SerializeField] private bool activateListenerOnStart = false;
    [SerializeField] private bool activateListenerOnEnable = false;
    [SerializeField] private bool deactivateListenerOnDisable = false;
    [SerializeField] private UnityEvent OnTweenCompleted = default;

    private void Start()
    {
        if (activateListenerOnStart)
            ActivateListener();
    }
    private void OnDestroy()
    {
        DeactivateListener();
    }

    private void OnEnable()
    {
        if (activateListenerOnEnable)
            ActivateListener();

    }

    private void OnDisable()
    {
        if (deactivateListenerOnDisable)
            DeactivateListener();

    }

    public void ActivateListener()
    {
        DeactivateListener();

        if (leanTween != null)
            leanTween.OnTweenCompletedPublic.AddListener(HandleTweenCompleted);
    }
    
    public void DeactivateListener()
    {
        if (leanTween != null)
            leanTween.OnTweenCompletedPublic.RemoveListener(HandleTweenCompleted);

    }

    public void HandleTweenCompleted(LeanTween_MoveUI leanTween)
    {

        OnTweenCompleted?.Invoke();

    }

}
