using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Dont Destroy on Load Command SO", menuName = "ScriptableObjects/UnityEvent Commands/Dont Destroy On Load")]
public class DontDestroyOnLoadSO : ScriptableObject
{
    public void SetDontDestroyOnLoad(GameObject gameObject)
    {
        gameObject.transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }
}
