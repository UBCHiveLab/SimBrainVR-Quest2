using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnImageGrabbable : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public GameObject object4;
    public GameObject object5;
    public Transform button1Pos; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        button1.onClick.AddListener(() => AddImageOne());
        button2.onClick.AddListener(() => AddImageTwo());
        button3.onClick.AddListener(() => AddImageThree());
        button4.onClick.AddListener(() => AddImageFour());
        button5.onClick.AddListener(() => AddImageFive());
    }

    public void AddImageOne()
    {
        object1.SetActive(true); 
        Debug.Log("adding 1st image"); 
    }

    public void AddImageTwo()
    {
        object2.SetActive(true); 
    }
    public void AddImageThree()
    {
        object3.SetActive(true);
    }
    public void AddImageFour()
    {
        object4.SetActive(true);
    }
    public void AddImageFive()
    {
        object5.SetActive(true);
    }
}
