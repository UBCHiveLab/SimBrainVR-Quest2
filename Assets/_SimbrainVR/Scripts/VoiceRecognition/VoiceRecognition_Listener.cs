using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class VoiceRecognition_Listener : MonoBehaviour
{
    [SerializeField] private SystemChannelBaseSO onVoiceRecognized = default;

    [SerializeField] private bool activateOnEnabled = true;
    [SerializeField] private bool deactivateOnDisabled = true;

    [SerializeField] private List<VoiceRecognitionKeyword> keywords = new List<VoiceRecognitionKeyword>();

    private void OnEnable()
    {
        if (activateOnEnabled)
            Activate();
    }

    private void OnDisable()
    {
        if (deactivateOnDisabled)
            Deactivate();


    }

    public void Activate()
    {
        onVoiceRecognized.OnEventRaised += HandleVoiceRecognized;
    }

    public void Deactivate()
    {
        onVoiceRecognized.OnEventRaised -= HandleVoiceRecognized;
    }
    
    private void HandleVoiceRecognized(SystemChannelBaseSO channel)
    {
        if (channel != null && channel.UseParameters && channel.Parameters.Count > 0)
        {
            string speech = "";

            try
            {
                speech = (string)channel.Parameters[0];
            }
            catch (System.InvalidCastException)
            {
                return;
            }

            if (speech == "")
                return;

            foreach (VoiceRecognitionKeyword keyword in keywords)
            {

                if (keyword.stringValue.Equals("") 
                    || speech.IndexOf(keyword.stringValue, StringComparison.OrdinalIgnoreCase) >= 0) //check if contains keyword (case insensitive)
                {
                    keyword.unityEvent?.Invoke(speech);

                    if (keyword.ignoreOtherKeywordsBelowWhenFired)
                        break;
                }
            }
        }

    }

}

[System.Serializable]
public class VoiceRecognitionKeyword
{
    public string stringValue = default;
    public UnityEvent<string> unityEvent = default;
    public bool ignoreOtherKeywordsBelowWhenFired = true;
}
