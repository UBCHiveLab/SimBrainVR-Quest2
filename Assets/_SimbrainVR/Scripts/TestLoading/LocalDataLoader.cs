using System.Collections;
using UnityEngine;

/**
 * Extends DataLoader. For fetching a manifest located at manifestPath from Resources within the project.
 */
public class LocalDataLoader : DataLoader
{
    protected override IEnumerator LoadManifest()
    {
        TextAsset file = Resources.Load<TextAsset>(manifestPath);
        manifest = JsonUtility.FromJson<DataManifest>(file.text);
        manifestLoaded = true;
        yield break;
    }

}
