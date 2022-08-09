using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[CreateAssetMenu(fileName = "Specimen Data SO", menuName = "ScriptableObjects/Specimen Data")]
[System.Serializable]
public class SpecimenData_New
{
    public GameObject localPrefab = default;
    public SpecimenData_DummyIdString _id;
    public string id;
    public string name;
    public int version;
    public string organ;
    public string assetUrl;
    public string assetUrlWebGl;
    public string assetUrlOsx;
    public string altAssetUrl;
    public string prefabPath;
    public double scale;
    public double yPos;
    public List<AnnotationData_New> annotations;

}

[System.Serializable]
public class SpecimenData_DummyIdString
{
    public string dummyId;
}

[System.Serializable]
public class AnnotationData_New
{
    public string annotationId;
    public string title;
    public string content;
    public AnnotationData_Position position; // Local position of the annotation on the model

}

[System.Serializable]
public class AnnotationData_Position
{
    public double x = default;

    public double y = default;

    public double z = default;
    
    public bool global = default;
}
