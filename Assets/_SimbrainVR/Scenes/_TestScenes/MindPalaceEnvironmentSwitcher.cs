using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindPalaceEnvironmentSwitcher : MonoBehaviour
{

    public GameObject terrainAmber, terrainGrassy, terrainOcean, terrain4;
    int toggle = 0;


    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            if (toggle == 0) //switch to grassly
            {
                Camera camera = Camera.main;
                camera.clearFlags = CameraClearFlags.Skybox;
                SwitchToGrasslyTerrain();
            }
            else if (toggle == 1) //ocean
            {
                Camera camera = Camera.main;
                camera.clearFlags = CameraClearFlags.Skybox;
                SwitchToOcean();
            }
            else if (toggle == 2)
            {
                SwitchToWhiteSky();
                
            }else if(toggle == 3)
            {
                SwitchToCartoonWaterTerrain();
                toggle = 0;
            }

            toggle++;
        }
    }


    void SwitchToCartoonWaterTerrain()
    {
        Camera camera = Camera.main;
        camera.clearFlags = CameraClearFlags.Skybox;

        terrainAmber.SetActive(true);

        terrainGrassy.SetActive(false);
        terrain4.SetActive(false);
        terrainOcean.SetActive(false);
    }

    void SwitchToGrasslyTerrain()
    {
        terrainGrassy.SetActive(true);

        terrainAmber.SetActive(false);
        terrainOcean.SetActive(false);
        terrain4.SetActive(false);
    }

    void SwitchToOcean()
    {
        terrainOcean.SetActive(true);

        terrainGrassy.SetActive(false);
        terrainAmber.SetActive(false);
        terrain4.SetActive(false);
    }

    void SwitchToWhiteSky()
    {
        Camera camera = Camera.main;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Color.white;

        terrainAmber.SetActive(false);
        terrainOcean.SetActive(false);
        terrainGrassy.SetActive(false);

        terrain4.SetActive(true);
    }
}
