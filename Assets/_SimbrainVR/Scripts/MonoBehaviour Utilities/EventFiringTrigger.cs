using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(Collider2D))] //causes bugs when destroying object
public class EventFiringTrigger : MonoBehaviour
{
    public delegate void TriggerEventHandler(Collider thisCollider, Collider otherCollider);
    public event TriggerEventHandler OnTriggerEntered;
    public event TriggerEventHandler OnTriggerExited;
    public event TriggerEventHandler OnTriggerStayed;

    public UnityEvent<Collider, Collider> OnTriggerEnteredUnityEvent;
    public UnityEvent<Collider, Collider> OnTriggerExitedUnityEvent;
    public UnityEvent<Collider, Collider> OnTriggerStayedUnityEvent;

    [SerializeField] private bool debug = false;
    [SerializeField] private bool useTriggerStay = false;
    [SerializeField] private Collider trigger = default;
    [SerializeField] private bool applyLayeringToRigidbodyInsteadOfCollider = false;
    [SerializeField] private LayerMask validLayers = default;

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (debug)
        {
            Debug.Log("trigger enter " + otherCollider.name);
        }

        if (!applyLayeringToRigidbodyInsteadOfCollider && IsInValidLayer(otherCollider) == false)
            return;

        if (applyLayeringToRigidbodyInsteadOfCollider)
        {
            if (otherCollider.attachedRigidbody == null)
                return;

            if (debug)
            {
                Debug.Log("trigger enter " + otherCollider.attachedRigidbody.name);
            }

            if (IsInValidLayer(otherCollider.attachedRigidbody) == false)
                return;

        }

        if (debug)
        {
            Debug.Log("trigger enter " + name + " with " + otherCollider.name);
        }

        OnTriggerEntered?.Invoke(trigger, otherCollider);
        OnTriggerEnteredUnityEvent?.Invoke(trigger, otherCollider);
    }
    private void OnTriggerExit(Collider otherCollider)
    {
        if (!applyLayeringToRigidbodyInsteadOfCollider && IsInValidLayer(otherCollider) == false)
            return;

        if (applyLayeringToRigidbodyInsteadOfCollider)
        {
            if (otherCollider.attachedRigidbody == null)
                return;

            if (IsInValidLayer(otherCollider.attachedRigidbody) == false)
                return;

        }

        if (debug)
        {
            Debug.Log("trigger exit " + name + " with " + otherCollider.name);
        }

        OnTriggerExited?.Invoke(trigger, otherCollider);
        OnTriggerExitedUnityEvent?.Invoke(trigger, otherCollider);

    }

    private void OnTriggerStay(Collider otherCollider)
    {
        if (!useTriggerStay)
            return;

        if (!applyLayeringToRigidbodyInsteadOfCollider && IsInValidLayer(otherCollider) == false)
            return;

        if (applyLayeringToRigidbodyInsteadOfCollider)
        {
            if (otherCollider.attachedRigidbody == null)
                return;

            if (IsInValidLayer(otherCollider.attachedRigidbody) == false)
                return;

        }

        //if (debug)
        //{
        //    Debug.Log("trigger stay " + name + " with " + collision.gameObject.name);
        //}

        OnTriggerStayed?.Invoke(trigger, otherCollider);
        OnTriggerStayedUnityEvent?.Invoke(trigger, otherCollider);

    }

    private bool IsInValidLayer(Collider collider)
    {
        return (validLayers == (validLayers | (1 << collider.gameObject.layer)));
    }

    private bool IsInValidLayer(Rigidbody rigidbody)
    {
        return (validLayers == (validLayers | (1 << rigidbody.gameObject.layer)));
    }

}
