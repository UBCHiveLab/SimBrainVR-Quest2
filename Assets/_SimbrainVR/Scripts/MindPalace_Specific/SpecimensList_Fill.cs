using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecimensList_Fill : MonoBehaviour
{
    [SerializeField] private SpecimensListSO specimensListSO = default;

    public void FillUsingSpecimensJson(TextAsset jsonFile)
    {
        JsonUtility.FromJsonOverwrite(jsonFile.text, specimensListSO);

        specimensListSO.SetDirty();
        //specimensListSO.specimens = new List<SpecimenDataSO>(JsonUtility.FromJson<SpecimensListSO>(jsonFile.text).specimens);
    }
}
