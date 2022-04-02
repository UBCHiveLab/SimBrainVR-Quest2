using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;

/**
 * Extends DataLoader. For fetching a manifest located at manifestPath as an http resource.
 */
public class RemoteDataLoader : DataLoader
{


    //MongoDB
    MongoClient client = new MongoClient("mongodb://hive:8afDe1K6XwY1W5cy@van-vr-shard-00-00.zr7vf.mongodb.net:27017,van-vr-shard-00-01.zr7vf.mongodb.net:27017,van-vr-shard-00-02.zr7vf.mongodb.net:27017/test?authSource=admin&replicaSet=atlas-afl4g3-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true");
    IMongoDatabase database;


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

      LoadManifestFromMongoDB(); 

        manifestLoaded = true;
        yield break;
    }

    
    private void LoadManifestFromMongoDB()
    {
        IMongoCollection<BsonDocument> specimenCollection;
        database = client.GetDatabase("vanvr");
        specimenCollection = database.GetCollection<BsonDocument>("specimens");

        var filter = Builders<BsonDocument>.Filter.Empty;

        var specimens = specimenCollection.Find(filter).ToList();

        manifest = new DataManifest();
        manifest.specimenData = new SpecimenRequestData[specimens.Count];


        for (int i = 0; i < specimens.Count; i++)
        {
            /*
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

            */


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
    }
    

}

