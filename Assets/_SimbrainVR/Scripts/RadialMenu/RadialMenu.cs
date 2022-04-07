using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{

    public GameObject Menu;
    public Transform dummyPos;
    public Vector2 normalisedThumbstickPosition; 
    public GameObject cursor;
    private int selection; 
    private int previousSelection;

    public GameObject[] menuItems;

    private MenuItemScript menuItemSc;
    private MenuItemScript previousMenuItemSc;
    private MenuItemScript nextMenuItemSc;

    public GameObject playerMovement;
    public GameObject rightController; 
    // Start is called before the first frame update
    void Start()
    {
        MenuItemScript firstMenu = menuItems[0].GetComponent<MenuItemScript>();
        firstMenu.description.SetActive(true); 
        selection = 0;
        previousSelection = -1;
    }

    // Update is called once per frame
    void Update()
    {
        OpenRadialMenu();
        UpdateRadialMenu();

        SimpleCapsuleWithStickMovement movt = playerMovement.GetComponent<SimpleCapsuleWithStickMovement>();
        if (Menu.activeInHierarchy)
        {
            movt.enabled = false;
        }
        else
        {
            movt.enabled = true; 
        }
       // Menu.transform.localPosition = transform.position;
      //  Menu.transform.localRotation = transform.rotation;
    }

    void OpenRadialMenu()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Menu.SetActive(!Menu.activeInHierarchy);
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
        /*
        
        if (selection != previousSelection)
        {
            previousMenuItemSc = menuItems[previousSelection].GetComponent<MenuItemScript>();
            previousMenuItemSc.Deselect();
            previousSelection = selection;

            menuItemSc = menuItems[selection].GetComponent<MenuItemScript>();
            menuItemSc.Select(); 
        }
        */
      //  Debug.Log(selection);

        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
        {
           if (selection != previousSelection)
            {
                if (previousSelection >= 0)
                {
                    previousMenuItemSc = menuItems[previousSelection].GetComponent<MenuItemScript>();
                    previousMenuItemSc.description.SetActive(false);
                    previousSelection = selection;
                }
               
                selection++;
                nextMenuItemSc = menuItems[selection].GetComponent<MenuItemScript>();
                nextMenuItemSc.MoveCursor();
                Debug.Log("cursor moved right");
            }
               
        }

        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        {
            Raycaster ray = rightController.GetComponent<Raycaster>();
            ray.DisableLines(); 
            Debug.Log("hide lines"); 
        }
    }
}
