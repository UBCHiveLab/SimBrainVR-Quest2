using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    private List<GameObject> objectsConnected;
    List<LineRenderer> connectedLines;

    [Header("Colours")]
    public Material red;
    public Material green;
    public Material yellow;
    public Material pink;
    public Material purple;
    public Material blue;


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
        Debug.Log(this.name + "addedLine");
    }

    public void ToggleLines(bool show)
    {
        if (show)
        {
            foreach (var line in connectedLines)
            {
                Debug.Log("hiding lines");
                line.gameObject.SetActive(false);

            }
        } else
        {
            foreach (var line in connectedLines)
            {
                Debug.Log("showing lines");
                line.gameObject.SetActive(true);
            }
        }
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

    public void DisableLines()
    {
        foreach (var line in connectedLines)
        {
            line.gameObject.SetActive(false);
        }
    }

    public void Colour(int number)
    {
        if (number == 0)
        {
            foreach (var line in connectedLines)
            {
                line.material = blue;


            }
        }
        else if (number == 1)
        {
            foreach (var line in connectedLines)
            {
                line.material = green;
            }
        }
        else if (number == 2)
        {
            foreach (var line in connectedLines)
            {
                line.material = yellow;
            }
        }
        else if (number == 3)
        {
            foreach (var line in connectedLines)
            {
                line.material = red;
            }
        }
        else if (number == 4)
        {
            foreach (var line in connectedLines)
            {
                line.material = purple;
            }
        }
        else if (number == 5)
        {
            foreach (var line in connectedLines)
            {
                line.material = pink;
            }
        }
    }
}
