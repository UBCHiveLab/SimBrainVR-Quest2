using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections;
using Microsoft.CognitiveServices.Speech.Audio;
using System.IO;
using UnityEngine.SceneManagement;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
#if PLATFORM_IOS
using UnityEngine.iOS;
using System.Collections;
#endif

public class VoiceRecg : MonoBehaviour
{

    #region singleton
    private static VoiceRecg _instance;

    public static VoiceRecg Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion


    Vector3 posA;
    Vector3 posB;

    private bool micPermissionGranted = false;
    //public Button recoButton;
    SpeechRecognizer recognizer;
    SpeechConfig config;
    AudioConfig audioInput;
    PushAudioInputStream pushStream;

    private object threadLocker = new object();
    private bool recognitionStarted = false;
    private string message;
    int lastSample = 0;
    AudioSource audioSource;



#if PLATFORM_ANDROID || PLATFORM_IOS
    // Required to manifest microphone permission, cf.
    // https://docs.unity3d.com/Manual/android-manifest.html
    private Microphone mic;
#endif

    private byte[] ConvertAudioClipDataToInt16ByteArray(float[] data)
    {
        MemoryStream dataStream = new MemoryStream();
        int x = sizeof(Int16);
        Int16 maxValue = Int16.MaxValue;
        int i = 0;
        while (i < data.Length)
        {
            dataStream.Write(BitConverter.GetBytes(Convert.ToInt16(data[i] * maxValue)), 0, x);
            ++i;
        }
        byte[] bytes = dataStream.ToArray();
        dataStream.Dispose();
        return bytes;
    }

    private void RecognizingHandler(object sender, SpeechRecognitionEventArgs e)
    {
        lock (threadLocker)
        {
            message = e.Result.Text;
            //outputText.text = "RecognizING: " + e.Result.Text;
            Debug.Log("RecognizingHandler: " + message);
        }
    }
    
    
    private void RecognizedHandler(object sender, SpeechRecognitionEventArgs e) //for i dont know response, boolean here? only when person has talked! otherwise no.
    {
        lock (threadLocker)
        {
            message = e.Result.Text;
            //outputText.text = "RecogniZED: " + e.Result.Text;
            Debug.Log("RecognizedHandler: " + message);
            //hasSaidSomething = true;
        }
    }

    private void CanceledHandler(object sender, SpeechRecognitionCanceledEventArgs e)
    {
        lock (threadLocker)
        {
            message = e.ErrorDetails.ToString();
            //outputText.text = "Cancelled: : " + e.Result.Text;
            Debug.Log("CanceledHandler: " + message);
        }
    }

    public async void ButtonClick()
    {
        if (recognitionStarted)
        {
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(true);

            if (Microphone.IsRecording(Microphone.devices[0]))
            {
                Debug.Log("Microphone.End: " + Microphone.devices[0]);
                Microphone.End(null);
                lastSample = 0;
            }

            lock (threadLocker)
            {
                recognitionStarted = false;
                Debug.Log("RecognitionStarted: " + recognitionStarted.ToString());
            }
        }
        else
        {
            if (!Microphone.IsRecording(Microphone.devices[0]))
            {
                Debug.Log("Microphone.Start: " + Microphone.devices[0]);
                audioSource.clip = Microphone.Start(Microphone.devices[0], true, 200, 16000);
                Debug.Log("audioSource.clip channels: " + audioSource.clip.channels);
                Debug.Log("audioSource.clip frequency: " + audioSource.clip.frequency);
            }

            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            lock (threadLocker)
            {
                recognitionStarted = true;
                Debug.Log("RecognitionStarted: " + recognitionStarted.ToString());
            }
        }
    }


    void Start()
    {

        
        
        // Continue with normal initialization, Text and Button objects are present.
#if PLATFORM_ANDROID
        // Request to use the microphone, cf.
        // https://docs.unity3d.com/Manual/android-RequestingPermissions.html
        message = "Waiting for mic permission";
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#elif PLATFORM_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Application.RequestUserAuthorization(UserAuthorization.Microphone);
        }
#else
        micPermissionGranted = true;
        message = "Click button to recognize speech";
#endif
        config = SpeechConfig.FromSubscription("2849c9beadec47c59defe817c4478fe5", "westus");
        pushStream = AudioInputStream.CreatePushStream();
        audioInput = AudioConfig.FromStreamInput(pushStream);
        recognizer = new SpeechRecognizer(config, audioInput);
        recognizer.Recognizing += RecognizingHandler;
        recognizer.Recognized += RecognizedHandler;
        recognizer.Canceled += CanceledHandler;

        //recoButton.onClick.AddListener(ButtonClick);
        foreach (var device in Microphone.devices)
        {
            Debug.Log("DeviceName: " + device);
        }
        audioSource = GameObject.Find("MyAudioSource").GetComponent<AudioSource>();
        


        ButtonClick();
    }

    void Disable()
    {
        recognizer.Recognizing -= RecognizingHandler;
        recognizer.Recognized -= RecognizedHandler;
        recognizer.Canceled -= CanceledHandler;
        pushStream.Close();
        recognizer.Dispose();
    }

    void FixedUpdate()
    {
#if PLATFORM_ANDROID
        if (!micPermissionGranted && Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            micPermissionGranted = true;
            message = "The AI is listening...";
        }
#elif PLATFORM_IOS
        if (!micPermissionGranted && Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            micPermissionGranted = true;
            message = "Click button to recognize speech";
        }
#endif
        lock (threadLocker)
        {

            //if (recoButton != null) recoButton.interactable = micPermissionGranted;
            /*
            if (outputText != null)
            {
                outputText.text = message;
                if(message.Contains("mind palace"))
                {
                     SceneManager.LoadScene(1);

                }else if (message.Contains("clinic"))
                {
                    SceneManager.LoadScene(0);

                }else if (message.Contains("flashlight"))
                {

                    var flashlight = GameObject.Find("MedicalLight");
                    if (flashlight != null)
                    {
                        //Rigidbody flashlightRB = flashlight.GetComponent<Rigidbody>();
                        //flashlightRB.isKinematic = true;
                        //flashlightRB.useGravity = false;

                        flashlight.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
                    }
 
                }else if(message.Contains("light on"))
                {
                    var light = GameObject.Find("MainLight");
                    if (light != null)
                    {
                        light.transform.GetChild(0).gameObject.SetActive(true);
                    }

                }else if(message.Contains("light off"))
                {
                    var light = GameObject.Find("MainLight");
                    if (light != null)
                    {
                        light.transform.GetChild(0).gameObject.SetActive(false);
                    }

                }
                else if(message.Contains("brain"))
                {
                    if (SceneManager.GetActiveScene().buildIndex == 1)
                    {
                        brain.SetActive(true);
                        brain1.SetActive(true);
                        brain2.SetActive(true);
                        brain3.SetActive(true);
                    }

                }
                else if (message.Contains("eric"))
                {
                    var eric = GameObject.Find("Eric_Nurse");
                    if (eric != null)
                    {
                        eric.GetComponent<EricNurse>().Wave();
                    }
                }
                
                if ( (Camera.main.transform.localEulerAngles.y > 165f && Camera.main.transform.localEulerAngles.y <= 185f) ||
                    (Camera.main.transform.localEulerAngles.y < -181f && Camera.main.transform.localEulerAngles.y > -165f))
                {
                    if (SceneManager.GetActiveScene().buildIndex == 1) return;
                    if (!PatientHumanoid.activeSelf) return;

                    if (message.Contains("what happen") || message.Contains("tell me"))
                    {
                        patientSpeakingController.Speak(PatientDialogueOption.WhatHappened);
                    }
                    else if (message.Contains("medical history"))
                    {
                        patientSpeakingController.Speak(PatientDialogueOption.MedicalHistory);
                    }
                    else if (message.Contains("medication") || message.Contains("medicine"))
                    {
                        patientSpeakingController.Speak(PatientDialogueOption.Medication);
                    }
                    else if (message.Contains("allergies") || message.Contains("allergy"))
                    {
                        patientSpeakingController.Speak(PatientDialogueOption.Allergies);
                    }
                    else if (message.Contains("drink") || message.Contains("smoke") || message.Contains("drug"))
                    {
                        patientSpeakingController.Speak(PatientDialogueOption.DrinkSmokeDrugs);
                    }
                    else if (message.Contains("family history"))
                    {
                        patientSpeakingController.Speak(PatientDialogueOption.FamilyHistory);
                    }
                    else if (message.Contains("family member") || message.Contains("live with"))
                    {
                        patientSpeakingController.Speak(PatientDialogueOption.FamilyMember);
                    }
                    else if(hasSaidSomething)
                    {
                        hasSaidSomething = false;
                        patientSpeakingController.Speak(PatientDialogueOption.DontKnow);
                    }
                }
                message = "";
            }
        
        
            */
        }

        if (Microphone.IsRecording(Microphone.devices[0]) && recognitionStarted == true)
        {
            
            int pos = Microphone.GetPosition(Microphone.devices[0]);
            int diff = pos - lastSample;

            if (diff > 0)
            {
                float[] samples = new float[diff * audioSource.clip.channels];
                audioSource.clip.GetData(samples, lastSample);
                byte[] ba = ConvertAudioClipDataToInt16ByteArray(samples);
                if (ba.Length != 0)
                {
                    //Debug.Log("pushStream.Write pos:" + Microphone.GetPosition(Microphone.devices[0]).ToString() + " length: " + ba.Length.ToString());
                    pushStream.Write(ba);
                }
            }
            lastSample = pos;
        }
        else if (!Microphone.IsRecording(Microphone.devices[0]) && recognitionStarted == false)
        {
            
        }
    }


    private void OnDestroy()
    {
        Disable();
    }

    private void OnApplicationQuit()
    {
        Disable();
    }

}
