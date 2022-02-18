using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Food, 
    Equipment, 
    Default
}
public abstract class ItemObject : ScriptableObject //base class for creating our items
{
    public GameObject prefab; 
    public ItemType type;
    [TextArea(15,20)]
    public string description; 

}
