using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Set Player Prefs", menuName = "ScriptableObjects/UnityEvent Commands/Set Player Prefs")]
public class SetPlayerPrefsSO : ScriptableObject
{
    [SerializeField] private string key = default;

    public void SetPlayerPrefsInt(int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
    public void SetPlayerPrefsFloat(float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }
}
