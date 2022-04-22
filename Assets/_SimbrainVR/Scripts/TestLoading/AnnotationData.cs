using System;
using UnityEngine;

/**
 * Used for serializing annotation data from json.
 */
[Serializable]
public class AnnotationData
{
    public string annotationId;
    public string title;
    public string content;
    public AnnotationNullablePosition position; // Local position of the annotation on the model

    public Vector3? positionVector3
    {
        get
        {
            if (position.global) return null;
            return new Vector3(position.x, position.y, position.z);
        }
    }

    public AnnotationData(string annotationId, string title, string content, AnnotationNullablePosition position)
    {
        this.annotationId = annotationId;
        this.title = title;
        this.content = content;
        this.position = position;
    }
}
