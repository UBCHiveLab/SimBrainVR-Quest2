using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Specimens List", menuName = "ScriptableObjects/Specimens List")]
[System.Serializable]
public class SpecimensListSO: ScriptableObject
{
    public List<SpecimenData_New> specimens = new List<SpecimenData_New>();

    //public SpecimenDataSO[] specimens = new SpecimenDataSO[1000];

}
