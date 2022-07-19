using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

/*
struct AnnotationData2
{
    public string id, title, content;
    public bool global;
    public Vector3 position;
}*/

public class TestModelLoader : MonoBehaviour
{
    
    public string reqUri = "https://hivemodelstorage.blob.core.windows.net/win/handbones";
    public string prefabPath = "Assets/Models/Specimens/AssetBundles/HandBones.prefab";
    public string annotationID = "hand_bones"; //Infratemporalfossa2, l4lungswithtrachea, "Brain Optic", "Brain before Sections", hand_and_forearm

    public Transform spawnLocation;
    public GameObject annotationLabelPrefab;

    //MongoDB
    MongoClient client = new MongoClient("mongodb://hive:8afDe1K6XwY1W5cy@van-vr-shard-00-00.zr7vf.mongodb.net:27017,van-vr-shard-00-01.zr7vf.mongodb.net:27017,van-vr-shard-00-02.zr7vf.mongodb.net:27017/test?authSource=admin&replicaSet=atlas-afl4g3-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true");
    IMongoDatabase database;
    bool manifestLoaded;

    IMongoCollection<BsonDocument> specimenCollection;

    GameObject remoteModelPrefab;

    private void Start()
    {
        database = client.GetDatabase("vanvr");
        specimenCollection = database.GetCollection<BsonDocument>("specimens");

        TestLoadBundle();
    }


    //returns null of specimenID is invalid 
    private Annotation[] GetAnnotationBySpecimenID(string specimenID)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("id", specimenID);
        var result = specimenCollection.Find(filter);

        try
        {

            if(result != null)
            {
                if (result.FirstOrDefault() == null) return null;

                if (result.FirstOrDefault().Contains("name"))
                {
                    print($"Retrieving annotation from specimen name: {result.FirstOrDefault().GetValue("name").AsString}");
                }


                if (result.FirstOrDefault().Contains("annotations"))
                {
                    var annotations = result.FirstOrDefault().GetValue("annotations").AsBsonArray;

                    Annotation[] resultingArray = new Annotation[annotations.Count];
                    
                    //assign values to the annotation return array "resultingArray"
                    for(int i = 0; i < annotations.Count; i++)
                    {
                        resultingArray[i].id = annotations[i].AsBsonDocument.GetValue("annotationId").AsString;
                        resultingArray[i].title = annotations[i].AsBsonDocument.GetValue("title").AsString;
                        resultingArray[i].content = annotations[i].AsBsonDocument.GetValue("content").AsString;

                        if (annotations[i].AsBsonDocument.GetValue("position").AsBsonDocument.Contains("global")) resultingArray[i].global = annotations[i].AsBsonDocument.GetValue("position").AsBsonDocument.GetValue("global").AsBoolean;
                        if (annotations[i].AsBsonDocument.GetValue("position").AsBsonDocument.Contains("x")) resultingArray[i].position.x = (float)annotations[i].AsBsonDocument.GetValue("position").AsBsonDocument.GetValue("x").ToDecimal(); 
                        if (annotations[i].AsBsonDocument.GetValue("position").AsBsonDocument.Contains("y")) resultingArray[i].position.y = (float)annotations[i].AsBsonDocument.GetValue("position").AsBsonDocument.GetValue("y").ToDecimal();
                        if (annotations[i].AsBsonDocument.GetValue("position").AsBsonDocument.Contains("z")) resultingArray[i].position.z = (float)annotations[i].AsBsonDocument.GetValue("position").AsBsonDocument.GetValue("z").ToDecimal();

                    }

                    return resultingArray;
                }
            }


        }catch(Exception e)
        {
            if (e.Message != null) Debug.LogError($"Exception occured in TestModelLoader: {e.Message}");
            
        }

        return null;
    }


    //Loads the 3D model from database
    private IEnumerator LoadFromData()
    {
        yield return new WaitForSeconds(0.1f); // Allows animations to run without hiccuping

        using (UnityWebRequest req =
        UnityWebRequestAssetBundle.GetAssetBundle(reqUri, 0U))
        {
            yield return req.SendWebRequest();

            if (req.isNetworkError || req.isHttpError)
            {
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
                        print("we got here");
                        remoteModelPrefab = bundle.LoadAsset<GameObject>(prefabPath);


                        Renderer rend = remoteModelPrefab.transform.GetChild(0).GetComponent<Renderer>();
                        if (rend != null)
                        {
                            rend.sharedMaterial.shader = Shader.Find("Universal Render Pipeline/Lit");
                        }


                        Instantiate(remoteModelPrefab, spawnLocation);

                        var annotations = GetAnnotationBySpecimenID(annotationID);
                        if (annotations != null)
                        {
                            print($"Annotations list count: {annotations.Length}");
                            int count = 0;
                            foreach (var annotation in annotations)
                            {
                                if(annotation.global == false)
                                {
                                    GameObject label = Instantiate(annotationLabelPrefab, spawnLocation);
                                    label.transform.position = new Vector3(annotation.position.x, annotation.position.y, annotation.position.z);
                                    label.GetComponent<TextMeshPro>().text = annotation.id + ": " + annotation.title;
                                }

                            }

                        }


                        spawnLocation.transform.localScale = new Vector3(0.0105851f, 0.0105851f, 0.0105851f);
                        spawnLocation.transform.position = new Vector3(2.055f, 1.7352f, -0.956f);

                    }
                    
                }
                catch (Exception e)
                {
                    print("error");
                }
            }

        }
    }

    
    //Loads model from database, using assetbundle
    public void TestLoadBundle()
    {
        StartCoroutine(LoadFromData());
    }



    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            var annotations = GetAnnotationBySpecimenID(annotationID);
            if (annotations != null)
            {
                print($"Annotations list count: {annotations.Length}");
                int count = 0;
                foreach(var annotation in annotations)
                {
                    print($"Annotation {count++}: ");
                    print($"Title: {annotation.title} - Content: {annotation.content}");
                    print($"Global: {annotation.global} Position: {annotation.position.x},{annotation.position.y},{annotation.position.z}");
                    print("----------------------------------------------------------");
                }

            }
        }
    }

    
}
