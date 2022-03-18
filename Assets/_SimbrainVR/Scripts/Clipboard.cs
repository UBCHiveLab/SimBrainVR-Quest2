using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clipboard : MonoBehaviour
{

    public static Clipboard Instance;

    public Text title;
    public Text info;
    public Text progressText;

    public GameObject patientInfo, medicalHistory, medication, allergies, drugHistory, familyHistory;

    int progress = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ModifyClipboard(int selection)
    {

        switch (selection)
        {
            case 1:
                title.text = "Patient brief";
                info.text = "Patient: Julia Taylor, 64 years old female\n\nHistory of hypertension, dyslipidemia, type 2 diabetes\nPresenting with severe headache and double vision.\n\nVital signs are afebrile. BP 180 and HR 90\nRR16 SpO2 97%";
                if (!patientInfo.activeSelf) progress++;
                patientInfo.SetActive(true);
                break;
            case 2:
                title.text = "Medical History";
                info.text = "I have hiypertension, dyslipidemia,  and type 2 diabetes";
                if (!medicalHistory.activeSelf) progress++;
                medicalHistory.SetActive(true);
                break;

            case 3:
                title.text = "Medication";
                info.text = "Amlodipine 10 mg PO daily, atorvastatin 20 mg PO daily, metformin 1000 mg PO BID";
                if (!medication.activeSelf) progress++;
                medication.SetActive(true);
                break;
            case 4:
                title.text = "Allergies";
                info.text = "Taking penicillin causes patient to breakout in rashes all over her body";
                if (!allergies.activeSelf) progress++;
                allergies.SetActive(true);
                break;
            case 5:
                title.text = "Drug History";
                info.text = "Smoker for 20 years\nSocial drinker of roughly 2 times / week";
                if (!drugHistory.activeSelf) progress++;
                drugHistory.SetActive(true);
                break;

            case 6:
                title.text = "Family History";
                info.text = "Father passed away from aneurysm rupture in his 60s";
                if (!familyHistory.activeSelf) progress++;
                familyHistory.SetActive(true);
                break;
        }


        progressText.text = progress + "/6";
    }




}
