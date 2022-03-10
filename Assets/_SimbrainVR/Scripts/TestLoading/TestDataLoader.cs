using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; 

/**
* Loads SpecimenData -- including mesh and texture -- from a specimen request
* If replace is selected, the data will replace any appropriate data in the
* current SpecimenStore
*/

public class TestDataLoader : MonoBehaviour
{

    //public string prefabPath = "Assets/Models/Specimens/AssetBundles/Lungs_with_bronchi_656K.prefab"; 
    public string reqUri = "https://hivemodelstorage.blob.core.windows.net/win/right_lung";
    public string prefabPath = "Assets/Models/Specimens/AssetBundles/right_lung.prefab";
    public Transform specimenContent;
    public Button button; 
    private IEnumerator LoadFromData()
    {
        yield return new WaitForSeconds(0.1f); // Allows animations to run without hiccuping 
        using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(reqUri, 0U))
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
                        //EDIT: better to make a list and check for all children and ensure they're all the same shader
                        Renderer rendChildren = rend.transform.GetChild(0).GetChild(0).GetComponent<Renderer>(); 
                        if (rend != null)
                        {
                            rend.sharedMaterial.shader = Shader.Find("Sprites/Default");
                            if (rendChildren != null)
                            {
                                rendChildren.sharedMaterial.shader = Shader.Find("Sprites/Default"); 
                            }
                        }
                        Instantiate(prefab, specimenContent);
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
            TestLoadBundle();
        }
    }
}

