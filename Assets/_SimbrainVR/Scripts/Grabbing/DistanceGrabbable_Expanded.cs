using System;
using UnityEngine;
using UnityEngine.Events;
using OVRTouchSample;
using OculusSampleFramework;

public class DistanceGrabbable_Expanded : DistanceGrabbable
{
    [SerializeField] private MindPalaceWorldStateSO mindPalaceWorldState = default;
    [SerializeField] private Transform referencePosition = default;

    public UnityEvent OnInRange = default;
    public UnityEvent OnOutOfRange = default;
    public UnityEvent OnTargeted = default;
    public UnityEvent OnTargetedExit = default;
    public UnityEvent OnSelected = default;
    public UnityEvent OnDeselected = default;

    public virtual Transform ReferencePosition
    {
        get
        {
            if (referencePosition != null)
                return referencePosition;

            return transform;
        }
    }

    public override bool InRange
    {
        get { return m_inRange; }
        set
        {
            bool oldValue = InRange;

            m_inRange = value;

            if (InRange != oldValue)
            {
                if (InRange)
                {
                    OnInRange?.Invoke();
                }
                else
                {
                    OnOutOfRange?.Invoke();

                }
            }
            //RefreshCrosshair();
        }
    }

    public override bool Targeted
    {
        get { return m_targeted; }
        set
        {
            bool oldValue = Targeted;

            m_targeted = value;

            if (Targeted != oldValue)
            {
                if (Targeted)
                {
                    OnTargeted?.Invoke();
                }
                else
                {
                    OnTargetedExit?.Invoke();

                }
            }

            //RefreshCrosshair();
        }
    }
    //bool m_targeted;

    public void SetToHighlightedOutline()
    {
        if (m_renderer != null)
            m_renderer.GetPropertyBlock(m_mpb);

        if (m_mpb != null)
            m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorHighlighted);

    }

    public void SetToOutOfRangeOutline()
    {
        if (m_renderer != null)
            m_renderer.GetPropertyBlock(m_mpb);

        m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorOutOfRange);

    }
    public void SetToInRangeOutline()
    {
        if (m_renderer != null)
            m_renderer.GetPropertyBlock(m_mpb);

        m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorInRange);

    }

    public void Select()
    {
        OnSelected?.Invoke();

        mindPalaceWorldState.HandleGrabbableSelected(this);
    }

    public void Deselect()
    {
        OnDeselected?.Invoke();

        mindPalaceWorldState.HandleGrabbableDeselected(this);
    }

}