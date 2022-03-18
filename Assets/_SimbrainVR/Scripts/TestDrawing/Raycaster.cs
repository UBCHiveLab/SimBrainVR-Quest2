using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public int rayLength = 100;
    private List<LineRenderer> lines; 
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
        lines = new List<LineRenderer>(); 
        line.positionCount = 0;
        firstHitObjectsList = new List<Transform>();
        secondHitObjectsList = new List<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        CreateLine();
        PressObject();
       
        line.gameObject.SetActive(true);
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].SetPosition(0, firstHitObjectsList[i].position);
            lines[i].SetPosition(1, secondHitObjectsList[i].position); 
        }
        /*
        if (line.positionCount >= 0)
        { 
            for (int i = 0; i < line.positionCount; i++)
            {
                line.SetPosition(i,hitObjectsList[i].position); 
               // Debug.Log("hit list " + hitObjectsList[i].position);
            }
        }*/
        
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
                lines.Add(line);
                firstHitPos = firstHitGameObject.transform; 
                hitObjectsList.Add(firstHitGameObject.transform);
              
              //  line.positionCount++;
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
             //   line.positionCount++;
          
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
                hitObject.GetComponent<ObjectData>().DisplayConnections();
                Debug.Log("list of Objects" + hitObject.GetComponent<ObjectData>().DisplayCountOfConnections()); 
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
