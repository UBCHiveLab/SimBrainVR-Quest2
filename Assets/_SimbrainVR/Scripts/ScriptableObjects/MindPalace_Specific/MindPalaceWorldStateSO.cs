using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Mind Palace_World State", menuName = "ScriptableObjects/Mind Palace_World State SO")]
public class MindPalaceWorldStateSO : ScriptableObject
{
    public UnityEvent OnSecondGrabbableSelected = default;

    private List<DistanceGrabbable_Expanded> grabbablesSelected = new List<DistanceGrabbable_Expanded>();

    public void ClearLists()
    {
        grabbablesSelected.Clear();
    }

    public void HandleGrabbableSelected(DistanceGrabbable_Expanded grabbable)
    {
        if (grabbablesSelected.Contains(grabbable) == false)
            grabbablesSelected.Add(grabbable);

        if (grabbablesSelected.Count >= 2)
        {
            OnSecondGrabbableSelected?.Invoke();
        }
    }

    public void SpawnLineRendererBetweenLastAddedPoints(MindPalace_LineBetweenGrabbables linePrefabToClone)
    {
        if (grabbablesSelected.Count >= 2)
        {
            DistanceGrabbable_Expanded grabbable1 = grabbablesSelected[0];

            DistanceGrabbable_Expanded grabbable2 = grabbablesSelected[1];

            MindPalace_LineBetweenGrabbables lineRenderer = Instantiate(linePrefabToClone, grabbable1.transform.position, Quaternion.identity);
            lineRenderer.Initialize(grabbable1, grabbable2);

            //lineRenderer.SetPosition(0, grabbable1.transform.position);
            //lineRenderer.SetPosition(1, grabbable2.transform.position);

            //grabbable1.Deselect();
            //grabbable2.Deselect();

            /*
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
            */
            //grabbablesSelected.Remove(grabbable1);
            //grabbablesSelected.Remove(grabbable2);
        }

    }

    public void HandleGrabbableDeselected(DistanceGrabbable_Expanded grabbable)
    {
        grabbablesSelected.Remove(grabbable);
    }
}
