using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public int rayLength = 100;
    private List<Transform> hitObjectsList;
    private List<Transform> firstHitObjectsList;
    private List<Transform> secondHitObjectsList; 
    private Transform secondHitPos;
    private Transform firstHitPos; 
 //   public GameObject linePrefab;
    public LineRenderer line;
    GameObject firstHitGameObject;
    GameObject secondHitGameObject;
    int layerUse = ~1;
    Transform end;
    Transform start;
    Vector3 startDrawingPos = new Vector3();
    Vector3 endDrawingPos = new Vector3();
    [Header("Lines")]
    [SerializeField] Transform lineParent;
    [SerializeField] private GameObject linePrefab;
    private LineController currentLine;

    [Header("Dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] Transform dotParent; 

    // Start is called before the first frame update
    void Start()
    {
        hitObjectsList = new List<Transform>();
     //   line.positionCount = 0;
        firstHitObjectsList = new List<Transform>();
        secondHitObjectsList = new List<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        CreateLine();
        PressObject();
       
        line.gameObject.SetActive(true);
        line.SetPosition(0, firstHitGameObject.transform.position);
        line.SetPosition(1, secondHitGameObject.transform.position);
       /*
        if (line.positionCount > 1)
        {
            for (int i = 2; i < line.positionCount; i++)
            {
                line.SetPosition(i, hitObjectsList[i].position);
               // Debug.Log("hit list " + hitObjectsList[i].position);
            }
        }
        */
    //    line.SetPosition(0, firstHitGameObject.transform.position);
      //  line.SetPosition(1, secondHitGameObject.transform.position);
    }
    
    
    void CreateLine()
    {
        RaycastHit hit; // did the ray make contact with an object
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10, layerUse))
            {
                firstHitGameObject = hit.transform.gameObject;
                firstHitObjectsList.Add(firstHitGameObject.transform);
                line = Instantiate(linePrefab, firstHitGameObject.transform).GetComponent<LineRenderer>();
                firstHitPos = firstHitGameObject.transform; 
                hitObjectsList.Add(firstHitGameObject.transform);
              
           //     line.positionCount++;
           //     Debug.Log("position count " + line.positionCount);
            //    Debug.Log(firstHitGameObject.name);
               
                //  lines.Add(line); 
            }
        }
        
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10, layerUse))
            {
                secondHitGameObject = hit.transform.gameObject;
                secondHitObjectsList.Add(secondHitGameObject.transform); 
                firstHitGameObject.GetComponent<ObjectData>().AddConnections(secondHitGameObject);
                secondHitGameObject.GetComponent<ObjectData>().AddConnections(firstHitGameObject); 
                secondHitPos = secondHitGameObject.transform; 
            //    Debug.Log(secondHitGameObject.name);
                hitObjectsList.Add(secondHitGameObject.transform);
            //    line.positionCount++;
            //    hitGameObjectsList.Add(new Tuple<firstHitPos, secondHitPos>()); 
            }      
        }
        
    }

    void PressObject()
    {
        RaycastHit hit; 
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10, layerUse))
        {
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                GameObject hitObject = hit.transform.gameObject;
              //  Debug.Log(hitObject.name);
                Debug.Log("listofObjects " + hitObject.GetComponent<ObjectData>().DisplayConnections()); 
            }

            if (OVRInput.Get(OVRInput.RawButton.X))
            {
                GameObject hitObject = hit.transform.gameObject; 
                if (hitObject.ToString() == "Line")
                {
                    Debug.Log("POP-up should be enabled"); 
                }
            }
        }
    }
    
    
    
}
