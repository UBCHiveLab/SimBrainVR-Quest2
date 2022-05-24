using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinScript : MonoBehaviour
{

    public GameObject pinPrefab;
    public LayerMask pinLayer;
    public float spherecastRadius = 1f, spherecastDistance = 2f;

    bool isInstantiatingPin;


    private void Update()
    {

        if (OVRInput.Get(OVRInput.RawButton.LHandTrigger))
        {
            
            DeletePinSphereCast();
        }

        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            StartCoroutine(SpawnPin());
        }

    }

    void DeletePinSphereCast()
    {
        RaycastHit hit;

        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.red);

        if (Physics.SphereCast(transform.position, spherecastRadius, transform.TransformDirection(Vector3.forward), out hit, spherecastDistance, pinLayer))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            print("Destroying" + hit.collider.name);
            Destroy(hit.collider.gameObject);

        }
    }

    IEnumerator SpawnPin()
    {

        if (!isInstantiatingPin)
        {
            isInstantiatingPin = true;

            print("spawning pin");

            Instantiate(pinPrefab, transform.position, pinPrefab.transform.rotation);
            yield return new WaitForSeconds(1f);

            isInstantiatingPin = false;
        }
    } 
}
