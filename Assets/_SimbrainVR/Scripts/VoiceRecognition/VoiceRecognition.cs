using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections;
using Microsoft.CognitiveServices.Speech.Audio;
using System.IO;
using UnityEngine.Android;
using UnityEngine.Events;

public class VoiceRecognition : MonoBehaviour
{
    [SerializeField] private bool activateOnEnabled = true;
    [SerializeField] private bool deactivateOnDisabled = true;
    [SerializeField] private AudioSource audioSource = default;

    public UnityEvent<string> OnRecognized = default;

    private bool hasRecognizedVoice = true;
    private bool micPermissionGranted = false;
    SpeechRecognizer recognizer;
    SpeechConfig config;
    AudioConfig audioInput;
    PushAudioInputStream pushStream;

    private object threadLocker = new object();
    private bool recognitionStarted = false;
    private string message;
    int lastSample = 0;


    // Required to manifest microphone permission, cf.
    // https://docs.unity3d.com/Manual/android-manifest.html
    private Microphone mic;

    private void OnDestroy()
    {
        Deactivate();
    }

    private void OnApplicationQuit()
    {
        Deactivate();
    }

    private void OnDisable()
    {
        if (deactivateOnDisabled)
            Deactivate();


    }

    private void OnEnable()
    {
        if (activateOnEnabled)
        {
            Activate();
        }
    }

    private void Activate()
    {
        hasRecognizedVoice = false;

        // Request to use the microphone, cf.
        // https://docs.unity3d.com/Manual/android-RequestingPermissions.html
        message = "Waiting for mic permission";
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }

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
        //audioSource = GameObject.Find("MyAudioSource").GetComponent<AudioSource>();

        StartRecognitionAsync();

    }
    private async void StartRecognitionAsync()
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

    private void Deactivate()
    {
        recognizer.Recognizing -= RecognizingHandler;
        recognizer.Recognized -= RecognizedHandler;
        recognizer.Canceled -= CanceledHandler;
        pushStream.Close();
        recognizer.Dispose();
    }

    void FixedUpdate()
    {
        if (!micPermissionGranted && Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            micPermissionGranted = true;
            message = "The AI is listening...";
        }

        lock (threadLocker)
        {
            if (hasRecognizedVoice)
            {
                OnRecognized?.Invoke(message);
                hasRecognizedVoice = false;
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
            hasRecognizedVoice = true;
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
}
