using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public int rayLength = 100; 
    public GameObject linePrefab;
    [Space(30f)]
    public Gradient lineColour;
    public float linePointsMinDistance;
    public float lineWidth;
    LineController currentLine;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10))
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                BeginDraw(); 
            }

            if (currentLine != null)
            {
                Draw(); 
            }

            if (OVRInput.GetUp(OVRInput.Button.One))
            {
                EndDraw(); 
            }
        }
    }

    void BeginDraw()
    {
        currentLine = Instantiate(linePrefab, this.transform).GetComponent<LineController>();

        currentLine.SetLineColour(lineColour);
        currentLine.SetPointsMinDistance(linePointsMinDistance);
        currentLine.SetLineWidth(lineWidth); 
    }

    void Draw()
    {
        
    }

    void EndDraw()
    {
        if (currentLine != null)
        {
            if (currentLine.pointsCount <2)
            {
                Destroy(currentLine.gameObject);
            } else
            {
                currentLine = null; 
            }
        }
    }
}
