using IMAPMailCommunicator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPMailCommunicator
{
    public class MailContainerFactory
    {
        DataRepository.IMailConfigurationRespository reporitory;
        private MailContainerFactory()
        {
            reporitory = new DataRepository.MailConfigurationRepository();
        }

        private static MailContainerFactory instance = new MailContainerFactory();
        public static MailContainerFactory Instance
        {
            get { return MailContainerFactory.instance; }
        }

        private Dictionary<int, MailClientWrapper> mailClientList = new Dictionary<int, MailClientWrapper>();
        public Dictionary<int, MailClientWrapper> MailClientList
        {
            get { return mailClientList; }
        }

        public MailClientWrapper GetMailClientByMailConfigurationId(int Id)
        {
            lock (mailClientList)
            {
                if (!mailClientList.ContainsKey(Id))
                {
                    var mailConfiguration = reporitory.GetMailConfigurationsById(Id);
                    if (mailConfiguration != null)
                    {
                        MailClientWrapper client = new MailClientWrapper(mailConfiguration);
                        bool isSuccess = client.Connect();
                        mailClientList.Add(Id, client);
                    }
                }
                if (mailClientList.ContainsKey(Id))
                {
                    if (!mailClientList[Id].IsConnected)
                    {
                        mailClientList[Id].Connect();
                    }
                    return mailClientList[Id];
                }
            }
            return null;
        }

        public void RemoveMailClient(int Id)
        {
            if (mailClientList.ContainsKey(Id))
            {
                mailClientList[Id].Disconnect();
                mailClientList.Remove(Id);
            }
        }
    }
}
