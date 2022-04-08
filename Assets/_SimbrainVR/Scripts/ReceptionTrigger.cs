using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionTrigger : MonoBehaviour
{

    public SceneLoading _sceneLoader;
    public GameObject sceneLoader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "casefile")
        {
            Camera camera = Camera.main;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;

            sceneLoader.SetActive(true);
            _sceneLoader.TransitionToClinic();
        }
    }
}
