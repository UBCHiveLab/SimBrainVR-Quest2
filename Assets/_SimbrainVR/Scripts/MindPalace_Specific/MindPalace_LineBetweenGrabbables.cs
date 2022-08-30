using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MindPalace_LineBetweenGrabbables : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer = default;

    public UnityEvent OnInitialized = default;
    public UnityEvent OnAnyPointSelected = default;

    public UnityEvent<MindPalace_LineBetweenGrabbables> OnDestroyed = default;

    private MindPalace_LinkableObject grabbable1 = default;
    private MindPalace_LinkableObject grabbable2 = default;

    public void Initialize(MindPalace_LinkableObject _grabbable1, MindPalace_LinkableObject _grabbable2)
    {
        grabbable1 = _grabbable1;
        grabbable2 = _grabbable2;

        lineRenderer.SetPosition(0, grabbable1.ReferencePosition.position);
        lineRenderer.SetPosition(1, grabbable2.ReferencePosition.position);

        //StartCoroutine(SubscribeNextFrame());

        OnInitialized?.Invoke();
    }

    private IEnumerator SubscribeNextFrame()
    {
        yield return new WaitForEndOfFrame();

        //grabbable1.OnSelected.AddListener(HandlePointSelected);

        //grabbable2.OnSelected.AddListener(HandlePointSelected);
    }

    private void OnDestroy()
    {
        //grabbable1.OnSelected.RemoveListener(HandlePointSelected);

        //grabbable2.OnSelected.RemoveListener(HandlePointSelected);

        OnDestroyed?.Invoke(this);
    }

    private void HandlePointSelected()
    {
        OnAnyPointSelected?.Invoke();
    }

    public void DeselectBothPoints()
    {
        /*
        if (grabbable1 != null)
            grabbable1.Deselect();

        if (grabbable2 != null)
            grabbable2.Deselect();
        */
    }
    public void DeselectBothPointsAfterFrame()
    {
        StartCoroutine(DeselectBothPointsAfterFrameCoroutine());
    }
    private IEnumerator DeselectBothPointsAfterFrameCoroutine()
    {
        yield return new WaitForEndOfFrame();

        DeselectBothPoints();
    }


    private void Update()
    {
        if (grabbable1 != null && grabbable2 != null)
        {
            lineRenderer.SetPosition(0, grabbable1.ReferencePosition.position);
            lineRenderer.SetPosition(1, grabbable2.ReferencePosition.position);

        }

    }

    public bool IsLinkedToGrabbable(MindPalace_LinkableObject grabbable)
    {
        if (grabbable1 == grabbable)
            return true;

        if (grabbable2 == grabbable)
            return true;

        return false;
    }

    public void DestroyLine()
    {
        Destroy(gameObject);
    }

    public MindPalace_LinkableObject GetRemainingNonDestroyedLinkable()
    {
        if (!grabbable1.WasDestroyed)
            return grabbable1;

        if (!grabbable2.WasDestroyed)
            return grabbable2;

        return null;
    }
}
