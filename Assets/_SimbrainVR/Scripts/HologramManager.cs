using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HologramManager : MonoBehaviour //dummy patient
{
    public GameObject ui;
    public TextMeshProUGUI textComponent;

    private void Start()
    {
        if (ClinicalLogger.Instance.hasShoneLight)
        {
            textComponent.color = Color.green;
            textComponent.text = "Actions completed: checked pupils";
        }
    }

    public void ToggleUI()
    {
        ui.SetActive(true);
    }

}
