using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RogoDigital.Lipsync;

public class LipSyncControl : MonoBehaviour
{

    public LipSync lipSyncController;
    public LipSyncData lipSyncDataFile, shorterDataFile;
    public AudioSource whatSound;

    public Behaviour eyeControl;

    public void PlayWhatHappened()
    {
        lipSyncController.Play(lipSyncDataFile);
        SoundManager.Instance.PlaySound(SoundManager.Instance.patientWhatHappened);
        eyeControl.enabled = false;


    }

    public void MoveLips()
    {
        lipSyncController.Play(shorterDataFile);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))PlayWhatHappened();
    }
}
