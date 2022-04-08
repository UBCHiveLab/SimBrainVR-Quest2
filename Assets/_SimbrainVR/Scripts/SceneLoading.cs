using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    public Image _progressBar;
    public bool startAuto = false;
    public string sceneNameToLoad;
    public float delay = 1.25f;

    public GameObject sceneloadingGO;

    AsyncOperation mindPalaceLevel;
    bool transitioningToMindPalace = false;


    private void Start()
    {
        if (startAuto)
        {
            StartCoroutine(LoadAsyncOperation(sceneNameToLoad));
        }
    }

    public void LoadLevelAsync(string sceneToLoad)
    {
        StartCoroutine(LoadAsyncOperation(sceneToLoad));
    }

    
    IEnumerator LoadAsyncOperation(string sceneToLoad)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Scene load async started");

        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(sceneToLoad);
        gameLevel.allowSceneActivation = false;

        while (gameLevel.progress < 0.9f)
        {
            _progressBar.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
        _progressBar.fillAmount = 1.0f;

        yield return new WaitForSeconds(0.5f);

        gameLevel.allowSceneActivation = true;
    }

    public void TransitionToMindPalace()
    {
        if(!transitioningToMindPalace)
        {
            transitioningToMindPalace = true;
            var player = GameObject.Find("PlayerControllerDistanceGrab");
            player.GetComponent<Rigidbody>().useGravity = false;

            var clinic = GameObject.Find("UBC_Clinic");
            clinic.SetActive(false);

            sceneloadingGO.SetActive(true);

            Camera camera = Camera.main;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;


            //StartCoroutine(LoadMindPalaceAsync(sceneNameToLoad)); //MindPalace
        }
      
    }

    public void TransitionToClinic()
    {
        var player = GameObject.Find("PlayerControllerDistanceGrab");
        player.GetComponent<Rigidbody>().useGravity = false;

        Camera camera = Camera.main;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Color.black;

        var reception = GameObject.Find("Reception");
        reception.SetActive(false);

        StartCoroutine(LoadAsyncOperation(sceneNameToLoad)); //"clinic"
    }

    

    IEnumerator LoadMindPalaceAsync(string sceneToLoad)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Scene load async started");

        mindPalaceLevel = SceneManager.LoadSceneAsync(sceneToLoad);
        mindPalaceLevel.allowSceneActivation = false;

        while (mindPalaceLevel.progress < 0.9f)
        {
            _progressBar.fillAmount = mindPalaceLevel.progress;
            yield return new WaitForEndOfFrame();
        }
        _progressBar.fillAmount = 1.0f;

        //mindPalaceLevel.allowSceneActivation = true;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            if (transitioningToMindPalace)
            {
                //if (mindPalaceLevel != null) mindPalaceLevel.allowSceneActivation = true;
                SceneManager.LoadScene(3);
            }
        }
    }

}
