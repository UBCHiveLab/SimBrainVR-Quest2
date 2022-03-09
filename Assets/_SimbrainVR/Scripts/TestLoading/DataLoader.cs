using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using System.Runtime.InteropServices;

/**
 * Spawned by SpecimenStore and responsible for all processes related to fetching manifest.json and online asset bundles.
 */
public abstract class DataLoader : MonoBehaviour
{

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void OpenNewTab(string url);
#endif

    [Header("Services")]
    public SpecimenStore store;

    [Header("Data")]
    public DataManifest manifest;
    public string manifestPath;
    public bool manifestLoaded;
    public string status;
    public bool manifestVerified;

    private List<SpecimenData> _specimens;
    private List<RegionData> _regions;
    private int _requestsResolved;
    private bool _loaded;
    private HashSet<string> _currentLoadingIds = new HashSet<string>(); // Keeps track of live requests so we don't double up
    private Transform loaderParent;
    private bool isDownload = false;


    protected abstract IEnumerator LoadManifest();

    public void Load(bool loadAllData)
    {
        StartCoroutine(Loading(loadAllData));
    }

    public bool Loaded()
    {
        return _loaded;
    }

    public List<SpecimenData> GetSpecimens()
    {
        return _specimens;
    }

 
    public List<RegionData> GetRegions()
    {
        return _regions;
    }

    public void LoadSpecimenAssets(string id)
    {
        SpecimenData data = store.GetSpecimen(id);
        SpecimenRequestData srd = data.request;
        StartCoroutine(LoadFromData(srd, true));
    }

    private IEnumerator Loading(bool loadAllData)
    {
        _loaded = false;
        manifestLoaded = false;
        Stopwatch watch = Stopwatch.StartNew();

        status = "Waiting for caching";
        Debug.Log("Waiting for caching");

        // Wait for the caching system to be ready
        while (!Caching.ready) yield return null;
        Debug.Log("Caching ready, loading manifest.");

        status = "Loading manifest";
        // Gets and verifies manifest file
        StartCoroutine(LoadManifest());
        while (!manifestLoaded) yield return null;
        manifestVerified = VerifyManifest(manifest);

        if (!manifestVerified)
        {
            Debug.LogWarning("Some issues found with the given manifest.");
        }

        //test

        // Regions and courses are stored directly in the manifests
        _regions = manifest.regions.ToList();

        //mongo test
        //_regions = manifest.regions.ToList();

        // having lab courses is optional (see VerifyManifest method). So, if no labs were loaded, make an empty list

        _specimens = new List<SpecimenData>();

        // Tracks the number of requests returned, even if failed
        _requestsResolved = 0;

        if (!loadAllData)
        {
            foreach (SpecimenRequestData srd in manifest.specimenData)
            {
                LoadSpecimenDataNoMeshTex(srd);
            }
            _loaded = true;
            watch.Stop();
            status = "Load successful";
            Debug.Log("Loaded manifest and specimen stubs.");
            yield break;
        }

        status = $"Loading Specimens [0/{manifest.specimenData.Length}]";
        foreach (SpecimenRequestData sprd in manifest.specimenData)
        {
            // Load specimen data from request. Must increment requestsResolved when finished, even if failed!
            StartCoroutine(LoadFromData(sprd));
            yield return null;
        }


        // Wait until all requests are resolved
        while (_requestsResolved < manifest.specimenData.Length)
        {
            status = $"Loading Specimens [{_requestsResolved}/{manifest.specimenData.Length}]";
            yield return new WaitForSeconds(1f); // The wait allows for ui interactions while loading!

        }
        watch.Stop();
        status = "Load successful";

        // Listeners can now check IsLoading() and know that the loader has completed.
        _loaded = true;

        Debug.Log($"Loaded {_specimens.Count} total specimens and {_regions.Count} regions.");
        Debug.Log($"Load time: {watch.Elapsed.TotalSeconds} seconds");
    }

    /**
     * Loads SpecimenData -- including mesh and texture -- from a specimen request
     * If replace is selected, the data will replace any appropriate data in the
     * current SpecimenStore
     */
    private IEnumerator LoadFromData(SpecimenRequestData srd, bool replace = false)
    {
        yield return new WaitForSeconds(0.1f);  // Allows animations to run without hiccuping

        if (store.specimens.ContainsKey(srd.id) && store.specimens[srd.id].dataLoaded)
        {
            Debug.Log($"Asset bundle for specimen of id {srd.id} is already loaded");
            yield break;
        }

        if (_currentLoadingIds.Contains(srd.id))
        {
            Debug.Log($"Already waiting on a request for {srd.id}");
            yield break;
        }

        _currentLoadingIds.Add(srd.id);
        string reqUri = srd.assetUrl;           // Default to standalone windows packages
#if UNITY_WEBGL || UNITY_WEBGL_API || PLATFORM_WEBGL
        reqUri = srd.assetUrlWebGl;             // WebGl packages
#elif UNITY_STANDALONE_OSX
        reqUri = srd.assetUrlOsx;               // Osx packages
#endif
        using (UnityWebRequest req =
            UnityWebRequestAssetBundle.GetAssetBundle(reqUri, Convert.ToUInt32(srd.version), 0U))
        {
            Debug.Log("Trying the bundle download");
            isDownload = true;

            StartCoroutine(progress(req));

            yield return req.SendWebRequest();
            isDownload = false;

            if (req.isNetworkError || req.isHttpError || srd.prefabPath == "")
            {
                if (srd.altAssetUrl != "")
                {
                    // if couldn't load the asset bundle and there's an alternative url for the asset's content, open a new tab with the alt content
                    OpenAlternativeContent(srd.altAssetUrl);
                }
                else
                {
                     // to ensure the camera is focused on the right screen 
                                                   //  SendError($"{req.error} : Could not find bundle for {srd.id}. Please contact the department if this problem persists.");
                    yield break;
                }
            }
            else
            {
                // _currentLoadingIds.Add(srd.id);

                yield return new WaitForSeconds(.1f);

                // Get downloaded asset bundle

                try
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(req);
                    if (srd.prefabPath != null)
                    {
                        GameObject prefab = bundle.LoadAsset<GameObject>(srd.prefabPath);
                        SpecimenData specimenData = new SpecimenData()
                        {
                            id = srd.id,
                          //  annotations = srd.annotations.ToList(),
                            prefab = prefab,
                            version = srd.version,
                            organ = srd.organ,
                            scale = srd.scale,
                            yPos = srd.yPos,
                            name = srd.name,
                            request = srd
                        };
                        if (replace)
                        {
                            store.specimens[srd.id] = specimenData;
                        }

                        _specimens.Add(specimenData);
                    }
                    else
                    {
                        // Checks for null material and mesh; if not loadable, will not add the asset.
                        Material mat = bundle.LoadAsset<Material>(srd.matPath);
                        if (mat == null)
                        {
                            Debug.LogWarning($"Could not find material for {srd.id} at path {srd.matPath} in bundle. Please check your bundle structure and try again.");
                            foreach (string file in bundle.GetAllAssetNames())
                            {
                                Debug.Log(file);
                            }
                            throw new Exception();
                        }

                        Mesh mesh = bundle.LoadAsset<Mesh>(srd.meshPath);
                        if (mesh == null)
                        {
                            Debug.LogWarning($"Could not find mesh for {srd.id} at path {srd.meshPath} in bundle.  Please check your bundle structure and try again.");
                            throw new Exception();
                        }

                        // Asset seems good, add to the specimens list.
                        SpecimenData specimenData = new SpecimenData()
                        {
                            id = srd.id,
                           // annotations = srd.annotations.ToList(),
                            material = mat,
                            mesh = mesh,
                            version = srd.version,
                            organ = srd.organ,
                            scale = srd.scale,
                            yPos = srd.yPos,
                            name = srd.name,
                            request = srd
                        };
                        if (replace)
                        {
                            store.specimens[srd.id] = specimenData;
                        }
                        _specimens.Add(specimenData);

                    }
                }
                catch (Exception e)
                {
                    SendError($"{e} : Problem with {srd.id}");
                }
            }

            _currentLoadingIds.Remove(srd.id);
            // Must resolve requests in order to trigger loading finished
            _requestsResolved++;

        }

    }

    //Records progress % of downlaoding specimen
    private IEnumerator progress(UnityWebRequest req)
    {
        while (isDownload)
        {
            float loadPercentage = req.downloadProgress * 100;
            Debug.Log("Downloading:" + req.downloadProgress * 100 + "%");
            yield return new WaitForSeconds(0.1f);
        }
    }


    /**
     * Able to open an alternative link
     */
    private void OpenAlternativeContent(string altContentLink)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        OpenNewTab(altContentLink);
#else
        Application.OpenURL(altContentLink);
#endif
    }

    private void LoadSpecimenDataNoMeshTex(SpecimenRequestData srd)
    {
        SpecimenData specimenData = new SpecimenData()
        {
            id = srd.id,
         //   annotations = srd.annotations.ToList(),
            prefab = null,
            version = srd.version,
            organ = srd.organ,
            scale = srd.scale,
            yPos = srd.yPos,
            name = srd.name,
            request = srd
        };
        _specimens.Add(specimenData);
    }

    /**
     * Manifest verification -- will need to change this 
     */
    private bool VerifyManifest(DataManifest manifest)
    {
        if (manifest == null)
        {
            SendError("Couldn't load manifest. Make sure you are pointing to a correct, existing online or local resource.");
            return false;
        }

        bool verify = true;

        if (manifest.regions == null)
        {
            SendWarning("No region data in loaded manifest. Please add region data.");
            verify = false;
        }

        if (manifest.specimenData == null)
        {
            SendWarning("No specimen data in loaded manifest. Please add specimen data if desired.");
            verify = false;
        }

        return verify;
    }

    /**
     * Convenience methods for sending error messages or logging warnings.
     */
    protected void SendError(string message)
    {
        Debug.LogError(message);
    }

    protected void SendWarning(string message)
    {
        Debug.LogWarning(message);
    }
}
