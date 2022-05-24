using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Mind Palace_World State", menuName = "ScriptableObjects/Mind Palace_World State SO")]
public class MindPalaceWorldStateSO : ScriptableObject
{
    public UnityEvent OnSecondGrabbableSelected = default;

    private List<OVRGrabbable> grabbablesSelected = new List<OVRGrabbable>();

    public void ClearLists()
    {
        grabbablesSelected.Clear();
    }

    public void HandleGrabbableSelected(OVRGrabbable grabbable)
    {
        if (grabbablesSelected.Contains(grabbable) == false)
            grabbablesSelected.Add(grabbable);

        if (grabbablesSelected.Count >= 2)
        {
            OnSecondGrabbableSelected?.Invoke();
        }
    }

    public void SpawnLineRendererBetweenLastAddedPoints(LineRenderer linePrefabToClone)
    {
        if (grabbablesSelected.Count >= 2)
        {
            OVRGrabbable grabbable1 = grabbablesSelected[0];

            OVRGrabbable grabbable2 = grabbablesSelected[1];

            LineRenderer lineRenderer = Instantiate(linePrefabToClone, grabbable1.transform.position, Quaternion.identity);
            lineRenderer.SetPosition(0, grabbable1.transform.position);
            lineRenderer.SetPosition(1, grabbable2.transform.position);

            try
            {
                DistanceGrabbable_Expanded expandedGrabbable1 = (DistanceGrabbable_Expanded)grabbable1;
                expandedGrabbable1.Deselect();

            }
            catch (System.InvalidCastException)
            {
            }

            try
            {
                DistanceGrabbable_Expanded expandedGrabbable2 = (DistanceGrabbable_Expanded)grabbable2;
                expandedGrabbable2.Deselect();
            }
            catch (System.InvalidCastException)
            {
            }

            grabbablesSelected.Remove(grabbable1);
            grabbablesSelected.Remove(grabbable2);
        }

    }

    public void HandleGrabbableDeselected(OVRGrabbable grabbable)
    {
        grabbablesSelected.Remove(grabbable);
    }
}
