using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMailWeb.Infrastructure
{
    public static class CurrentSession
    {
        private const string USER_ID = "USER_ID";
        private const string MAIL_CONFIGURATION_ID = "MAIL_CONFIGURATION_ID";

        public static String UserId
        {
            get
            {
                if (HttpContext.Current.Session[USER_ID] != null)
                {
                    return HttpContext.Current.Session[USER_ID].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session[USER_ID] = value;
            }
        }

        public static int? MailConfiurationId
        {
            get
            {
                var value = HttpContext.Current.Session[MAIL_CONFIGURATION_ID];
                int id;
                if (value != null && int.TryParse(value.ToString(), out id) && id > 0)
                {
                    return id;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[MAIL_CONFIGURATION_ID] = value;
            }
        }
    }
}