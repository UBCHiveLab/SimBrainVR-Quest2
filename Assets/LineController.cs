using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public int rayLength = 100; 
    private LineRenderer line;
    private List<Transform> points;
    public GameObject hitGameObject;
    public Vector3 hitGameObjectPos; 

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;

        points = new List<Transform>(); 
    }

    
    public void AddPoint(Transform point)
    {
        line.positionCount++;
        points.Add(point); 
    }

    private void LateUpdate()
    {
        if (points.Count >= 2)
        {
            for (int i = 0; i < points.Count; i++)
            {
                line.SetPosition(i, points[i].position); 
            }
        }
    }
    
}
