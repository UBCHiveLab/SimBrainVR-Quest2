using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Add Component_Canvas", menuName = "ScriptableObjects/UnityEvent Commands/Add Component_Canvas")]
public class AddComponent_Canvas_SO : ScriptableObject
{
    public void Activate(GameObject gameObject)
    {
        gameObject.AddComponent<Canvas>();
        
    }
}
