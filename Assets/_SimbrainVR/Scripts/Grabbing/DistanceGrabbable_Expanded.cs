using System;
using UnityEngine;
using UnityEngine.Events;
using OVRTouchSample;
using OculusSampleFramework;

public class DistanceGrabbable_Expanded : DistanceGrabbable
{
    [SerializeField] private MindPalaceWorldStateSO mindPalaceWorldState = default;

    public UnityEvent OnInRange = default;
    public UnityEvent OnOutOfRange = default;
    public UnityEvent OnTargeted = default;
    public UnityEvent OnTargetedExit = default;
    public UnityEvent OnSelected = default;
    public UnityEvent OnDeselected = default;

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
        m_renderer.GetPropertyBlock(m_mpb);
        m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorHighlighted);

    }

    public void SetToOutOfRangeOutline()
    {
        m_renderer.GetPropertyBlock(m_mpb);
        m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorOutOfRange);

    }
    public void SetToInRangeOutline()
    {
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