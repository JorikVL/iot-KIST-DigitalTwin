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

    string kSenderEmailAddress;
    string kSenderPassword;
    const string kReceiverEmailAddress = "APSensors.Zanzibar@gmail.com";

    public void SetEmail( InputField email){
        kSenderEmailAddress = email.text;
        Debug.Log("EmailAddress changed!");
    }

    public void SetPassword( InputField password){
        kSenderPassword = password.text;
        Debug.Log("Password changed!");
    }

    public void SendAnEmail( string message ) {
        // Create mail
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress( kSenderEmailAddress );
        mail.To.Add( kReceiverEmailAddress );
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
                Debug.Log( "Email success!" );
                return true;
            };

        // Send mail to server, print results
        try {
            smtpServer.Send( mail );
        }
        catch ( System.Exception e ) {
            Debug.Log( "Email error: " + e.Message );
        }
        finally {
            Debug.Log( "Email sent!" );
        }
    }
}
