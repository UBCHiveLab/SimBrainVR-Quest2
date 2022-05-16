using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnApplicationQuit_UnityEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onApplicationQuit = default;

    private void OnApplicationQuit()
    {
        onApplicationQuit?.Invoke();
    }

}
