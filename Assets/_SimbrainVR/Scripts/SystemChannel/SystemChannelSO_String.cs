using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SystemChannelSO_String", menuName = "System Channels/New System Channel_String")]
public class SystemChannelSO_String : SystemChannelBaseSO
{
    [SerializeField] private SystemChannelBaseSO mainChannel = default;
    [SerializeField] private string value = default;

    protected override void InsertSerializedParameters()
    {
        useParameters = true;

        parameters.InsertRange(0, new List<object>() { value });
    }

    protected override void DoSpecifics()
    {
        if (mainChannel != null)
        {
            mainChannel.SetParameters(Parameters);
            mainChannel.FireEvent();

        }
    }
    public void InsertParameter(string newParameter)
    {
        Debug.Log("parameter inserted " + newParameter);

        useParameters = true;

        parameters.InsertRange(0, new List<object>() { newParameter });
    }
}
