using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BasicListener_UnityEvents : MonoBehaviour
{
    public UnityEvent OnEnabled = default;
    public UnityEvent OnDisabled = default;
    public UnityEvent OnStarted = default;

    public bool useOnUpdate = false;
    public UnityEvent OnUpdate = default;

    private void OnEnable()
    {
        OnEnabled?.Invoke();
    }

    void Start()
    {
        OnStarted?.Invoke();
    }

    private void Update()
    {
        if (useOnUpdate)
            OnUpdate?.Invoke();
    }

    private void OnDisable()
    {
        OnDisabled?.Invoke();

    }
}