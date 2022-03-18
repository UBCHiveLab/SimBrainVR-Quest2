using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotter : MonoBehaviour
{
    private TouchScreenKeyboard overlayKeyboard;
    public static string inputText = "";


    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
            print("hello");
        }

    }
}
