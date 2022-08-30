using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Mind Palace_World State", menuName = "ScriptableObjects/Mind Palace_World State SO")]
public class MindPalaceWorldStateSO : ScriptableObject
{
    public UnityEvent OnSecondGrabbableSelected = default;

    private List<MindPalace_LinkableObject> grabbablesSelected = new List<MindPalace_LinkableObject>();

    private List<MindPalace_LineBetweenGrabbables> linesInScene = new List<MindPalace_LineBetweenGrabbables>();

    public void ClearLists()
    {
        grabbablesSelected.Clear();
    }

    public void HandleGrabbableSelected(MindPalace_LinkableObject grabbable)
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
            MindPalace_LinkableObject grabbable1 = grabbablesSelected[grabbablesSelected.Count - 2];

            MindPalace_LinkableObject grabbable2 = grabbablesSelected[grabbablesSelected.Count - 1];

            MindPalace_LineBetweenGrabbables lineRenderer = Instantiate(linePrefabToClone, grabbable1.transform.position, Quaternion.identity);

            lineRenderer.OnDestroyed.AddListener(HandleLineDestroyed);

            lineRenderer.Initialize(grabbable1, grabbable2);

            linesInScene.Add(lineRenderer);

            grabbablesSelected.Clear();
        }

    }

    public void HandleGrabbableDeselected(MindPalace_LinkableObject grabbable)
    {
        grabbablesSelected.Remove(grabbable);
    }

    private void HandleLineDestroyed(MindPalace_LineBetweenGrabbables line)
    {
        linesInScene.Remove(line);

        MindPalace_LinkableObject remainingLinkableObject = line.GetRemainingNonDestroyedLinkable();

        if (grabbablesSelected.Contains(remainingLinkableObject) == false)
            grabbablesSelected.Add(remainingLinkableObject);

    }

    public void DestroyAllLinesConnectingToGrabbable(MindPalace_LinkableObject grabbable)
    {
        List<MindPalace_LineBetweenGrabbables> linesWithGrabbable = new List<MindPalace_LineBetweenGrabbables>();

        foreach (MindPalace_LineBetweenGrabbables line in linesInScene)
        {
            if (line.IsLinkedToGrabbable(grabbable))
            {
                linesWithGrabbable.Add(line);
            }
        }

        for (int i = 0; i < linesWithGrabbable.Count; i++)
        {
            linesWithGrabbable[i].DestroyLine();
        }
    }
    /*
    public void HandleLinkableObjectDestroyed(GameObject objectDestroyed)
    {
        MindPalace_LinkableObject objectToClearFromList = null;

        foreach (MindPalace_LinkableObject objectFromList in grabbablesSelected)
        {
            if (objectFromList == objectDestroyed.gameObject)
            {
                objectToClearFromList = objectFromList;
                break;
            }
        }

        if (objectToClearFromList != null)
            grabbablesSelected.Remove(objectToClearFromList);
    }*/
    public void HandleLinkableObjectDestroyed(MindPalace_LinkableObject objectDestroyed)
    {
        if (objectDestroyed != null)
        {
            grabbablesSelected.Remove(objectDestroyed);

            DestroyAllLinesConnectingToGrabbable(objectDestroyed);
        }
    }
}
