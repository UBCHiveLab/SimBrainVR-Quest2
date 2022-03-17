using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public int rayLength = 100;
    private List<Transform> hitObjectsList; 
    
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
        line.positionCount = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        CreateLine();
        
        line.gameObject.SetActive(true);
        line.SetPosition(0, firstHitGameObject.transform.position);
      //  line.SetPosition(1, secondHitGameObject.transform.position);
        Debug.Log(firstHitGameObject.transform.position); 
        if (line.positionCount > 0)
        {
            for (int i = 1; i < line.positionCount; i++)
            {
                line.SetPosition(i, hitObjectsList[i].position);
                Debug.Log("hit list " + hitObjectsList[i].position);
            }
        }
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
                hitObjectsList.Add(firstHitGameObject.transform);
                line.positionCount++;
                Debug.Log("position count " + line.positionCount);
                Debug.Log(firstHitGameObject.name);
               
                //  lines.Add(line); 
            }
        }
        /*
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10, layerUse))
            {
                secondHitGameObject = hit.transform.gameObject;
                Debug.Log(secondHitGameObject.name);
                hitObjectsList.Add(secondHitGameObject.transform);
                line.positionCount++; 
            }      
        }
        */
    }
    
    
    
}
