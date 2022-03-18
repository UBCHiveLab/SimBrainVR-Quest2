using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

/**
 * Responsible accessing and distributing all specimens and related content.
 */
public class SpecimenStore : MonoBehaviour
{
    [Header("Manifest Data")]
    public bool
        manifestLocal =
            true; // If true, we will look for a manifest file in the resources folder; otherwise, we will make a request to a given url

    public string manifestLocalPath = "manifest"; // The local path to look for current manifest.

    public string
        manifestUrlPath = "http://example.net/manifest.json"; // The url location to look for the current manifest.

    public bool
        loadAllSpecimenAssets; // Select this to load ALL specimen data at app start; otherwise, loads asset bundles with mesh/materials when selected

    public string testUrl = "http://google.com";
    public GameObject SpecimenErrorLoadingPopUp;

    [Header("Stored Data")] public List<RegionData> regions;
    public Dictionary<string, SpecimenData> specimens;
    public Dictionary<string, Dictionary<string, List<SpecimenData>>> specimensByRegionByOrgan;
    public Dictionary<string, RegionData> organToRegion;
    public Dictionary<string, List<Tuple<string, List<SpecimenData>>>> courseSpecimens;

    private bool _loading = true;
    private DataLoader loader;


    public List<string> GetSpecimenIdsList()
    {
        return specimens.Keys.ToList();
    }

    public SpecimenData GetSpecimen(string id)
    {
        if (!specimens.ContainsKey(id))
        {
            Debug.LogWarning($"No specimen found with id {id}");
            return null;
        }

        return specimens[id];
    }

    public Dictionary<string, List<SpecimenData>> GetSpecimensByRegion(string region)
    {
        if (!specimensByRegionByOrgan.ContainsKey(region))
        {
            Debug.LogWarning($"No specimen region found with id {region}");
            return null;
        }

        return specimensByRegionByOrgan[region];
    }


    public List<SpecimenData> GetSpecimensByRegionOrgan(string region, string organ)
    {
        if (!specimensByRegionByOrgan.ContainsKey(region))
        {
            Debug.LogWarning($"No specimen region found for {region}");
            return new List<SpecimenData>();
        }

        if (!specimensByRegionByOrgan.ContainsKey(region))
        {
            Debug.LogWarning($"No specimen organ found for {organ} in {region}");
            return new List<SpecimenData>();
        }

        return specimensByRegionByOrgan[region][organ];
    }

    public List<string> GetOrgansByRegion(string region)
    {
        if (!specimensByRegionByOrgan.ContainsKey(region))
        {
            return new List<string>();
        }

        return specimensByRegionByOrgan[region].Keys.ToList();
    }

    public List<SpecimenData> GetSpecimenDataFiltered(List<string> filteredOutIds)
    {
        return specimens.Values.Where(spd => !filteredOutIds.Contains(spd.id)).ToList();
    }


    public bool Loading()
    {
        return _loading;
    }

    public string DumpCurrentSpecimenStructure()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Current Specimens: \n");
        foreach (var reg in specimensByRegionByOrgan)
        {
            sb.Append($"--{reg.Key}:\n");
            foreach (var org in reg.Value)
            {
                sb.Append($"----{org.Key}:\n");
                foreach (SpecimenData sd in org.Value)
                {
                    sb.Append($"------{sd.id}\n");
                }
            }
        }

        return sb.ToString();
    }

    public string GetStatus()
    {
        if (loader) return loader.status;
        return "";
    }

    private void Start()
    {
        StartCoroutine(TestInternet());
        StartCoroutine(LoadData());
    }

    public void LoadSpecimen(string id)
    {
        loader.LoadSpecimenAssets(id);
        //  AnalyzeOn(); 
    }

    /**
     * Tests the user's connection by sending a request to testUrl. Used to spawn the "please connect to the internet" message
     */
    private IEnumerator TestInternet()
    {
        using (UnityWebRequest req =
            UnityWebRequest.Get(testUrl))
        {
            yield return req.SendWebRequest();
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.LogWarning(req.error);
                Debug.Log("there is an error in the network");
            }
        }
    }

    private IEnumerator LoadData()
    {
        // Attachs the correct loader and sets the manifest path.
        if (manifestLocal)
        {
            loader = gameObject.AddComponent<LocalDataLoader>();
            loader.manifestPath = manifestLocalPath;
        }
        else
        {
            loader = gameObject.AddComponent<RemoteDataLoader>();
            loader.manifestPath = manifestUrlPath;
        }

        loader.store = this;

        // Waits for the loader to load the manifest and all connected bundles (could be long on first load, but once cached should be seconds)
        // if loadAllSpecimenAssets, will load the meshes/textures of all specimens at app start; otherwise, will smart load the requested assets
        loader.Load(loadAllSpecimenAssets);

        while (!loader.Loaded()) yield return null;

        // Builds all data structures needed for the SpecimenStore
        regions = loader.GetRegions().ToList();
        organToRegion = new Dictionary<string, RegionData>();

        foreach (RegionData region in regions)
        {
            foreach (string organ in region.organs)
            {
                organToRegion.Add(organ, region);
            }
        }
        specimens = loader.GetSpecimens().ToDictionary((spec => spec.id), spec => spec);
        specimensByRegionByOrgan = new Dictionary<string, Dictionary<string, List<SpecimenData>>>();

        foreach (SpecimenData spd in loader.GetSpecimens())
        {
            RegionData region = organToRegion[spd.organ];

            if (!specimensByRegionByOrgan.ContainsKey(region.name))
            {
                specimensByRegionByOrgan.Add(region.name, new Dictionary<string, List<SpecimenData>>());
            }

            if (!specimensByRegionByOrgan[region.name].ContainsKey(spd.organ))
            {
                specimensByRegionByOrgan[region.name].Add(spd.organ, new List<SpecimenData>());
            }

            specimensByRegionByOrgan[region.name][spd.organ].Add(spd);
        }

        // Turn loading off so that any listening UI can query for values.
        _loading = false;
    }


}