using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SystemChannel", menuName = "System Channels/New System Channel")]
public class SystemChannelBaseSO : ScriptableObject
{
    public delegate void OnEventRaisedHandler(SystemChannelBaseSO channel);
    public event OnEventRaisedHandler OnEventRaised;

    protected bool useParameters = false;
    protected List<object> parameters = new List<object>();

    [SerializeField] private bool useInDebug = false;
    [SerializeField] private bool useSerializedParameters = false;

    public bool UseParameters
    {
        get
        {
            return useParameters;
        }
    }

    public List<object> Parameters
    {
        get
        {
            return parameters;
        }
    }

    public void FireEvent()
    {
        if (useInDebug)
            Debug.Log("system channel fired " + name);

        if (useSerializedParameters)
        {
            InsertSerializedParameters();
        }

        OnEventRaised?.Invoke(this);
        DoSpecifics();

        useParameters = false;
        parameters.Clear();

    }
    public void FireEventWithDelayUnscaled(float delay)
    {
        LeanTween.value(0, 1, delay)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                FireEvent();
            });
    }

    public void FireEventWithDelayScaled(float delay)
    {
        LeanTween.value(0, 1, delay)
            .setOnComplete(() =>
            {
                FireEvent();
            });
    }

    public void SetParameters(List<object> parametersToSet)
    {

        useParameters = true;
        parameters = parametersToSet;

        //Debug.Log("setting external parameters " + name + " " + parameters.Count);

    }

    protected virtual void InsertSerializedParameters()
    {
        return;
    }

    protected virtual void DoSpecifics()
    {
        return;
    }

}
