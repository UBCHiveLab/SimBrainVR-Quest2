using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    private List<GameObject> objectsConnected;
    List<string> names;
    // Start is called before the first frame update
    void Start()
    {
        objectsConnected = new List<GameObject>();
        names = new List<string>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddConnections(GameObject newObject)
    {
        if (!objectsConnected.Contains(newObject))
        {
            objectsConnected.Add(newObject);
            Debug.Log("newObject" + newObject.name);
        }
    }

    public List<string> DisplayConnections()
    {
        
       foreach (var newObject in objectsConnected)
        {
            Debug.Log(newObject.name);
            names.Add(newObject.name);
        }
        return names;
    }
}
