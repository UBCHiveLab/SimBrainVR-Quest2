using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public int rayLength = 100; 
    private LineRenderer lineRenderer;
    public Rigidbody rigidBody;
    public BoxCollider boxCollider; 
  
    public GameObject hitGameObject;
    public Vector3 hitGameObjectPos;
    [HideInInspector] public List<Vector3> points = new List<Vector3>();
    [HideInInspector] public int pointsCount;
    float pointsMinDistance = 0.1f; 

    public void AddPoint(Vector3 newPoint)
    {
        if (pointsCount >= 1 && Vector3.Distance(newPoint, GetLastPoint()) < pointsMinDistance)
            return;

        points.Add(newPoint);
        pointsCount++;

        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPosition(pointsCount - 1, newPoint); 
    }

    public Vector3 GetLastPoint()
    {
        return (Vector3)lineRenderer.GetPosition(pointsCount - 1); 
    }
    public void SetLineColour(Gradient colorGradient)
    {
        lineRenderer.colorGradient = colorGradient; 
    }
    public void SetPointsMinDistance(float distance)
    {
        pointsMinDistance = distance; 
    }
    public void SetLineWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width; 

       
    }

}
