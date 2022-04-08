using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using TMPro;

public class ScreenShotter : MonoBehaviour
{


    public int resWidth = 2550;
    public int resHeight = 3300;
    public TextMeshProUGUI inputText;

    public TextMeshProUGUI infoText;

    public GameObject emailDisplayText;

    string TestEmail = "danieltiandev@gmail.com";

    private TouchScreenKeyboard overlayKeyboard;
    private string currentFileAttachment = null;

    bool inputY;

    private void Start()
    {
        //inputText.text = string.Empty;
        //infoText.text = string.Empty;
        inputY = false;
    }

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                                Application.persistentDataPath,
                                width, height,
                                System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }


    void TakeHighResScreenshot()
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        Camera.main.GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Camera.main.GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        Camera.main.GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenShotName(resWidth, resHeight);

        System.IO.FileInfo file = new System.IO.FileInfo(filename);
        file.Directory.Create();

        System.IO.File.WriteAllBytes(filename, bytes);
        currentFileAttachment = filename;

        Debug.Log(string.Format("Took screenshot to: {0}", filename));
        infoText.text = "we got here";
        SendViaGmail("Mail from ubc hive");
    }


    //https://myaccount.google.com/lesssecureapps
    //By may 30, 2022 we will need to switch to a different provider.
    public void SendViaGmail(string msg)
    {
        string emailTo = "";
        if (inputText == null || !inputText.text.Contains("@") || inputText.text.Length <= 2)
        {
            emailTo = TestEmail;
        }
        else
        {
            emailTo = inputText.text;
        }

        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("ubchive1@gmail.com");

            mail.To.Add(emailTo); //this should be an valid email. 
            
            
            mail.Subject = "SimBrainVR Notes - Screenshot";
            mail.Body = msg;

            if (currentFileAttachment != null) mail.Attachments.Add(new Attachment(currentFileAttachment));

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("ubchive1@gmail.com", "ILoveHoneycomb15!!") as ICredentialsByHost;
            smtp.EnableSsl = true;


            ServicePointManager.ServerCertificateValidationCallback =
                    delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };

            smtp.Send(mail);
        }
        catch(Exception e)
        {
            infoText.text = "The gmail smtp client is no longer supported, please switch to a different client.";
        }

       
        emailDisplayText.SetActive(false);
    }


    private void SendViaOutlook()
    {
        string _sender = "LuffyJoyBoy@outlook.com";
        string _password = "JoyboyLovesMeat69&";

        SmtpClient client = new SmtpClient("smtp-mail.outlook.com");

        client.Port = 587;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        System.Net.NetworkCredential credentials =
            new System.Net.NetworkCredential(_sender, _password);
        client.EnableSsl = true;
        client.Credentials = credentials;

        MailMessage message = new MailMessage(_sender, "daniel_tian2002@hotmail.com");
        message.Subject = "Joy boy";
        message.Body = "Joy boy has awakened for the first time in 800 years";

        if (currentFileAttachment != null) message.Attachments.Add(new Attachment(currentFileAttachment));

        client.Send(message);
        print("done sending");
    }

    private void Update()
    {

        if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            inputY = !inputY;
            if (inputY)
            {
                emailDisplayText.SetActive(true);
                overlayKeyboard = TouchScreenKeyboard.Open(inputText.text, TouchScreenKeyboardType.Default);
            }
    
        }

        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            TakeHighResScreenshot();
        }
        
        if (overlayKeyboard != null)
        {
            //infoText.text = "Input email:";
            inputText.text = overlayKeyboard.text;
            emailDisplayText.SetActive(TouchScreenKeyboard.visible);
        }

    }
}
