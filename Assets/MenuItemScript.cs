using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class MenuItemScript : MonoBehaviour
{
    public Color hoverColor;
    public Color baseColor;
    public Image background;
    public GameObject description;
  //  public GameObject cursor;

    public GameObject cursorPos; 
    // Start is called before the first frame update
    void Start()
    {
        description.SetActive(false); 
    }

    public void MoveCursor(GameObject cursor)
    {
        Debug.Log(cursor);
        cursor.transform.position = cursorPos.transform.position;
        cursor.transform.rotation = cursorPos.transform.rotation; 
      
        description.SetActive(true);
        Debug.Log((description.activeInHierarchy));
        Debug.Log(description + "description");
    }

    public void Select()
    {
        background.color = hoverColor;
        description.SetActive(true); 
    }

    public void Deselect()
    {
      //  background.color = baseColor;
        description.SetActive(false); 
    }

    public string DescriptionText()
    {
        string text = description.transform.gameObject.GetComponent<TextMeshProUGUI>().text;
        return text; 
    }
}
