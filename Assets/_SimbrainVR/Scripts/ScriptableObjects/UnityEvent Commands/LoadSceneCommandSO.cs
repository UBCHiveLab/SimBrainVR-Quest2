using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LoadSceneCommand_", menuName = "ScriptableObjects/UnityEvent Commands/Load Scene")]
public class LoadSceneCommandSO : ScriptableObject
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }

}
