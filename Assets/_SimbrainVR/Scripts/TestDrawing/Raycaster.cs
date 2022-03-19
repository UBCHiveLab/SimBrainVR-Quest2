using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class Raycaster : MonoBehaviour
{
    public int rayLength = 100;
    public Button button; 
    private List<LineRenderer> lines; 
    private List<Transform> hitObjectsList;
    private List<Transform> firstHitObjectsList;
    private List<Transform> secondHitObjectsList; 
   

 //   public GameObject linePrefab;
    public LineRenderer line;
    GameObject firstHitGameObject;
    GameObject secondHitGameObject;
    int layerUse = ~1;
    public GameObject linePrefab;

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
        OpenMenu();
        line.gameObject.SetActive(true);
        //Iterates through the list of lines and positions. Sets each line's posiiton every frame to ensure it stays with it
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
        //Sets the first position of the line
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10, layerUse))
            {
                firstHitGameObject = hit.transform.gameObject;
                firstHitObjectsList.Add(firstHitGameObject.transform);
                line = Instantiate(linePrefab, firstHitGameObject.transform).GetComponent<LineRenderer>();
                lines.Add(line);
                
              //  hitObjectsList.Add(firstHitGameObject.transform);
              
            }
        }
        //Sets the second position of the line
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10, layerUse))
            {
                secondHitGameObject = hit.transform.gameObject;
                secondHitObjectsList.Add(secondHitGameObject.transform); 
                firstHitGameObject.GetComponent<ObjectData>().AddConnections(secondHitGameObject); //adding second game object to the list of objects hit 
                firstHitGameObject.GetComponent<ObjectData>().AddLines(line);
                secondHitGameObject.GetComponent<ObjectData>().AddLines(line); 
                secondHitGameObject.GetComponent<ObjectData>().AddConnections(firstHitGameObject); //adding first game object to second's list of hit objects
               
                
            //    Debug.Log(secondHitGameObject.name);
            //   hitObjectsList.Add(secondHitGameObject.transform);
             //   line.positionCount++;
          
            }      
        }
        
    }

    //By pressing B on controller, you can figure out the gameobjects it's connected to, the number of them and the number of lines we've created under it 
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
                hitObject.GetComponent<ObjectData>().EnableLines(); 
                Debug.Log("list of Objects" + hitObject.GetComponent<ObjectData>().DisplayCountOfConnections());
                Debug.Log("list of lines for this object" + hitObject.GetComponent<ObjectData>().DisplayLines());
            }
        }
    }
    //Trial testing UI: functioning where you press a controller button and a UI button shows up
    void OpenMenu()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            button.gameObject.SetActive(true);
            button.onClick.AddListener(() => DisableLines()); 
        }
    }
    //Set the lines to inactive 
    void DisableLines()
    {
        foreach(var line in lines)
        {
            line.gameObject.SetActive(false); 
        }
    }    
    
    
    
}
