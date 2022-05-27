using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectDestroyer : MonoBehaviour
{
    public delegate void OnDestroyedHandler(GameObjectDestroyer objectDestroyed);
    public event OnDestroyedHandler OnDestroyed;

    public UnityEvent<GameObjectDestroyer> OnDestroyedUnityEvent = default;

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }

    public void DestroyOtherObject(GameObject objectToDestroy)
    {
        Destroy(objectToDestroy);
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
        OnDestroyedUnityEvent?.Invoke(this);
    }
}
