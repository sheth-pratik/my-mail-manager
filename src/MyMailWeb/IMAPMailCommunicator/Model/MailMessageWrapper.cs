using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using S22.Imap;

namespace IMAPMailCommunicator.Model
{
    public class MailMessageWrapper
    {
        public MailMessageWrapper(uint uid, MailMessage message)
        {
            this.uid = uid;
            this.mailMessage = message;
            this.date = message.Date();
        }

        private MailMessage mailMessage;
        public MailMessage MailMessage
        {
            get { return mailMessage; }
        }

        private uint uid;
        public uint Uid
        {
            get { return uid; }
        }

        private DateTime lastSynchTime = DateTime.MinValue;
        public DateTime LastSynchTime
        {
            get { return lastSynchTime; }
            set { lastSynchTime = value; }
        }

        private DateTime? date;
        public DateTime? Date
        {
            get { return date; }
            set { date = value; }
        }

        public bool IsMessageNotRead
        {
            get;
            set;
        }

        public string DateString
        {
            get
            {
                if (date.HasValue)
                {
                    if (date.Value.Date == DateTime.Today)
                    {
                        return Date.Value.ToString("t");
                    }
                    else
                    {
                        return Date.Value.ToString("m");
                    }
                }
                return string.Empty;
            }
        }

        public bool IsMessageInSynch
        {
            get
            {
                return (LastSynchTime.Add(MailBoxWrapper.MaxAsynchDuration) >= DateTime.Now);
            }
        }

        public DateTime lastMessageReadTime = DateTime.MinValue;

        public bool IsMessageContentInSynch
        {
            get
            {
                return (lastMessageReadTime.Add(MailBoxWrapper.MaxAsynchDuration) >= DateTime.Now);
            }
        }

        internal void UpdateMailMessage(MailMessage mailMessage)
        {
            if (mailMessage != null)
            {
                this.mailMessage = mailMessage;
                this.lastMessageReadTime = DateTime.Now;
            }
        }
    }
}
