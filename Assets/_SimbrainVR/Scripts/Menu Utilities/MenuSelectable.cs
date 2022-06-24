using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuSelectable : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, ISubmitHandler
{
    [SerializeField] private Selectable selectable = default;
    [SerializeField] private bool triggerOnDeselectEventWhenDisabled = false;

    [SerializeField] private UnityEvent OnPressedDown = default;
    [SerializeField] private UnityEvent OnPressedUp = default;
    [SerializeField] private UnityEvent OnHighlighted = default;

    [SerializeField] private UnityEvent OnSelected = default;
    [SerializeField] private UnityEvent OnSelectedAutomatically = default;
    [SerializeField] private UnityEvent OnDeselected = default;

    [SerializeField] private UnityEvent OnSubmitted = default;

    [HideInInspector] public UnityEvent<MenuSelectable> OnSelectedExternal = default;
    [HideInInspector] public UnityEvent<MenuSelectable> OnDeselectedExternal = default;
    [HideInInspector] public UnityEvent<MenuSelectable> OnSubmittedExternal = default;


    private bool wasSelectedAutomatically = false;

    private int startingIndex;

    private bool navigationWasTurnedOff = false;
    private Navigation lastNavigationTurnedOff = default;

    public Selectable Selectable
    {
        get => selectable;
    }

    private void Awake()
    {
        startingIndex = selectable.transform.GetSiblingIndex();
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log("on selected " + gameObject.name);

        if (wasSelectedAutomatically)
        {
            //Debug.Log("on selected automatically " + gameObject.name);
            OnSelectedAutomatically?.Invoke();

            wasSelectedAutomatically = false;
        }
        else
        {
            OnSelected?.Invoke();
        }

        OnSelectedExternal?.Invoke(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnDeselected?.Invoke();
        OnDeselectedExternal?.Invoke(this);
    }


    //used by event listeners
    public void SelectAutomatically()
    {
        Debug.Log("select automatically " + gameObject.name);
        wasSelectedAutomatically = true;

        selectable.Select();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHighlighted?.Invoke();


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressedDown?.Invoke();


    }
    public void OnPointerUp(PointerEventData eventData)
    {
        OnPressedUp?.Invoke();


    }

    public void OnSubmit(BaseEventData eventData)
    {
        OnSubmitted?.Invoke();

        OnSubmittedExternal?.Invoke(this);
    }

    public void ResetSiblingIndexToOriginal()
    {
        transform.SetSiblingIndex(startingIndex);
    }

    private void OnDisable()
    {
        if (triggerOnDeselectEventWhenDisabled)
        {
            OnDeselected?.Invoke();
            OnDeselectedExternal?.Invoke(this);
        }
    }

    public void TurnOffNavigation()
    {
        if (selectable != null && !navigationWasTurnedOff)
        {
            lastNavigationTurnedOff = selectable.navigation;

            selectable.navigation = new Navigation();

            navigationWasTurnedOff = true;
        }
    }

    public void TurnNavigationBackOn()
    {
        if (navigationWasTurnedOff)
        {
            selectable.navigation = lastNavigationTurnedOff;

            navigationWasTurnedOff = false;
        }
    }
    public void ActivateInteraction()
    {
        selectable.interactable = true;
    }
    public void DeactivateInteraction()
    {
        selectable.interactable = false;
    }
}
