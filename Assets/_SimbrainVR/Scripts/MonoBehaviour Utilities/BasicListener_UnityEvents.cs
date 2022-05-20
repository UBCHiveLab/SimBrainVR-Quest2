using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BasicListener_UnityEvents : MonoBehaviour
{
    public UnityEvent OnEnabled = default;
    public UnityEvent OnStarted = default;

    private void OnEnable()
    {
        OnEnabled?.Invoke();
    }

    void Start()
    {
        OnStarted?.Invoke();
    }
}