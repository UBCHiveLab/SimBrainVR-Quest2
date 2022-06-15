using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionTrigger : MonoBehaviour
{

    //attached to the receptionist in reception scene

    public SceneLoading _sceneLoader;
    public GameObject sceneLoader;

    public Transform player;
    public float triggerDistance = 0.5f;
    
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

    public void GoToClinicFromReception()
    {
        Camera camera = Camera.main;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Color.black;

        sceneLoader.SetActive(true);
        _sceneLoader.TransitionToClinic();
    }

    private void Update()
    {
        //print(Vector3.Distance(transform.position, player.position));
        if (Vector3.Distance(transform.position, player.position) < triggerDistance)
        {
            GoToClinicFromReception();
        }
    }
}
