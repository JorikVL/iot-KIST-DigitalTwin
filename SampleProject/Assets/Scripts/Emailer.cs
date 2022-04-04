using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Emailer : MonoBehaviour
{

    const string kSenderEmailAddress = "APSensors.Zanzibar@gmail.com";
    const string kSenderPassword = "APSensorsZanzibar2022";
    const string kReceiverEmailAddress = "APSensors.Zanzibar@gmail.com";

    void Start() {
        SendAnEmail( "Your sensor is offline" );
    }

    // Method 1: Direct message
    private static void SendAnEmail( string message ) {
        // Create mail
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress( kSenderEmailAddress );
        mail.To.Add( kReceiverEmailAddress );
        mail.Subject = "Sensor Offline";
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
