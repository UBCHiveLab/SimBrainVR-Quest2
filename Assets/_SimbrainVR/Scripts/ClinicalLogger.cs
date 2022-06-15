using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClinicalLogger : MonoBehaviour
{
    private static ClinicalLogger _instance;

    public static ClinicalLogger Instance { get { return _instance; } }

    public bool hasShoneLight, hasDimmedLights;

    public bool finishedIntroduction;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

}
