using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    private List<GameObject> objectsConnected;
    List<LineRenderer> connectedLines;
    // Start is called before the first frame update
    void Start()
    {
        objectsConnected = new List<GameObject>();
        connectedLines = new List<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //Adds the game objects that have been connected with lines
    public void AddConnections(GameObject newObject)
    {
        if (!objectsConnected.Contains(newObject))
        {
            objectsConnected.Add(newObject);
        }
    }

    public void DisplayConnections()
    {

        foreach (var newObject in objectsConnected)
        {
            Debug.Log("list" + newObject.name);
            Debug.Log("list" + newObject);
        }
    }

    public int DisplayCountOfConnections()
    {
        return objectsConnected.Count;
    }

    //Adds each line into the list once created
    public void AddLines(LineRenderer line)
    {
        connectedLines.Add(line);
    }

    public int DisplayLines()
    {
        return connectedLines.Count; 
    }
    public void EnableLines()
    {
        foreach (var line in connectedLines)
        {
            line.gameObject.SetActive(true); 
        }
    }
}
