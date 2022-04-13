using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustObjectSize : MonoBehaviour
{
    public GameObject leftController;
    public GameObject rightController; 
    private Vector3 initialObjectPosition;
    private Quaternion initialObjectRotation;
    private Vector3 initialObjectScale;

    private Vector3 initialHandPositionR;
    private Vector3 initialHandPositionL;
    Vector3 currentHandPositionL;
    Vector3 currentHandPositionR;

    int rayLength = 100;
    int layerUse = ~1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; 
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength *10, layerUse))
        {
            initialObjectPosition = hit.transform.position;
            initialObjectRotation = hit.transform.rotation;
            initialObjectScale = hit.transform.localScale;
            if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) && OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
            {
                initialHandPositionR = rightController.transform.position;
                initialHandPositionL = leftController.transform.position;
            }
            if (OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger) && OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger))
            {
                currentHandPositionL = rightController.transform.position; // current first hand position
                currentHandPositionR = leftController.transform.position; // current second hand position
                float currentGrabDistance = Vector3.Distance(currentHandPositionL, currentHandPositionR);
                float initialGrabDistance = Vector3.Distance(initialHandPositionL, initialHandPositionR);
                float p = (currentGrabDistance / initialGrabDistance); // percentage based on the distance of the initial positions and the new positions

                Vector3 newScale = new Vector3(p * initialObjectScale.x, p * initialObjectScale.y, p * initialObjectScale.z); // calculate new object scale with p

                hit.transform.localScale = newScale; // set new scale
            }
        }
       
        

        

       
    }
}
