using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPMailCommunicator.Model
{
    public static class MailCommonConfiguration
    {
        static MailCommonConfiguration()
        {
            ImapHostName = ConfigurationManager.AppSettings["ImapHostName"].ToString();
            ImapPort = Convert.ToInt32(ConfigurationManager.AppSettings["ImapPort"]);

            SMTPHostName = ConfigurationManager.AppSettings["SMTPHostName"].ToString();
            SMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

            IsUseSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsUseSSL"]);

        }

        public static string ImapHostName { get; set; }

        public static int ImapPort { get; set; }

        public static string SMTPHostName { get; set; }

        public static int SMTPPort { get; set; }

        public static bool IsUseSSL { get; set; }

    }
}
