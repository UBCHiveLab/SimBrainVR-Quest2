using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{

    public GameObject Menu;
    
    public GameObject cursor;
    private int selection; 
    private int previousSelection;

    public GameObject[] menuItems;

    private MenuItemScript menuItemSc;
    private MenuItemScript previousMenuItemSc;
    private MenuItemScript nextMenuItemSc;

    public GameObject playerMovement;
    public GameObject rightController;
    Raycaster raycaster;
    MenuItemScript sixthMenu;
    string sixthDesc;
    MenuItemScript seventhMenu;
    string seventhDesc;

    // Start is called before the first frame update
    void Start()
    {
        MenuItemScript firstMenu = menuItems[0].GetComponent<MenuItemScript>();
        firstMenu.description.SetActive(true);
        sixthMenu = menuItems[5].GetComponent<MenuItemScript>();
        sixthDesc = sixthMenu.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        Debug.Log(sixthDesc); 
        seventhMenu = menuItems[6].GetComponent<MenuItemScript>();
        seventhDesc = seventhMenu.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        Debug.Log(seventhDesc); 
        selection = 0;
        previousSelection = -1;
        Debug.Log("cursor at 1");
        raycaster = rightController.GetComponent<Raycaster>();

    }

    // Update is called once per frame
    void Update()
    {
        OpenRadialMenu();

        UpdateSelection(); 
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
        RadialFunctions();
        ShowToggle(); 
       // Menu.transform.localPosition = transform.position;
      //  Menu.transform.localRotation = transform.rotation;
    }

    void OpenRadialMenu()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Menu.SetActive(!Menu.activeInHierarchy);
            Menu.transform.localPosition = transform.position;
            selection = 0; 
            for (int i = 1; i < menuItems.Length; i++)
            {
                menuItemSc = menuItems[i].GetComponent<MenuItemScript>();
                menuItemSc.description.SetActive(false); 
            }
        }
    }
    
    

    private void UpdateRadialMenu()
    {
        if (selection == 0)
        {
            previousMenuItemSc = menuItems[6].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = menuItems[0].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor();

            nextMenuItemSc = menuItems[1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
        } else if (selection == 6)
        {
            previousMenuItemSc = menuItems[5].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = menuItems[6].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor();

            nextMenuItemSc = menuItems[0].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
        } else
        {
            previousMenuItemSc = menuItems[selection - 1].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = menuItems[selection].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor();

            nextMenuItemSc = menuItems[selection + 1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
        }
        
    }

    void UpdateSelection()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight))
        {
            if (selection == 6)
            {
                selection = 0; 
            } else
            {
                selection = selection + 1;
            }
             
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft))
        {
            if (selection == 0)
            {
                selection = 6; 
            } else
            {
                selection = selection - 1;
            }
           
        }
    }
    void RadialFunctions()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        {
            if (selection == 4)
            {
                raycaster.DeleteObject(); 
                Debug.Log("delete object"); 
            }
          
            if (selection == 6)
            { 
                raycaster.AdjustObjectSize();
                Debug.Log("adjust size"); 
            }
        }
    }

    void ShowToggle()
    {

        if (selection == 5)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
            {
                if (sixthDesc == "Hidden")
                {
                    sixthDesc = "Show";
                    raycaster.DisableLines(true);
                    
                }
                else
                {
                    raycaster.DisableLines(false);
                    //  sixthDesc = "Hidden"; 
                }

                Debug.Log("hide lines");
            }
        }
    }



}
