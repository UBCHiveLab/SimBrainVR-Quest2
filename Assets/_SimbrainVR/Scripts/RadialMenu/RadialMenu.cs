using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class RadialMenu : MonoBehaviour
{
    [Header("Radial Menus")]
    public GameObject ObjectRadialMenu;
    public GameObject LineRadialMenu;
    public GameObject TextRadialMenu;
    public GameObject ImageRadialMenu; 
    public GameObject Menu;
    
    [Header("Cursor")]
    public GameObject objectCursor;
    public GameObject imageCursor;
    public GameObject lineCursor;
    public GameObject textCursor; 
   // private int selection; 
    private int previousSelection;
    [Header("selection")]
    private int objectSelection = 0;
    private int textSelection = 0;
    private int imageSelection = 0;
    private int lineSelection = 0;

    [Header("Menu Items")]
    public GameObject[] objectMenuItems;
    public GameObject[] imageMenuItems;
    public GameObject[] lineMenuItems;
    public GameObject[] textMenuItems;
    public Button[] symptomButtons;
    public Button[] colorButtons;

    private MenuItemScript menuItemSc;
    private MenuItemScript previousMenuItemSc;
    private MenuItemScript nextMenuItemSc;

    public GameObject playerMovement;
    public GameObject rightController;
    Raycaster raycaster;
    MenuItemScript fifthMenu;
    TextMeshProUGUI fifthDesc;
    MenuItemScript seventhMenu;
    string seventhDesc;
    public GameObject colourDisplay;
    public Button closeColor;
    public GameObject symptomList;
    public Button closeSymptoms;

    int layerUse = ~1;
    int rayLength = 100;
    GameObject objectHit;

    [Header("specimens")]
    public Transform specimen1;
    private Vector3 currentSpecimen1Pos; 
    public Transform specimen2;
    private Vector3 currentSpecimen2Pos; 
    public Transform specimen3;
    private Vector3 currentSpecimen3Pos; 

    // Start is called before the first frame update
    void Start()
    {
        MenuItemScript firstObjectMenu = objectMenuItems[0].GetComponent<MenuItemScript>();
        firstObjectMenu.description.SetActive(true);
        MenuItemScript firstImageMenu = imageMenuItems[0].GetComponent<MenuItemScript>();
        firstImageMenu.description.SetActive(true);
        fifthMenu = imageMenuItems[4].GetComponent<MenuItemScript>();
        fifthDesc = fifthMenu.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        Debug.Log(fifthDesc.text); 
     
      
        previousSelection = -1;
        currentSpecimen1Pos = specimen1.position;
        currentSpecimen2Pos = specimen2.position;
        currentSpecimen3Pos = specimen3.position; 
        Debug.Log(currentSpecimen1Pos);
        raycaster = rightController.GetComponent<Raycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        OpenRadialMenu();

      //  UpdateSelection(); 
      //  UpdateRadialMenu();
        SimpleCapsuleWithStickMovement movt = playerMovement.GetComponent<SimpleCapsuleWithStickMovement>();
        if ((ObjectRadialMenu.activeInHierarchy) || (LineRadialMenu.activeInHierarchy) || (ImageRadialMenu.activeInHierarchy) || (TextRadialMenu.activeInHierarchy))
        {
            movt.enabled = false;
        //    Menu.transform.position = objectHit.transform.position + (transform.right * .3f);
        }
        else
        {
            movt.enabled = true;
        }
        if (ObjectRadialMenu.activeInHierarchy)
        {
            UpdateSpecimenSelection();
            //  UpdateSelection(objectSelection);
            UpdateSpecimenRadialMenu();
            SpecimenRadialFunctions();
            lineSelection = 0;
            imageSelection = 0;
            textSelection = 0; 
        }

        if (ImageRadialMenu.activeInHierarchy)
        {
            UpdateImageSelection();
            UpdateImageRadialMenu();
            ImageRadialFunctions();
            lineSelection = 0;
            textSelection = 0;
            objectSelection = 0; 
        }
        if (TextRadialMenu.activeInHierarchy)
        {
            UpdateTextSelection();
            UpdateTextRadialMenu();
            lineSelection = 0;
            imageSelection = 0;
            objectSelection = 0; 
        }
        if (LineRadialMenu.activeInHierarchy)
        {
            UpdateLineSelection();
            UpdateLineRadialMenu();
            textSelection = 0;
            objectSelection = 0;
            imageSelection = 0; 
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
  
      //  ShowToggle();
       
    }
    
    void OpenRadialMenu()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (DetectObject() != null)
            {
                objectHit = DetectObject();
                //  Menu.transform.localPosition = objectHit.transform.position + (transform.right * .3f);
                //  selection = 0;
                /*
                  for (int i = 1; i < menuItems.Length; i++)
                  {
                      menuItemSc = menuItems[i].GetComponent<MenuItemScript>();
                      menuItemSc.description.SetActive(false);
                  }
                */
                if (objectHit.name.Contains("LOADEDSPECIMEN"))
                {
                    Debug.Log("show radial menu for 3d objects");
                    OpenSpecimenRadialMenu();

                }
                if (objectHit.name.Contains("IMAGE"))
                {
                    Debug.Log("show radial menu for images");
                    OpenImageRadialMenu();
                }
                if (objectHit.name.Contains("TEXT"))
                {
                    Debug.Log("show radial menu for text");
                    OpenTextRadialMenu();
                }
                if (objectHit.name.Contains("Line"))
                {
                    Debug.Log("show radial menu for line");
                    OpenLineRadialMenu();
                }
            }
            SwitchOffDisplays();
        }
    }
    
    private void OpenSpecimenRadialMenu()
    {
        ObjectRadialMenu.SetActive(!ObjectRadialMenu.activeInHierarchy);
        ObjectRadialMenu.transform.position = objectHit.transform.position + (transform.right * .3f);
        objectSelection = 0; 
       for (int i = 1; i < objectMenuItems.Length; i++)
        {
            menuItemSc = objectMenuItems[i].GetComponent<MenuItemScript>();
            menuItemSc.description.SetActive(false);
        }
      
    }
    private void OpenImageRadialMenu()
    {
        ImageRadialMenu.SetActive(!ImageRadialMenu.activeInHierarchy);
        ImageRadialMenu.transform.position = objectHit.transform.position + (transform.right * .3f);
        imageSelection = 0;
    }
    private void OpenTextRadialMenu()
    {
        TextRadialMenu.SetActive(!TextRadialMenu.activeInHierarchy);
        TextRadialMenu.transform.position = objectHit.transform.position + (transform.right * .3f);
        textSelection = 0; 
    }

    private void OpenLineRadialMenu()
    {
        LineRadialMenu.SetActive(!LineRadialMenu.activeInHierarchy);
        LineRadialMenu.transform.position = objectHit.transform.position + (transform.right * .3f);
        lineSelection = 0; 
      }
    private void UpdateSpecimenRadialMenu()
    {

        if (objectSelection == 0)
        {
            Debug.Log("selection is at 0");
            previousMenuItemSc = objectMenuItems[5].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = objectMenuItems[0].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(objectCursor);

            nextMenuItemSc = objectMenuItems[1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
        }
        else if (objectSelection == 5)
        {
            Debug.Log("selection is at 5");
            previousMenuItemSc = objectMenuItems[4].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = objectMenuItems[5].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(objectCursor);

            nextMenuItemSc = objectMenuItems[0].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
            SwitchOffDisplays();
        }
        else
        {
            Debug.Log("selection is at " + objectSelection);
            previousMenuItemSc = objectMenuItems[objectSelection - 1].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = objectMenuItems[objectSelection].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(objectCursor);

            nextMenuItemSc = objectMenuItems[objectSelection + 1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
            symptomList.SetActive(false);
        }
    }

    private void UpdateImageRadialMenu()
    {

        if (imageSelection == 0)
        {
            Debug.Log("image selection is at 0");
            previousMenuItemSc = imageMenuItems[5].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = imageMenuItems[0].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(imageCursor);

            nextMenuItemSc = imageMenuItems[1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
        }
        else if (imageSelection == 5)
        {
            Debug.Log("image selection is at 5");
            previousMenuItemSc = imageMenuItems[4].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = imageMenuItems[5].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(imageCursor);

            nextMenuItemSc = imageMenuItems[0].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
            SwitchOffDisplays();
        }
        else
        {
            Debug.Log("image selection is at " + imageSelection);
            previousMenuItemSc = imageMenuItems[imageSelection - 1].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = imageMenuItems[imageSelection].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(imageCursor);

            nextMenuItemSc = imageMenuItems[imageSelection + 1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
            symptomList.SetActive(false);
        }
    }
    private void UpdateLineRadialMenu()
    {

        if (lineSelection == 0)
        {
            Debug.Log("line selection is at 0");
            previousMenuItemSc = lineMenuItems[5].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = lineMenuItems[0].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(lineCursor);

            nextMenuItemSc = lineMenuItems[1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
        }
        else if (lineSelection == 5)
        {
            Debug.Log("line selection is at 5");
            previousMenuItemSc = lineMenuItems[4].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = lineMenuItems[5].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(lineCursor);

            nextMenuItemSc = lineMenuItems[0].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
            SwitchOffDisplays();
        }
        else
        {
            Debug.Log("line selection is at " + lineSelection);
            previousMenuItemSc = lineMenuItems[lineSelection - 1].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = lineMenuItems[textSelection].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(lineCursor);

            nextMenuItemSc = lineMenuItems[lineSelection + 1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
            symptomList.SetActive(false);
        }
    }

    private void UpdateTextRadialMenu()
    {

        if (textSelection == 0)
        {
            Debug.Log("text selection is at 0");
            previousMenuItemSc = textMenuItems[5].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = textMenuItems[0].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(textCursor);

            nextMenuItemSc = textMenuItems[1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
        }
        else if (textSelection == 5)
        {
            Debug.Log("text selection is at 5");
            previousMenuItemSc = textMenuItems[4].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = textMenuItems[5].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(textCursor);

            nextMenuItemSc = textMenuItems[0].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
            SwitchOffDisplays();
        }
        else
        {
            Debug.Log("selection is at " + objectSelection);
            previousMenuItemSc = textMenuItems[textSelection - 1].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = textMenuItems[textSelection].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor(textCursor);

            nextMenuItemSc = textMenuItems[textSelection + 1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
            symptomList.SetActive(false);
        }
    }
    /*
    private void UpdateRadialMenu(int selection)
    {
        
        if (selection == 0)
        {
            previousMenuItemSc = menuItems[5].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = menuItems[0].GetComponent<MenuItemScript>();
            menuItemSc.MoveCursor();

            nextMenuItemSc = menuItems[1].GetComponent<MenuItemScript>();
            nextMenuItemSc.description.SetActive(false);
        } else if (selection == 5)
        {
            previousMenuItemSc = menuItems[4].GetComponent<MenuItemScript>();
            previousMenuItemSc.description.SetActive(false);

            menuItemSc = menuItems[5].GetComponent<MenuItemScript>();
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
    */
    void UpdateSpecimenSelection()
    {
        Debug.Log("objectselection " + objectSelection);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight))
        {
            if (objectSelection == 5)
            {
                objectSelection = 0;
            }
            else
            {
                objectSelection = objectSelection + 1;
            }

        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft))
        {
            if (objectSelection == 0)
            {
                objectSelection = 5;
            }
            else
            {
                objectSelection = objectSelection - 1;
            }

        }
    }

    void UpdateImageSelection()
    {
        Debug.Log("imageselection " + imageSelection);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight))
        {
            if (imageSelection == 5)
            {
                imageSelection = 0;
            }
            else
            {
                imageSelection = imageSelection + 1;
            }

        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft))
        {
            if (imageSelection == 0)
            {
                imageSelection = 5;
            }
            else
            {
                imageSelection = imageSelection - 1;
            }

        }
    }

    void UpdateLineSelection()
    {
        Debug.Log("lineselection " + lineSelection);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight))
        {
            if (lineSelection == 5)
            {
                lineSelection = 0;
            }
            else
            {
                lineSelection = lineSelection + 1;
            }

        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft))
        {
            if (lineSelection == 0)
            {
                lineSelection = 5;
            }
            else
            {
                lineSelection = lineSelection - 1;
            }

        }
    }
    void UpdateTextSelection()
    {
        Debug.Log("textselection " + textSelection);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight))
        {
            if (textSelection == 5)
            {
                textSelection = 0;
            }
            else
            {
                textSelection = textSelection + 1;
            }

        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft))
        {
            if (textSelection == 0)
            {
                textSelection = 5;
            }
            else
            {
                textSelection = textSelection - 1;
            }

        }
    }


    void UpdateSelection(int selection)
    {
        Debug.Log("selection" + selection);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight))
        {
            if (selection == 5)
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
                selection = 5; 
            } else
            {
                selection = selection - 1;
            }
           
        }
    }
    void SpecimenRadialFunctions()
    {

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {
            if (objectSelection == 0)
            {
                symptomList.SetActive(true);
                Debug.Log("symptom list is there");
                MarkSymptoms();

            }
            if (objectSelection == 4)
            {
                if (objectHit.name.Contains("1")) objectHit.transform.position = currentSpecimen1Pos; 
                if (objectHit.name.Contains("2")) objectHit.transform.position = currentSpecimen2Pos;
                if (objectHit.name.Contains("3")) objectHit.transform.position = currentSpecimen3Pos;
                Debug.Log("set specimen back to cart");
                Debug.Log(currentSpecimen1Pos);
                symptomList.SetActive(false);
                ObjectRadialMenu.SetActive(false);

            }
        }
    }

    void ImageRadialFunctions()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {
            if (imageSelection == 0)
            {
                symptomList.SetActive(true);

            }
            if (imageSelection == 1)
            {
                objectHit.SetActive(false);
                Debug.Log("delete object");
                ImageRadialMenu.SetActive(false); 
            }
           
            if (imageSelection == 4)
            {
                if (fifthDesc.text == "Hide")
                {
                    objectHit.SetActive(false);
                    fifthDesc.text = "Show"; 
                } else
                {
                    objectHit.SetActive(true);
                    fifthDesc.text = "Hide"; 
                }

            }
        }
    }
    /*
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
    */

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
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, layerUse))
        {
            hitObject = hit.transform.gameObject; 
        }
        return hitObject; 
    }

    
}
