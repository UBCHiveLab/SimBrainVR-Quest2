using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGrabbableSpecimen : MonoBehaviour
{
    public Transform spawnLocation;
    public Vector3 randomAreaAroundSpawnLocation = Vector3.one;
    public float minYPositionAfterSpawning = 0f;
    public bool addRigidbodyAfterSpawning = true;
    public bool addMeshColliderToParentAfterSpawning = true;
    public bool destroyMeshColliderOnChildAfterSpawning = true;
    public bool addGrabbableAsChild = true;
    public DistanceGrabbable_Expanded grabbablePrefab = default;

    public void Activate(GameObject specimenPrefab)
    {

        GameObject spawnedObject = Instantiate(specimenPrefab, spawnLocation.position, spawnLocation.rotation);

        Debug.Log("specimen spawned " + spawnedObject.name);

        //spawnedObject.transform.localScale = (float)specimenData.scale * Vector3.one;

        Vector3 randomOffset = UnityEngine.Random.insideUnitSphere;

        Vector3 newPosition = spawnLocation.position + new Vector3(randomOffset.x * randomAreaAroundSpawnLocation.x, randomOffset.y * randomAreaAroundSpawnLocation.y, randomOffset.z * randomAreaAroundSpawnLocation.z);

        newPosition.y = Mathf.Max(minYPositionAfterSpawning, newPosition.y);

        spawnedObject.transform.position = newPosition;
        //spawnedObject.transform.position = new Vector3(2.055f, 1.7352f, -0.956f);


        //adjust components structure:

        if (destroyMeshColliderOnChildAfterSpawning)
        {
            MeshCollider meshCollider = spawnedObject.GetComponentInChildren<MeshCollider>();

            Destroy(meshCollider);

        }

        Rigidbody newRigidbody = null;

        if (addRigidbodyAfterSpawning)
        {
            newRigidbody = spawnedObject.AddComponent<Rigidbody>();
            newRigidbody.isKinematic = true;
            newRigidbody.useGravity = false;
        }

        MeshCollider newMeshCollider = null;

        if (addMeshColliderToParentAfterSpawning)
        {
            newMeshCollider = spawnedObject.AddComponent<MeshCollider>();

        }

        if (addGrabbableAsChild)
        {
            DistanceGrabbable_Expanded grabbable = Instantiate(grabbablePrefab, spawnedObject.transform);

            grabbable.optionalExternalCollider = newMeshCollider;
            grabbable.optionalExternalRigidbody = newRigidbody;

            grabbable.enabled = true;
        }

    }

}
