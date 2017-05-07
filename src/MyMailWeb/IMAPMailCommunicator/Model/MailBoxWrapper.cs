using S22.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IMAPMailCommunicator.Model
{
    public class MailBoxWrapper
    {
        public static readonly TimeSpan MaxAsynchDuration = TimeSpan.FromMinutes(5);

        public MailBoxWrapper(string name, MailboxInfo mailBoxInfo, int index)
        {
            this.Name = name;
            this.MailBoxInfo = mailBoxInfo;
            this.Index = index;
        }

        public int Index { get; private set; }

        public string Name { get; private set; }

        public MailboxInfo MailBoxInfo { get; set; }

        private DateTime lastFetchedTime = DateTime.MinValue;
        public DateTime LastFetchedTime
        {
            get { return lastFetchedTime; }
            set { lastFetchedTime = value; }
        }

        public bool IsMailBoxInSynch
        {
            get
            {
                return (lastFetchedTime.Add(MaxAsynchDuration) >= DateTime.Now);
            }
        }

        private Dictionary<uint, MailMessageWrapper> mailMessageList = new Dictionary<uint, MailMessageWrapper>();
        public Dictionary<uint, MailMessageWrapper> MailMessageList
        {
            get { return mailMessageList; }
        }

        private List<uint> messageIdList = new List<uint>();
        public List<uint> MessageIdList
        {
            get { return messageIdList; }
            set { messageIdList = value; }
        }

        internal void UpdateIdList(IEnumerable<uint> idList, IEnumerable<uint> unseenIdList)
        {
            messageIdList = idList.Reverse().ToList();
            this.unseenIdList = unseenIdList;
            var deletedMailIds = MailMessageList.Keys.Where(m => !idList.Contains(m)).ToList();
            deletedMailIds.ForEach(u => MailMessageList.Remove(u));
            LastFetchedTime = DateTime.Now;
        }

        internal List<MailMessageWrapper> GetMessages(int page, int count, ImapClient client)
        {
            List<MailMessageWrapper> messageWrapperList = new List<MailMessageWrapper>();
            int startCount = page * count;
            var idList = MessageIdList.Skip(startCount).Take(count).ToList();

            var asynchList = new List<uint>();
            foreach (var id in idList)
            {
                if (!MailMessageList.ContainsKey(id) || !MailMessageList[id].IsMessageInSynch)
                {
                    asynchList.Add(id);
                }
            }
            if (asynchList.Count > 0)
            {
                var mailList = client.GetMessages(asynchList, FetchOptions.HeadersOnly, false, Name).ToList();
                for (int index = 0; index < asynchList.Count; index++)
                {
                    uint id = asynchList[index];
                    MailMessageWrapper messageWrapper = null;
                    if (!MailMessageList.ContainsKey(id))
                    {
                        messageWrapper = new MailMessageWrapper(id, mailList[index]);
                        MailMessageList.Add(id, messageWrapper);
                    }
                    else
                    {
                        messageWrapper = MailMessageList[id];
                    }
                    messageWrapper.LastSynchTime = DateTime.Now;
                    if(unseenIdList.Contains(id))
                    {
                        messageWrapper.IsMessageNotRead = true;
                    }
                }
            }
            foreach (var item in idList)
            {
                if (MailMessageList.ContainsKey(item))
                {
                    messageWrapperList.Add(MailMessageList[item]);
                }
            }
            return messageWrapperList;
        }

        public IEnumerable<uint> unseenIdList { get; set; }
    }
}
