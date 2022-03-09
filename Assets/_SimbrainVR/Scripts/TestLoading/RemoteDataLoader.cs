//using MongoDB.Bson;
//using MongoDB.Driver;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/**
 * Extends DataLoader. For fetching a manifest located at manifestPath as an http resource.
 */
public class RemoteDataLoader : DataLoader
{


    //MongoDB
  //  MongoClient client = new MongoClient("mongodb://hive:8afDe1K6XwY1W5cy@van-vr-shard-00-00.zr7vf.mongodb.net:27017,van-vr-shard-00-01.zr7vf.mongodb.net:27017,van-vr-shard-00-02.zr7vf.mongodb.net:27017/test?authSource=admin&replicaSet=atlas-afl4g3-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true");
 //   IMongoDatabase database;


    protected override IEnumerator LoadManifest()
    {
        /*
        using (UnityWebRequest req =
            UnityWebRequest.Get(manifestPath))
        {
            req.SetRequestHeader("Cache-Control", "max-age=0, no-cache, no-store");
            req.SetRequestHeader("Pragma", "no-cache");
            yield return req.SendWebRequest();
            if (req.isNetworkError || req.isHttpError)
            {
                SendError($"Unable to get manifest. Please check your internet connection or contact the administrator.");
            } else
            {
                manifest = JsonUtility.FromJson<DataManifest>(req.downloadHandler.text);
                manifestLoaded = true;
            }
        }*/

        //Using MongoDB

    //   LoadManifestFromMongoDB(); //UNCOMMENT

        manifestLoaded = true;
        yield break;
    }

/*
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
    }
    */

}

