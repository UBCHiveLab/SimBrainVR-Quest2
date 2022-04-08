using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://developer.oculus.com/documentation/unity/latest/concepts/unity-ovrinput/#unity-ovrinput-touch
public class RaycastClicker : MonoBehaviour
{


    public LayerMask IgnoreMe;
    RaycastHit _hit;


    private void Update()
    {

        if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
        {
            Click();
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out _hit, Mathf.Infinity, ~IgnoreMe))
        {
            if (_hit.collider.name == "Hologram")
            {
                var outline = _hit.collider.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                }
            }
        }

    }

    private void Click()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.red);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~IgnoreMe))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);

            if(hit.collider.name == "Hologram")
            {
                HologramManager hologramManager = hit.collider.GetComponent<HologramManager>();
                if (hologramManager!=null)
                {
                    hologramManager.ToggleUI();
                }
            }

        }
    }

 
}
