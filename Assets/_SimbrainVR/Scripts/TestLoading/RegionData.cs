using System;
using UnityEngine;

/**
 * For deserializing region data.
 */
[Serializable]
public class RegionData
{
    public string name;
    public string[] organs;
    public int order;
    public Sprite icon;
}
