using DataRepository.DataSource;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IMAPMailCommunicator.Model
{
    public class MailClientWrapper
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public MailClientWrapper(MailConfiguration configuration)
        {
            this.mailConfiguration = configuration;
        }

        private ImapClient imapClient;
        protected ImapClient ImapClient
        {
            get
            {
                return imapClient;
            }
        }

        private List<string> mailBoxNameList = new List<string>();
        public List<string> MailBoxNameList
        {
            get { return mailBoxNameList; }
        }

        private Dictionary<string, MailBoxWrapper> mailBoxList = new Dictionary<string, MailBoxWrapper>();
        public Dictionary<string, MailBoxWrapper> MailBoxList
        {
            get { return mailBoxList; }
        }

        private string defaultMailBox = string.Empty;
        public string DefaultMailBox
        {
            get { return defaultMailBox; }
        }

        public bool IsConnected
        {
            get
            {
                if (ImapClient != null)
                {
                    return ImapClient.Authed;
                }
                return false;
            }
        }

        public MailConfiguration mailConfiguration { get; set; }

        public string ErrorMessage { get; private set; }

        internal bool Connect()
        {
            if (imapClient != null)
            {
                imapClient.Dispose();
                imapClient = null;
            }
            MailBoxList.Clear();
            MailBoxNameList.Clear();
            try
            {
                imapClient = new ImapClient(MailCommonConfiguration.ImapHostName, MailCommonConfiguration.ImapPort, MailCommonConfiguration.IsUseSSL);
            
                logger.Trace("Attempting to connect to Imap client: " + mailConfiguration.ImapEmailId);
                ImapClient.Login(mailConfiguration.ImapEmailId, mailConfiguration.ImapEmailPassword, AuthMethod.Auto);
                ImapClient.IdleError += ImapClient_IdleError;
            }
            catch (Exception ex)
            {
                logger.Error("Error while connecting to Imap client: " + mailConfiguration.ImapEmailId + ". Error: " + ex.Message);
                ErrorMessage = ex.Message;
            }
            if (IsConnected)
            {
                try
                {
                    var mailBoxNameList = ImapClient.ListMailboxes();
                    logger.Trace("Mail box found: " + mailBoxNameList.Count());
                    MailBoxNameList.AddRange(mailBoxNameList);

                    defaultMailBox = ImapClient.DefaultMailbox;
                    int index = 0;
                    foreach (var item in mailBoxNameList)
                    {
                        var mailBox = ImapClient.GetMailboxInfo(item);
                        if (mailBox != null)
                        {
                            MailBoxList.Add(item, new MailBoxWrapper(item, mailBox, index++));
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    logger.Error("Error while fetching mailbox data: " + mailConfiguration.ImapEmailId + ". Error: " + ex.Message);
                    ErrorMessage = ex.Message;
                }
            }
            return false;
        }

        void ImapClient_IdleError(object sender, IdleErrorEventArgs e)
        {
            logger.Error("IMAP idle error occured: " + mailConfiguration.ImapEmailId + ". Error: " + e.Exception.ToString());
        }

        public MailMessageListModel GetMessages(int index, int page, int count)
        {
            MailMessageListModel model = null;
            if (IsConnected)
            {
                try
                {
                    string mailBoxName = mailBoxNameList[index];
                    if (MailBoxList.ContainsKey(mailBoxName))
                    {
                        var mailBox = MailBoxList[mailBoxName];
                        SynchMailBoxIfRequired(mailBox);
                        int totalMessage = mailBox.MailBoxInfo.Messages;
                        var list = mailBox.GetMessages(page, count, imapClient);
                        model = new MailMessageListModel(index, page, count, totalMessage, list);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error while fetching mailbox index: " + index + " for mail id: " + mailConfiguration.ImapEmailId + ". Error: " + ex.Message);
                }
            }
            return model;
        }

        private void SynchMailBoxIfRequired(MailBoxWrapper mailBox)
        {
            if (!mailBox.IsMailBoxInSynch)
            {
                var idList = ImapClient.Search(SearchCondition.All(), mailBox.Name);
                var unseenIdList = ImapClient.Search(SearchCondition.Unseen(), mailBox.Name);
                mailBox.UpdateIdList(idList, unseenIdList);
            }
        }


        public MailMessageWrapper GetMessage(int index, uint id)
        {
            MailMessageWrapper message = null;
            if (IsConnected)
            {
                try
                {
                    string mailBoxName = mailBoxNameList[index];
                    if (MailBoxList.ContainsKey(mailBoxName))
                    {
                        var mailBox = MailBoxList[mailBoxName];
                        SynchMailBoxIfRequired(mailBox);

                        if (mailBox.MailMessageList.ContainsKey(id))
                        {
                            message = mailBox.MailMessageList[id];
                            if (!message.IsMessageContentInSynch)
                            {
                                var mailMessage = ImapClient.GetMessage(id, FetchOptions.NoAttachments, false, mailBoxName);
                                message.UpdateMailMessage(mailMessage);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error while fetching mailbox index: " + index + " for mail id: " + mailConfiguration.ImapEmailId + ". Error: " + ex.Message);
                }
            }
            return message;
        }

        public DataRepository.Model.StatusResponse DeleteMails(int index, List<uint> mailIdList)
        {
            DataRepository.Model.StatusResponse response = new DataRepository.Model.StatusResponse();
            if (IsConnected)
            {
                try
                {
                    string mailBoxName = mailBoxNameList[index];
                    if (MailBoxList.ContainsKey(mailBoxName))
                    {
                        imapClient.DeleteMessages(mailIdList, mailBoxName);

                        // As no exception resetting current mail box.
                        MailBoxList[mailBoxName].LastFetchedTime = DateTime.MinValue;
                        response.Status = true;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error while deleting mail for index: " + index + " for mail id: " + mailConfiguration.ImapEmailId + ". Error: " + ex.Message);
                    response.Status = false;
                    response.Message = "Error occured whiel deleting mails.";
                }
            }
            return response;
        }

        public DataRepository.Model.StatusResponse SendMail(string toEmailAddress, string subject, bool isHtmlContent, string body)
        {
            DataRepository.Model.StatusResponse response = new DataRepository.Model.StatusResponse();
            try
            {
                using (SmtpClient client = new SmtpClient(MailCommonConfiguration.SMTPHostName, MailCommonConfiguration.SMTPPort))
                {
                    client.EnableSsl = MailCommonConfiguration.IsUseSSL;
                    MailMessage message = new MailMessage(mailConfiguration.SMTPEmailId, toEmailAddress);
                    message.IsBodyHtml = isHtmlContent;
                    message.Subject = subject;
                    message.Body = body;

                    client.Credentials = new System.Net.NetworkCredential(mailConfiguration.SMTPEmailId, mailConfiguration.SMTPEmailPassword);
                    client.Send(message);
                    response.Status = true;

                    // Update all mailbox having title send
                    mailBoxNameList.Where(m => m.ToLower().Contains("send")).ToList().ForEach(f =>
                    {
                        if (MailBoxList.ContainsKey(f))
                        {
                            MailBoxList[f].LastFetchedTime = DateTime.MinValue;
                        }
                    });
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Error occured while sending Mail. Please verify your SMTP Configuration";
                logger.Error("Error while sending mail from : " + mailConfiguration.SMTPEmailId + " to mail id: " + toEmailAddress + ". Error: " + ex.Message);
            }
            return response;
        }

        internal void Disconnect()
        {
            if (IsConnected)
            {
                try
                {
                    ImapClient.Logout();

                }
                catch (Exception ex)
                {
                    logger.Error("Error while logging out from : " + mailConfiguration.SMTPEmailId + ". Error: " + ex.Message);
                }
            }
        }
    }
}
