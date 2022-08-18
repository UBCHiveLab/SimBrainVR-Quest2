using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;
using UnityEngine.Networking;
using TMPro;

/*
struct AnnotationData2
{
    public string id, title, content;
    public bool global;
    public Vector3 position;
}*/

public class LoadModelsFromDatabase_OLD : MonoBehaviour
{

    //public string assetUrl = "https://hivemodelstorage.blob.core.windows.net/win/handbones";
    //public string prefabPath = "Assets/Models/Specimens/AssetBundles/HandBones.prefab";
    //public string annotationID = "hand_bones"; //Infratemporalfossa2, l4lungswithtrachea, "Brain Optic", "Brain before Sections", hand_and_forearm

    public SpecimensListSO specimensListSO = default;
    public Transform spawnLocation;
    public Vector3 randomAreaAroundSpawnLocation = Vector3.one;
    public float minYPositionAfterSpawning = 0f;

    public bool addRigidbodyAfterSpawning = true;
    public bool addBoxColliderToParentAfterSpawning = true;
    public bool destroyMeshColliderOnChildAfterSpawning = true;
    public bool addGrabbableAsChild = true;
    public DistanceGrabbable_Expanded grabbablePrefab = default;

    //public Vector3 scaleOnSpawn = new Vector3(0.0025f, 0.0025f, 0.0025f);
    //public GameObject annotationLabelPrefab;

    //public UnityEvent<GameObject> OnModelReturned = default;

    //MongoDB
    MongoClient client = new MongoClient("mongodb://hive:8afDe1K6XwY1W5cy@van-vr-shard-00-00.zr7vf.mongodb.net:27017,van-vr-shard-00-01.zr7vf.mongodb.net:27017,van-vr-shard-00-02.zr7vf.mongodb.net:27017/test?authSource=admin&replicaSet=atlas-afl4g3-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true");
    IMongoDatabase database;
    bool manifestLoaded;

    IMongoCollection<BsonDocument> specimenCollection;

    GameObject remoteModelPrefab;

    public IEnumerator SpawnModelCoroutine(SpecimenData_New specimenData)
    {
        yield return new WaitForSeconds(0.1f); // Allows animations to run without hiccuping

        string assetUrl = specimenData.assetUrl;
        string prefabPath = specimenData.prefabPath;

        using (UnityWebRequest req =
        UnityWebRequestAssetBundle.GetAssetBundle(assetUrl, 0U))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("network or http error when trying to load " + specimenData.name + " " + req.result);
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(.5f);

                try
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(req);
                    if (prefabPath != null)
                    {
                        Debug.Log("trying to load " + specimenData.name);
                        remoteModelPrefab = bundle.LoadAsset<GameObject>(prefabPath);

                        /*
                        Renderer rend = remoteModelPrefab.transform.GetChild(0).GetComponent<Renderer>();
                        if (rend != null)
                        {
                            rend.sharedMaterial.shader = Shader.Find("Universal Render Pipeline/Lit");
                        }
                        */

                        GameObject spawnedObject = Instantiate(remoteModelPrefab, spawnLocation.position, spawnLocation.rotation);

                        Debug.Log("specimen spawned " + spawnedObject.name);

                        spawnedObject.transform.localScale = (float) specimenData.scale * Vector3.one;

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

                        BoxCollider newBoxCollider = null;

                        if (addBoxColliderToParentAfterSpawning)
                        {
                            newBoxCollider = spawnedObject.AddComponent<BoxCollider>();
                            //newBoxCollider.transform.localScale *= 0.15f;
                        }

                        if (addGrabbableAsChild)
                        {
                            DistanceGrabbable_Expanded grabbable = Instantiate(grabbablePrefab, spawnedObject.transform);

                            grabbable.optionalExternalCollider = newBoxCollider;
                            grabbable.optionalExternalRigidbody = newRigidbody;

                            grabbable.enabled = true;
                        }

                    }

                }
                catch (Exception e)
                {
                    Debug.Log("error loading specimen " + e.Message);
                }
            }

        }

    }
    public IEnumerator SpawnLocalPrefabCoroutine(SpecimenData_New specimenData)
    {
        //used with local prefabs that don't need to be downloaded at runtime

        yield return new WaitForSeconds(0.1f); // Allows animations to run without hiccuping

        GameObject spawnedObject = Instantiate(specimenData.localPrefab, spawnLocation.position, spawnLocation.rotation);

        Debug.Log("specimen spawned " + spawnedObject.name);

        spawnedObject.transform.localScale = (float)specimenData.scale * Vector3.one;

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

        BoxCollider newBoxCollider = null;

        if (addBoxColliderToParentAfterSpawning)
        {
            newBoxCollider = spawnedObject.AddComponent<BoxCollider>();
            newBoxCollider.size = new Vector3(
                (0.15f / newBoxCollider.transform.lossyScale.x),
                (0.15f / newBoxCollider.transform.lossyScale.y),
                (0.15f / newBoxCollider.transform.lossyScale.z)
                )
                ;
            //newBoxCollider.transform.localScale *= 0.15f;
            //find current global scale and do math to set scale = 0.15f;
        }

        if (addGrabbableAsChild)
        {
            DistanceGrabbable_Expanded grabbable = Instantiate(grabbablePrefab, spawnedObject.transform);

            grabbable.transform.localScale = new Vector3(
                (0.15f / grabbable.transform.lossyScale.x),
                (0.15f / grabbable.transform.lossyScale.y),
                (0.15f / grabbable.transform.lossyScale.z)
                )
                ;

            grabbable.optionalExternalCollider = newBoxCollider;
            grabbable.optionalExternalRigidbody = newRigidbody;

            grabbable.enabled = true;
        }


    }

    public void SpawnModelByIndex(int index)
    {
        if (specimensListSO == null)
            return;

        if (specimensListSO.specimens.Count == 0)
            return;


        SpecimenData_New specimen = specimensListSO.specimens[index];

        Debug.Log("requesting spawn for " + specimen.name);

        StartCoroutine(SpawnLocalPrefabCoroutine(specimen));
    }
    public void SpawnFirstModelInList(SpecimensListSO specimensList)
    {
        if (specimensList == null)
            return;

        if (specimensList.specimens.Count == 0)
            return;


        SpecimenData_New specimen = specimensList.specimens[1];

        Debug.Log("requesting spawn for " + specimen.name);

        StartCoroutine(SpawnLocalPrefabCoroutine(specimen));
    }
    public void SpawnRandomModel(SpecimensListSO specimensList)
    {
        if (specimensList == null)
            return;

        if (specimensList.specimens.Count == 0)
            return;


        SpecimenData_New specimen = ChooseRandomModel(specimensList);

        Debug.Log("requesting spawn for " + specimen.name);

        StartCoroutine(SpawnModelCoroutine(specimen));
    }

    public SpecimenData_New ChooseRandomModel(SpecimensListSO specimensList)
    {
        int randomIndex = UnityEngine.Random.Range(0, specimensList.specimens.Count);

        SpecimenData_New selectedSpecimen = specimensList.specimens[randomIndex];
        /*
          
        //trying to ensure valid URL:

        using (UnityWebRequest req =
        UnityWebRequestAssetBundle.GetAssetBundle(selectedSpecimen.assetUrl, 0U))
        {
            req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.ProtocolError)
            {
                return ChooseRandomModel(specimensList);
            }
        }
        */
        return selectedSpecimen;

    }

}
