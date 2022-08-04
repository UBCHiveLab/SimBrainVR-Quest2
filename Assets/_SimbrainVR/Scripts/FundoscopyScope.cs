using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FundoscopyScope : MonoBehaviour
{

    [SerializeField]
    GameObject Display;
    [SerializeField]
    Camera retinaCamera;
    [SerializeField]
    FundoscopyTrigger retinaTrigger;

    [SerializeField]
    Transform retinaImage, leftRetina, rightRetina;

    public LayerMask everything, onlyFundoscopy;
    public GameObject humanoidPatient, genericPatient;

    bool inputB;

    public Vector3 humanoidEyePos = new Vector3(-4.5397f, 1.5806f, 0.8153f);
    public Vector3 genericPatientEyePos = new Vector3(0, 0, 0);

    private void Awake()
    {
        StartCoroutine(CheckHumanoidPos());
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            inputB = !inputB;
            Display.SetActive(inputB);
        }

        if (inputB) //if it's turned on
        {
            //retina images should only be visible to the camera layer.
            if (retinaTrigger.isLeft)
            {
                leftRetina.gameObject.SetActive(false);
                rightRetina.gameObject.SetActive(true);
            }
            else if (retinaTrigger.isRight)
            {
                leftRetina.gameObject.SetActive(true);
                rightRetina.gameObject.SetActive(false);
            }
            else
            {
                leftRetina.gameObject.SetActive(false);
                rightRetina.gameObject.SetActive(false);
            }


            if(retinaTrigger.isLeft || retinaTrigger.isRight)
            {
                retinaCamera.cullingMask = onlyFundoscopy;
            }
            else
            {
                retinaCamera.cullingMask = everything;
            }
        }
        else
        {
            leftRetina.gameObject.SetActive(false);
            rightRetina.gameObject.SetActive(false);
            retinaCamera.cullingMask = everything;
        }
        
    }

    IEnumerator CheckHumanoidPos()
    {

        while (true)
        {
            if (humanoidPatient.activeSelf)
            {
                retinaImage.position = humanoidEyePos;
            }else if (genericPatient.activeSelf)
            {
                retinaImage.position = genericPatientEyePos;
            }

            yield return new WaitForSeconds(1.33f);
        }
        
    }
}
