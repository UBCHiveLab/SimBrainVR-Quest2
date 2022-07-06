using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[CreateAssetMenu(fileName = "Persistent Save Utility", menuName = "ScriptableObjects/Persistent Save Utility")]
public class PersistentSaveUtilitySO : ScriptableObject
{
    [SerializeField] private string folderNameInPersistentDataPath = "SaveFiles";

    public UnityEvent<ScriptableObject> OnScriptableObjectLoaded = default;
    public UnityEvent<ScriptableObject> OnScriptableObjectSaved = default;
    /*
    [Header("Only for debug viewing, do not edit this list manually")]
    public List<SaveableObject> saveableObjects = new List<SaveableObject>();
    */
    private string SaveFolderFullDirectory
    {
        get => Application.persistentDataPath + "/" + folderNameInPersistentDataPath;
    }

    //private string lastLocalDataPath = "";

    public void Save(ScriptableObject scriptableObject)
    {
        //Debug.Log("save request " + scriptableObject.name);

        if (scriptableObject == null)
            return;

        if (!SaveDirectoryExists())
        {
            CreateSaveDirectory();
        }

        if (!SaveFileExists(scriptableObject))
        {
        }

        BinaryFormatter bf = new BinaryFormatter();
        string path = GetSavePathForScriptableObject(scriptableObject);
        FileStream fs = File.Create(path);
        string json = JsonUtility.ToJson(scriptableObject);
        bf.Serialize(fs, json);
        fs.Close();

        Debug.Log("file saved " + scriptableObject.name);

        OnScriptableObjectSaved?.Invoke(scriptableObject);
    }

    public void Load(ScriptableObject scriptableObject)
    {
        //Debug.Log("load request " + scriptableObject.name);

        if (scriptableObject == null)
            return;

        if (!SaveFileExists(scriptableObject))
        {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(GetSavePathForScriptableObject(scriptableObject), FileMode.Open);
        JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), scriptableObject);
        file.Close();

        Debug.Log("file loaded " + scriptableObject.name);

        OnScriptableObjectLoaded?.Invoke(scriptableObject);
    }
    /*
    public void LoadFromSpecificLocalJsonPath(ScriptableObject scriptableObject)
    {
        //Debug.Log("load request " + scriptableObject.name);

        if (scriptableObject == null)
            return;

        if (lastLocalDataPath == "")
            return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(lastLocalDataPath, FileMode.Open);
        JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), scriptableObject);
        file.Close();

        Debug.Log("file loaded " + scriptableObject.name);

        OnScriptableObjectLoaded?.Invoke(scriptableObject);

        lastLocalDataPath = "";
    }
    */
    public void Delete(ScriptableObject scriptableObject)
    {
        if (scriptableObject == null)
            return;

        if (SaveFileExists(scriptableObject))
        {
            string path = GetSavePathForScriptableObject(scriptableObject);

            //DirectoryInfo directory = new DirectoryInfo(path);
            //directory.Delete(true);

            File.Delete(path);

            Debug.Log("file deleted " + scriptableObject.name);
        }

    }
    private bool SaveDirectoryExists()
    {
        bool returner = File.Exists(SaveFolderFullDirectory);

        //Debug.Log(SaveFolderFullDirectory + " exists? " + returner);
        
        return returner;
    }
    
    private bool SaveFileExists(ScriptableObject scriptableObject)
    {
        bool returner = File.Exists(GetSavePathForScriptableObject(scriptableObject));

        //Debug.Log(GetSavePathForScriptableObject(scriptableObject) + " exists? " + returner);

        return returner;


    }

    private void CreateSaveDirectory()
    {
        Directory.CreateDirectory(SaveFolderFullDirectory);

    }

    private string GetSavePathForScriptableObject(ScriptableObject scriptableObject)
    {
        return SaveFolderFullDirectory + "/" + scriptableObject.name;

    }
    /*
    public void SetUpLocalDataPathForJson(string pathAfterAssetsFolder)
    {
        lastLocalDataPath = Application.dataPath + pathAfterAssetsFolder;
    }
    */
    /*
    public void AddSaveableObjectToList(SaveableObject saveableObject)
    {

        #if UNITY_EDITOR

        foreach (SaveableObject objectInList in saveableObjects)
        {
            if (objectInList.guid.Equals(saveableObject.guid))
            {
                return;
            }
            else if (objectInList.scriptableObject == saveableObject.scriptableObject)
            {
                Debug.LogWarning("Found saveable objects with same SO " + objectInList.scriptableObject.name
                    + " but different GUIDs, new is " + saveableObject.guid
                    + ", old is " + objectInList.guid);

                return;
            }
        }

        saveableObjects.Add(saveableObject);

        UnityEditor.EditorUtility.SetDirty(this);

        #endif
    }
    
    public SaveableObject GetScriptableObjectForGuid(string guid)
    {
        foreach (SaveableObject saveableObject in saveableObjects)
        {
            if (saveableObject.guid.Equals(guid))
            {
                return saveableObject;
            }
        }

        return null;
    }

    public void CleanUpObsoleteReferences()
    {
        #if UNITY_EDITOR

        List<ScriptableObject> alreadySeenScriptableObjects = new List<ScriptableObject>();
        List<SaveableObject> objectsToRemove = new List<SaveableObject>();

        foreach (SaveableObject saveableObject in saveableObjects)
        {
            if ((saveableObject.scriptableObject == null)
                || (alreadySeenScriptableObjects.Contains(saveableObject.scriptableObject)))
            {
                objectsToRemove.Add(saveableObject);
            }
            else
            {
                alreadySeenScriptableObjects.Add(saveableObject.scriptableObject);

            }

        }

        foreach (SaveableObject saveableObjectToRemove in objectsToRemove)
        {
            saveableObjects.Remove(saveableObjectToRemove);

        }

        if (objectsToRemove.Count > 0)
        {
            UnityEditor.EditorUtility.SetDirty(this);

        }

        #endif

    }
    */
}
