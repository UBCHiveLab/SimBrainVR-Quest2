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

public class VoiceRecognitionClinic : MonoBehaviour
{

    #region singleton
    private static VoiceRecognitionClinic _instance;

    public static VoiceRecognitionClinic Instance { get { return _instance; } }


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


    public Text outputText;
    public PatientSpeakingController patientSpeakingController;
    public GameObject patientHumanoid, patientExamMode;
    public bool hasStartedAskingQuestions;

    private float talkingStartTime;
    private bool hasSaidSomething = true;
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
    
    
    private void RecognizedHandler(object sender, SpeechRecognitionEventArgs e) 
    {
        lock (threadLocker)
        {
            message = e.Result.Text;
            //outputText.text = "RecogniZED: " + e.Result.Text;
            Debug.Log("RecognizedHandler: " + message);
            hasSaidSomething = true;
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

        talkingStartTime = Time.time;

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


            if (outputText != null)
            {
                //outputText.text = message;

                if(message.Contains("mind palace"))
                {
                    //SceneManager.LoadScene(3);
                    var sceneLoading = GameObject.Find("SceneLoader").GetComponent<SceneLoading>();
                    if (sceneLoading != null)
                    {
                        sceneLoading.TransitionToMindPalace();
                    }

                }else if (message.Contains("flashlight"))
                {

                    var flashlight = GameObject.Find("MedicalLight");
                    if (flashlight != null)
                    {
                        flashlight.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
                    }
 
                }/*else if(message.Contains("light on"))
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

                }*/
                else if (message.Contains("eric"))
                {

                    if(message.Contains("off") && message.Contains("light"))
                    {
                        var eric = GameObject.Find("Eric_Nurse");
                        if (eric != null) eric.GetComponent<EricNurse>().ToggleLight(false);
                    }
                    else if (message.Contains("on") && message.Contains("light"))
                    {
                        var eric = GameObject.Find("Eric_Nurse");
                        if (eric != null) eric.GetComponent<EricNurse>().ToggleLight(true);
                    }

                }else if (message.Contains("extend")) //can you extend your arms?
                {
                    MotorTest.Instance.ToggleRaiseArms(true);
                }else if (message.Contains("put down")) //put down your arms
                {
                    MotorTest.Instance.ToggleRaiseArms(false);
                }
                else if (message.Contains("done"))
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.doctorWhatHaveYouFound);
                    message = "";

                }else if(message.Contains("that is all"))
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.doctorDone);
                    patientHumanoid.SetActive(true);
                    patientExamMode.SetActive(false);
                }
                else if(message.Contains("start exam"))
                {
                    patientHumanoid.SetActive(false);
                    patientExamMode.SetActive(true);
                }

                if (patientSpeakingController.gameObject.activeSelf)
                {
                    if (patientSpeakingController.isMakingEyeContact)
                    {
                        if (!patientHumanoid.activeSelf) return;
                        //if (!message.Contains("julia")) return;

                        if (message.Contains("what happen") || message.Contains("tell me")) //this part is hardcoded
                        {
                            patientSpeakingController.Speak(PatientDialogueOption.WhatHappened);
                            Clipboard.Instance.ModifyClipboard(1);
                        }
                        else if (message.Contains("medical history"))
                        {
                            patientSpeakingController.Speak(PatientDialogueOption.MedicalHistory);
                            Clipboard.Instance.ModifyClipboard(2);
                        }
                        else if (message.Contains("medication") || message.Contains("medicine"))
                        {
                            patientSpeakingController.Speak(PatientDialogueOption.Medication);
                            Clipboard.Instance.ModifyClipboard(3);
                        }
                        else if (message.Contains("allergies") || message.Contains("allergy"))
                        {
                            patientSpeakingController.Speak(PatientDialogueOption.Allergies);
                            Clipboard.Instance.ModifyClipboard(4);
                        }
                        else if (message.Contains("drink") || message.Contains("smoke") || message.Contains("drug"))
                        {
                            patientSpeakingController.Speak(PatientDialogueOption.DrinkSmokeDrugs);
                            Clipboard.Instance.ModifyClipboard(5);
                        }
                        else if (message.Contains("family history"))
                        {
                            patientSpeakingController.Speak(PatientDialogueOption.FamilyHistory);
                            Clipboard.Instance.ModifyClipboard(6);
                        }
                        else if (message.Contains("family member") || message.Contains("live with"))
                        {
                            patientSpeakingController.Speak(PatientDialogueOption.FamilyMember);
                        }
                        else if (hasSaidSomething)
                        {
                            if (hasStartedAskingQuestions && (Time.time - talkingStartTime >= 5.5f))
                            {
                                hasSaidSomething = false;
                                talkingStartTime = Time.time;
                                if (message.Length > 0)
                                    patientSpeakingController.Speak(PatientDialogueOption.DontKnow);
                            }

                        }

                        

                        message = "";
                    }
                }
                
                //message = "";
            }
        
            
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

    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.F10))
        {
            foreach (var device in Microphone.devices)
            {
                Debug.Log("Name: " + device);
            }
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
