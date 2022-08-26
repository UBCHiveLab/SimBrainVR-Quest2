using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=tEsuLTpz_DU&ab_channel=Tarodev
//@author: daniel tian
public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    [SerializeField] private AudioSource _voiceSource, _musicSource;

    public AudioClip patientWhatHappened, patientMedicalHistory, patientMedication, patientAllergies, patientDrugs, patientFamily, patientFather;
    public AudioClip doctorIntro, doctorWhatHaveYouFound, doctorDone;
    public AudioClip patientDontKnow, patientSureGoAhead;
    public AudioClip ericIntroClip, ericHello;
    public AudioClip patientAlright, patientOK;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (!_voiceSource.isPlaying)
        {
            _voiceSource.PlayOneShot(clip);
        }
        
    }
}
