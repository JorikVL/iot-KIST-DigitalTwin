using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class Emailer : MonoBehaviour
{

    private string kSenderEmailAddress;
    private string kSenderPassword;
    private string kReceiverEmailAddress;

    public Notification notification;
    public InputField senderEmail;
    public InputField senderPassword;
    public InputField receiverEmail;

    private void Start() {
        SetEmail();
    }

    public void SetEmail() {
        kSenderEmailAddress = senderEmail.text;
        kSenderPassword = senderPassword.text;
        kReceiverEmailAddress = receiverEmail.text;
        Debug.Log("Email Set");
    }

    public void SendAnEmail(string message) {
        // Create mail
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(kSenderEmailAddress);
        mail.To.Add(kReceiverEmailAddress);
        mail.Subject = "Sensor Alert";
        mail.Body = message;

        // Setup server 
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(
            kSenderEmailAddress, kSenderPassword ) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate ( object s, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors ) {
                Debug.Log("Email success!");
                return true;
            };

        // Send mail to server, print results
        try {
            smtpServer.Send(mail);
        }
        catch ( System.Exception e ) {
            Debug.Log("Email error: " + e.Message);
            notification.Notify("Email error: " + e.Message);
        }
        finally {
            Debug.Log("Email sent!");
        }
    }
}
