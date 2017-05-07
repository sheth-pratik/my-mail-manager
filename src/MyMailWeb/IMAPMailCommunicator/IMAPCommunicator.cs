using S22.Imap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IMAPMailCommunicator
{
    // This is test class created for testing S22 Imap library
    public class MailCommunicator
    {
        public static bool ConnectToServer()
        {
            try
            {
                using (ImapClient client = new ImapClient("imap.gmail.com", 993, "your email", "your password", AuthMethod.Login, true))
                {
                    var info = client.GetMailboxInfo();
                    var list = client.ListMailboxes();
                    MailMessage mes = client.GetMessage(1);
                    client.Login(string.Empty, string.Empty, AuthMethod.Auto);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error while connection to IMAP: " + ex.Message);
            }
            return false;
        }

        public static bool SendMailToServer()
        {
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    MailMessage message = new MailMessage("email id", "email id");
                    message.IsBodyHtml = false;
                    message.Subject = "This is test mail Subject";
                    message.Body = "Hi, This is test mail body";

                    client.Credentials = new System.Net.NetworkCredential("your email id", "your password");
                    client.Send(message);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error while sending mail: " + ex.Message);
            }
            return false;
        }
    }
}
