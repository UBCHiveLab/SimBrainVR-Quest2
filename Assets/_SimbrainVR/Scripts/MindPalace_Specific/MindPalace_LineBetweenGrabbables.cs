using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MindPalace_LineBetweenGrabbables : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer = default;

    public UnityEvent OnInitialized = default;
    public UnityEvent OnAnyPointSelected = default;

    private DistanceGrabbable_Expanded grabbable1 = default;
    private DistanceGrabbable_Expanded grabbable2 = default;

    public void Initialize(DistanceGrabbable_Expanded _grabbable1, DistanceGrabbable_Expanded _grabbable2)
    {
        grabbable1 = _grabbable1;
        grabbable2 = _grabbable2;

        lineRenderer.SetPosition(0, grabbable1.transform.position);
        lineRenderer.SetPosition(1, grabbable2.transform.position);

        StartCoroutine(SubscribeNextFrame());

        OnInitialized?.Invoke();
    }

    private IEnumerator SubscribeNextFrame()
    {
        yield return new WaitForEndOfFrame();

        grabbable1.OnSelected.AddListener(HandlePointSelected);

        grabbable2.OnSelected.AddListener(HandlePointSelected);
    }

    private void OnDestroy()
    {
        grabbable1.OnSelected.RemoveListener(HandlePointSelected);

        grabbable2.OnSelected.RemoveListener(HandlePointSelected);

    }

    private void HandlePointSelected()
    {
        OnAnyPointSelected?.Invoke();
    }

    public void DeselectBothPoints()
    {
        if (grabbable1 != null)
            grabbable1.Deselect();

        if (grabbable2 != null)
            grabbable2.Deselect();
    }
    public void DeselectBothPointsAfterFrame()
    {
        StartCoroutine(DeselectBothPointsAfterFrameCoroutine());
    }
    private IEnumerator DeselectBothPointsAfterFrameCoroutine()
    {
        yield return new WaitForEndOfFrame();

        if (grabbable1 != null)
            grabbable1.Deselect();

        if (grabbable2 != null)
            grabbable2.Deselect();
    }


    private void Update()
    {
        if (grabbable1 != null && grabbable2 != null)
        {
            lineRenderer.SetPosition(0, grabbable1.transform.position);
            lineRenderer.SetPosition(1, grabbable2.transform.position);

        }

    }

}
