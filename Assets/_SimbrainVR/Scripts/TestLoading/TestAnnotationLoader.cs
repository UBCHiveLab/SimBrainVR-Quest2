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

public class TestAnnotationLoader : MonoBehaviour
{

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


    //returns null if specimenID is invalid 
    private Annotation[] GetAnnotationBySpecimenID(string specimenID)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("id", specimenID);
        var result = specimenCollection.Find(filter);

        try
        {

            if (result != null)
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
                    for (int i = 0; i < annotations.Count; i++)
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


        }
        catch (Exception e)
        {
            if (e.Message != null) Debug.LogError($"Exception occured in TestAnnotationLoader: {e.Message}");

        }

        return null;
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
                foreach (var annotation in annotations)
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
