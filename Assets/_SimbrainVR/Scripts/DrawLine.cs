
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRInput;

public class DrawLine : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;
    public List<Vector2> fingerPositions;

    public Controller controller; 
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(OVRInput.GetDown(OVRInput.Button.One));
        Debug.Log(OVRInput.Get(OVRInput.Button.One));
    }

    // Update is called once per frame
    void Update()
    {

        Drawing(); 

    }

    public void Drawing()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
            {

        }
    }
    void CreateLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
        fingerPositions.Clear(); //Not sure if this is required since it's more for clearing - don;t think we need to allow clearing of lines
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick)));
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick))); 
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);
        edgeCollider.points = fingerPositions.ToArray(); 


    }

    void UpdateLine(Vector2 newFingerPosition)
    {
        fingerPositions.Add(newFingerPosition);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPosition);
        edgeCollider.points = fingerPositions.ToArray(); 

    }
}
