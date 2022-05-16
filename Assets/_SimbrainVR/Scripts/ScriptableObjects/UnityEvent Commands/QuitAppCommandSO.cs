using UnityEngine;

[CreateAssetMenu(fileName = "Quit App_Command", menuName = "ScriptableObjects/UnityEvent Commands/Quit App")]
public class QuitAppCommandSO : ScriptableObject
{
    public void QuitApp()
    {
        Application.Quit();
    }
}
