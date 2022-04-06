using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{

    public GameObject Menu;
    public Transform dummyPos;
    public GameObject cursor;
    private List<Transform> allCursorPos;
    public Transform startCursor;
    public Transform secondCursor;
    public Transform thirdCursor;
    public Transform fourthCursor;
    public Transform fifthCursor;
    public Transform sixthCursor;
    public Transform seventhCursor; 
    // Start is called before the first frame update
    void Start()
    {
        allCursorPos = new List<Transform>(); 
     //   cursorPos = startCursor; 
        allCursorPos.Add(startCursor);
        allCursorPos.Add(secondCursor);
     //   allCursorPos.Add(thirdCursor);
      //  allCursorPos.Add(fourthCursor);
       // allCursorPos.Add(fifthCursor);
        //allCursorPos.Add(sixthCursor);
       // allCursorPos.Add(seventhCursor); 
    }

    // Update is called once per frame
    void Update()
    {
        OpenRadialMenu();
        UpdateRadialMenu(); 
       // Menu.transform.localPosition = transform.position;
      //  Menu.transform.localRotation = transform.rotation;
    }

    void OpenRadialMenu()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Menu.SetActive(true);
            Vector3 controllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
          //  Menu.transform.localPosition = controllerPos;
          //  Menu.transform.localRotation = controllerRotation;
            Menu.transform.localPosition = transform.position;
         //   Menu.transform.localRotation = transform.rotation;
        }
    }

    void UpdateRadialMenu()
    {
        Debug.Log(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick)); 
        /*
        if (OVRInput.Get(OVRInput.Button.Four))
        {
            Transform cursorPos = cursor.transform;
            cursorPos = secondCursor; 
            Debug.Log("second cursor"); 
        }
        */
        
    }
}
