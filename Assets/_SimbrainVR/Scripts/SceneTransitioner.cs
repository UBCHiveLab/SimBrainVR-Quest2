using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "casefile")
        {
            SceneManager.LoadScene("Clinic");
        }
    }
}
