using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

struct Annotation
{
    public string id, title, content;
    public bool global;
    public Vector3 position;
}

public class TestModelLoader : MonoBehaviour
{
    public string reqUri = "https://hivemodelstorage.blob.core.windows.net/win/brainoptic";
    public string prefabPath = "Assets/Models/Specimens/AssetBundles/BrainOptic.prefab";
    public string annotationID = "Infratemporalfossa2"; //Infratemporalfossa2, l4lungswithtrachea, "Brain Optic", "Brain before Sections", hand_and_forearm

    //MongoDB
    MongoClient client = new MongoClient("mongodb://hive:8afDe1K6XwY1W5cy@van-vr-shard-00-00.zr7vf.mongodb.net:27017,van-vr-shard-00-01.zr7vf.mongodb.net:27017,van-vr-shard-00-02.zr7vf.mongodb.net:27017/test?authSource=admin&replicaSet=atlas-afl4g3-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true");
    IMongoDatabase database;
    bool manifestLoaded;

    IMongoCollection<BsonDocument> specimenCollection;

    private void Start()
    {
        database = client.GetDatabase("vanvr");
        specimenCollection = database.GetCollection<BsonDocument>("specimens");
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

    IEnumerator LoadManifest()
    {
        LoadManifestFromMongoDB();

        manifestLoaded = true;
        yield break;
    }

    private void LoadManifestFromMongoDB()
    {
        IMongoCollection<BsonDocument> specimenCollection, courseCollection, regionCollection, labsCollection;
        database = client.GetDatabase("vanvr");
        specimenCollection = database.GetCollection<BsonDocument>("specimens");
        courseCollection = database.GetCollection<BsonDocument>("courses");
        regionCollection = database.GetCollection<BsonDocument>("regions");
        labsCollection = database.GetCollection<BsonDocument>("labs");

        var filter = Builders<BsonDocument>.Filter.Empty;

        var specimens = specimenCollection.Find(filter).ToList();
        var courses = courseCollection.Find(filter).ToList();
        var regions = regionCollection.Find(filter).ToList();

        /*
        manifest = new DataManifest();
        manifest.specimenData = new SpecimenRequestData[specimens.Count];


        for (int i = 0; i < specimens.Count; i++)
        {

            //Populate all fields
            manifest.specimenData[i] = new SpecimenRequestData();
            manifest.specimenData[i].id = specimens[i].GetValue("id").AsString;
            manifest.specimenData[i].name = specimens[i].GetValue("name").AsString;
            if (specimens[i].Contains("version")) manifest.specimenData[i].version = specimens[i].GetValue("version").AsInt32;
            if (specimens[i].Contains("organ")) manifest.specimenData[i].organ = specimens[i].GetValue("organ").AsString;
            if (specimens[i].Contains("assetUrl")) manifest.specimenData[i].assetUrl = specimens[i].GetValue("assetUrl").AsString;
            if (specimens[i].Contains("assetUrlWebGl")) manifest.specimenData[i].assetUrlWebGl = specimens[i].GetValue("assetUrlWebGl").AsString;
            if (specimens[i].Contains("assetUrlOsx")) manifest.specimenData[i].assetUrlOsx = specimens[i].GetValue("assetUrlOsx").AsString;
            if (specimens[i].Contains("altAssetUrl")) manifest.specimenData[i].altAssetUrl = specimens[i].GetValue("altAssetUrl").AsString;
            if (specimens[i].Contains("prefabPath")) manifest.specimenData[i].prefabPath = specimens[i].GetValue("prefabPath").AsString;
            if (specimens[i].Contains("scale")) manifest.specimenData[i].scale = (float)specimens[i].GetValue("scale").ToDecimal();
            if (specimens[i].Contains("yPos")) manifest.specimenData[i].yPos = (float)specimens[i].GetValue("yPos").ToDecimal();




            //Now populate SpecimenRequestData.AnnotationData[] array
            if (specimens[i].Contains("annotations"))
            {
                var annotations = specimens[i].GetValue("annotations").AsBsonArray;
                manifest.specimenData[i].annotations = new AnnotationData[annotations.Count];
                for (int y = 0; y < annotations.Count; y++)
                {
                    manifest.specimenData[i].annotations[y] = new AnnotationData(annotations[y].AsBsonDocument.GetValue("annotationId").AsString,
                                                                                 annotations[y].AsBsonDocument.GetValue("title").AsString,
                                                                                 annotations[y].AsBsonDocument.GetValue("content").AsString,
                                                                                 new AnnotationNullablePosition());

                    if (annotations[y].AsBsonDocument.GetValue("position").AsBsonDocument.Contains("global")) manifest.specimenData[i].annotations[y].position.global = annotations[y].AsBsonDocument.GetValue("position").AsBsonDocument.GetValue("global").AsBoolean;
                    if (annotations[y].AsBsonDocument.GetValue("position").AsBsonDocument.Contains("x")) manifest.specimenData[i].annotations[y].position.x = (float)annotations[y].AsBsonDocument.GetValue("position").AsBsonDocument.GetValue("x").ToDecimal(); // .AsDouble;
                    if (annotations[y].AsBsonDocument.GetValue("position").AsBsonDocument.Contains("y")) manifest.specimenData[i].annotations[y].position.y = (float)annotations[y].AsBsonDocument.GetValue("position").AsBsonDocument.GetValue("y").ToDecimal();
                    if (annotations[y].AsBsonDocument.GetValue("position").AsBsonDocument.Contains("z")) manifest.specimenData[i].annotations[y].position.z = (float)annotations[y].AsBsonDocument.GetValue("position").AsBsonDocument.GetValue("z").ToDecimal();

                }
            }
        
        }


        manifest.labCourses = new CourseData[courses.Count];
        for (int i = 0; i < courses.Count; i++)
        {
            manifest.labCourses[i] = new CourseData();
            manifest.labCourses[i].courseId = courses[i].GetValue("courseId").AsString;

            var labs = courses[i].GetValue("labs").AsBsonArray;
            manifest.labCourses[i].labs = new LabData[labs.Count];

            for (int y = 0; y < labs.Count; y++)
            {
                var filterLab = Builders<BsonDocument>.Filter.Eq("labId", labs[y].AsInt32);
                var filteredLab = labsCollection.Find(filterLab);
                if (filteredLab.FirstOrDefault() != null)
                {
                    var lab = filteredLab.FirstOrDefault().AsBsonDocument;

                    manifest.labCourses[i].labs[y] = new LabData();

                    manifest.labCourses[i].labs[y].labName = lab.GetValue("labName").AsString;
                    manifest.labCourses[i].labs[y].labId = labs[y].AsInt32;
                    manifest.labCourses[i].labs[y].imgUrl = lab.GetValue("imgUrl").AsString;

                    var specimenLists = lab.GetValue("specimenList").AsBsonArray;
                    manifest.labCourses[i].labs[y].specimenList = new string[specimenLists.Count];
                    for (int k = 0; k < specimenLists.Count; k++)
                    {
                        manifest.labCourses[i].labs[y].specimenList[k] = specimenLists[k].AsString;
                    }
                }

            }
        }


        manifest.regions = new RegionData[regions.Count];
        for (int i = 0; i < regions.Count; i++)
        {
            manifest.regions[i] = new RegionData();
            manifest.regions[i].name = regions[i].GetValue("name").AsString;
            manifest.regions[i].order = regions[i].GetValue("order").AsInt32;

            var organs = regions[i].GetValue("organs").AsBsonArray;
            manifest.regions[i].organs = new string[organs.Count];
            for (int y = 0; y < organs.Count; y++)
            {
                manifest.regions[i].organs[y] = organs[y].AsString;
            }
        }
        */
    }

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
                        GameObject prefab = bundle.LoadAsset<GameObject>(prefabPath);



                        Renderer rend = prefab.transform.GetChild(0).GetComponent<Renderer>();
                        if (rend != null)
                        {
                            rend.sharedMaterial.shader = Shader.Find("Standard");
                        }




                        Instantiate(prefab);
                        print(prefab.name);



                    }
                    else
                    {
                    }
                }
                catch (Exception e)
                {
                    print("error");
                }
            }

        }
    }



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
