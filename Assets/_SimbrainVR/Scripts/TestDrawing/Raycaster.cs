using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public int rayLength = 100; 
    private bool aboutToDraw = false;
    public GameObject linePrefab; 
    public LineRenderer line;
    GameObject firstHitGameObject;
    GameObject secondHitGameObject; 
    Transform end;
    Transform start; 
    Vector3 startDrawingPos = new Vector3();
    Vector3 endDrawingPos = new Vector3(); 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; // did the ray make contact with an object
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength*10))
            {
                Debug.Log("object detected"); 
                aboutToDraw = true;
                start = hit.transform;
                startDrawingPos = hit.point;
                firstHitGameObject = hit.transform.gameObject;
                string firstHitFeedback = firstHitGameObject.name;
                Debug.Log(hit.transform.gameObject.name); 
                line = Instantiate(linePrefab, firstHitGameObject.transform).GetComponent<LineRenderer>();
                line.gameObject.SetActive(true);
                Debug.DrawLine(startDrawingPos, transform.forward * rayLength * 10); 
                Debug.Log("startDrawingPos" + startDrawingPos); 
            }
        }
        if (OVRInput.GetUp(OVRInput.RawButton.A))
        {
            Debug.Log("X is pressed at secondPos");
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10))
            {
                end = hit.transform;
                endDrawingPos = hit.point;
                secondHitGameObject = hit.transform.gameObject;
                string secondHitFeedback = secondHitGameObject.name;
                Debug.Log(hit.transform.gameObject.name);
                line.SetPosition(0, firstHitGameObject.transform.position);
                line.SetPosition(1, secondHitGameObject.transform.position);
                Debug.Log("startDrawingPos after last point" + startDrawingPos);
              //  line.positionCount = 2;
            }
        }


    }
}
