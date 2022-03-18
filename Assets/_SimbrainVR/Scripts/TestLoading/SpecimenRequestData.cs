
/**
 * For deserializing specimen data from the manifest; is transformed into SpecimenData once loaded.
 */
[System.Serializable]
public class SpecimenRequestData
{
    public string id;
    public string name;
    public int version;
    public string organ;
    public string assetUrl;
    public string assetUrlWebGl;
    public string assetUrlOsx;
    public string altAssetUrl; // link to asset on alternative source (e.g. SketchFab)
    public string meshPath;
    public string matPath;
    public string prefabPath;
    public float scale;
    public float yPos;
  //  public AnnotationData[] annotations;

}
