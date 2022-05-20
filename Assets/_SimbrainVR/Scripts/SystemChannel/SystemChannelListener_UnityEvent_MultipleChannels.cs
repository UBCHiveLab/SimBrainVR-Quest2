using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SystemChannelListener_UnityEvent_MultipleChannels : MonoBehaviour
{
    [SerializeField] private bool initializeOnSceneStart = true;
    [SerializeField] private bool activateOnEnable = false;
    [SerializeField] private bool deactivateOnDisable = false;

    [SerializeField] private List<SystemChannelBaseSO> requiredSystemChannelsToRaiseEvent = default;

    [SerializeField] private UnityEvent<SystemChannelBaseSO> onAnyChannelRaisedAllowingDuplicate = default;
    [SerializeField] private UnityEvent<SystemChannelBaseSO> onAllChannelsRaised = default;

    private List<SystemChannelBaseSO> requiredChannelsChecklist = new List<SystemChannelBaseSO>();

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
        requiredChannelsChecklist = new List<SystemChannelBaseSO>(requiredSystemChannelsToRaiseEvent);

        foreach (SystemChannelBaseSO systemChannel in requiredSystemChannelsToRaiseEvent)
        {
            systemChannel.OnEventRaised -= HandleEventRaised;
            systemChannel.OnEventRaised += HandleEventRaised;

        }

    }
    public void DisableListener()
    {
        foreach (SystemChannelBaseSO systemChannel in requiredSystemChannelsToRaiseEvent)
        {
            systemChannel.OnEventRaised -= HandleEventRaised;

        }

    }


    private void HandleEventRaised(SystemChannelBaseSO channel)
    {
        requiredChannelsChecklist.Remove(channel);

        onAnyChannelRaisedAllowingDuplicate?.Invoke(channel);

        if (requiredChannelsChecklist.Count == 0)
            onAllChannelsRaised?.Invoke(channel);

    }
}
