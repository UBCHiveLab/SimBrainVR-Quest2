using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SystemChannelListener_UnityEvent: MonoBehaviour
{
    [SerializeField] private bool initializeOnSceneStart = true;
    [SerializeField] private bool activateOnEnable = false;
    [SerializeField] private bool deactivateOnDisable = false;

    [SerializeField] private SystemChannelBaseSO systemChannel = default;

    [SerializeField] private UnityEvent<SystemChannelBaseSO> unityEvent = default;

    private void Awake()
    {
        if (initializeOnSceneStart)
            InitializeListener();   
    }

    private void OnEnable()
    {
        if (activateOnEnable)
            InitializeListener();
    }

    private void OnDisable()
    {
        if (deactivateOnDisable)
            DisableListener();
    }

    private void OnDestroy()
    {
        DisableListener();
    }

    public void InitializeListener()
    {
        systemChannel.OnEventRaised -= HandleEventRaised;
        systemChannel.OnEventRaised += HandleEventRaised;

    }
    public void DisableListener()
    {
        systemChannel.OnEventRaised -= HandleEventRaised;

    }


    private void HandleEventRaised(SystemChannelBaseSO channel)
    {
        unityEvent?.Invoke(channel);

    }
}
