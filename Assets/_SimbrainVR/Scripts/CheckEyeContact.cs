using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEyeContact : MonoBehaviour
{
    public LayerMask IgnoreMe;
    public string lookingAtName = "";
    Outline _outline;

    void IsLookingAtHuman()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~IgnoreMe))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            if(hit.collider.tag == "Human")
            {
                _outline = hit.collider.gameObject.GetComponent<Outline>();
                if (_outline != null)
                {
                    if(lookingAtName != hit.collider.gameObject.name)
                    {
                        lookingAtName = hit.collider.gameObject.name;
                        _outline.enabled = true;
                    }
                }

            }
            else
            {
                if (_outline != null)
                {
                    _outline.enabled = false;
                }
            }
        }

    }


    void Update()
    {
        IsLookingAtHuman();
    }
}
