using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class RadialMenu : MonoBehaviour
{

    public GameObject Menu;
    
    public GameObject cursor;
    private int selection; 
    private int previousSelection;

    public GameObject[] menuItems;
    public Button[] symptomButtons;
    public Button[] colorButtons;

    private MenuItemScript menuItemSc;
    private MenuItemScript previousMenuItemSc;
    private MenuItemScript nextMenuItemSc;

    public GameObject playerMovement;
    public GameObject rightController;
    Raycaster raycaster;
    MenuItemScript sixthMenu;
    TextMeshProUGUI sixthDesc;
    MenuItemScript seventhMenu;
    string seventhDesc;
    public GameObject colourDisplay;
    public Button closeColor;
    public GameObject symptomList;
    public Button closeSymptoms;

    int layerUse = ~1;
    int rayLength = 100;
    GameObject objectHit;

    // Start is called before the first frame update
    void Start()
    {
        MenuItemScript firstMenu = menuItems[0].GetComponent<MenuItemScript>();
        firstMenu.description.SetActive(true);
        sixthMenu = menuItems[5].GetComponent<MenuItemScript>();
        sixthDesc = sixthMenu.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        Debug.Log(sixthDesc.text); 
        seventhMenu = menuItems[6].GetComponent<MenuItemScript>();
        seventhDesc = seventhMenu.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        Debug.Log(seventhDesc); 
        selection = 0;
        previousSelection = -1;
      
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
        //    Menu.transform.position = objectHit.transform.position + (transform.right * .3f);
        }
        else
        {
            movt.enabled = true;
        }
        /*
        if (DetectObject() != null)
        {
            if (Menu.activeInHierarchy)
            {
                
                Menu.transform.localPosition = DetectObject().transform.position + (transform.right * .3f);
            }
        }
        */
        RadialFunctions();
        ShowToggle();
       if (selection != 3)
        {
            colourDisplay.SetActive(false);
        }
    }

    void OpenRadialMenu()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (DetectObject() != null)
            {
                Menu.SetActive(!Menu.activeInHierarchy);
                
                objectHit = DetectObject();
                Menu.transform.localPosition = objectHit.transform.position + (transform.right * .3f);
                selection = 0;
                for (int i = 1; i < menuItems.Length; i++)
                {
                    menuItemSc = menuItems[i].GetComponent<MenuItemScript>();
                    menuItemSc.description.SetActive(false);
                }
            }
            
          //  Menu.transform.localPosition = transform.position;
            
            SwitchOffDisplays();
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
            SwitchOffDisplays();
        } else
        {
            previousMenuItemSc = menuItems[selection - 1].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = menuItems[selection].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor();

            nextMenuItemSc = menuItems[selection + 1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
            symptomList.SetActive(false);
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
                objectHit.SetActive(false);
                Menu.SetActive(false);
             //   raycaster.DeleteObject(); 
                Debug.Log("delete object");
                
            }
          
            if (selection == 6)
            {
                objectHit.transform.localScale += new Vector3(.01f, .01f, .01f);
              //  DetectObject().transform.localScale += new Vector3((float).1, (float).1, (float).1);
              //  raycaster.AdjustObjectSize();
                Debug.Log("adjust size");
           
            }
        }
    }

    void ShowToggle()
    {

        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {
            if (selection == 5)  
            {
                ObjectData objectData = objectHit.GetComponent<ObjectData>();
             //   objectData.ToggleLines(sixthDesc.text == "Hidden"); 
                
                
             //   SwitchOffDisplays(); 
                if (sixthDesc.text == "Hidden")
                {
                    sixthDesc.text = "Show";
                    objectHit.GetComponent<ObjectData>().DisableLines();
                    objectHit.GetComponent<ObjectData>().DisplayConnections();
                    Debug.Log(objectHit.GetComponent<ObjectData>().DisplayLines());
                    //  raycaster.DisableLines(true);
                  
                }
                else
                {
                    objectHit.GetComponent<ObjectData>().EnableLines();
                  //  raycaster.DisableLines(false);
                    sixthDesc.text = "Hidden";
               
                }
                
            }

            if (selection == 3)
            {
              //  symptomList.SetActive(false);
                colourDisplay.SetActive(true);
                ChangeColour();
            }

            if (selection == 0)
            {
                symptomList.SetActive(true);
                MarkSymptoms();
            }
        }
    }

    void MarkSymptoms()
    {
        symptomButtons[0].onClick.AddListener(() => Highlight(0));
        symptomButtons[1].onClick.AddListener(() => Highlight(1));
        symptomButtons[2].onClick.AddListener(() => Highlight(2));
        symptomButtons[3].onClick.AddListener(() => Highlight(3));
        symptomButtons[4].onClick.AddListener(() => Highlight(4));
        closeSymptoms.onClick.AddListener(() => symptomList.SetActive(false));
    }

   void Highlight(int number)
    {
        symptomButtons[number].image.color = Color.blue; 
    }

    void SwitchOffDisplays()
    {
        symptomList.SetActive(false);
        colourDisplay.SetActive(false);
    }

    void ChangeColour()
    {
        ObjectData data = objectHit.GetComponent<ObjectData>();

        colorButtons[0].onClick.AddListener(() => data.Colour(0));
        colorButtons[1].onClick.AddListener(() => data.Colour(1));
        colorButtons[2].onClick.AddListener(() => data.Colour(2));
        colorButtons[3].onClick.AddListener(() => data.Colour(3));
        colorButtons[4].onClick.AddListener(() => data.Colour(4));
        colorButtons[5].onClick.AddListener(() => data.Colour(5));
        closeColor.onClick.AddListener(() => colourDisplay.SetActive(false));
    }

    GameObject DetectObject()
    {
        RaycastHit hit;
        GameObject hitObject = null; 
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10, layerUse))
        {
            hitObject = hit.transform.gameObject; 
        }
        return hitObject; 
    }

    
}
