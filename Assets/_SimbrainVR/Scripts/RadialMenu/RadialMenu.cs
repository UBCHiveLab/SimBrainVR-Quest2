using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{

    public GameObject Menu;
    public Transform dummyPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OpenRadialMenu(); 
    }

    void OpenRadialMenu()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Menu.SetActive(true);
            Instantiate(Menu, dummyPos);       
        }
    }
}
