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
    public GameObject cursor;

    public GameObject cursorPos; 
    // Start is called before the first frame update
    void Start()
    {
        description.SetActive(false); 
     //   background.color = baseColor; 
    }

    public void MoveCursor()
    {
        cursor.transform.position = cursorPos.transform.position;
        cursor.transform.rotation = cursorPos.transform.rotation; 
        string desc = description.name;
        Debug.Log(desc);
        description.SetActive(true); 
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
}
