using DataRepository.DataSource;
using DataRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository
{
    public class MailConfigurationRepository : IMailConfigurationRespository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public List<MailConfiguration> GetMailConfigurationsByUserId(string userId)
        {
            List<MailConfiguration> mailConfigurationList = new List<MailConfiguration>();

            using (MyWebMailEntities entities = new MyWebMailEntities())
            {
                try
                {
                    mailConfigurationList = entities.MailConfigurations.Where(e => e.UserId.ToLower() == userId.ToLower()).ToList();
                }
                catch (Exception ex)
                {
                    logger.Error("Error occured while fetching Mail Configurations for userid: " + userId + ". Error Message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                }
            }
            return mailConfigurationList;
        }

        public MailConfiguration GetMailConfigurationsById(int Id)
        {
            using (MyWebMailEntities entities = new MyWebMailEntities())
            {
                try
                {
                    return entities.MailConfigurations.FirstOrDefault(e => e.Id == Id);
                }
                catch (Exception ex)
                {
                    logger.Error("Error occured while fetching Mail Configurations mail id: " + Id+ ". Error Message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                }
            }
            return null;
        }


        public StatusResponse ModifyMailConfiguration(MailConfiguration mailConfiguration)
        {
            bool isSuccess = false;
            string message = string.Empty;
            using (MyWebMailEntities entities = new MyWebMailEntities())
            {
                try
                {
                    MailConfiguration existingMailConfiguraiton = entities.MailConfigurations.FirstOrDefault(m => m.UserId.ToLower() == mailConfiguration.UserId.ToLower());
                    if (existingMailConfiguraiton == null)
                    {
                        existingMailConfiguraiton = new MailConfiguration();
                        existingMailConfiguraiton.UserId = mailConfiguration.UserId;
                        existingMailConfiguraiton.CreatedDate = DateTime.Now;
                        entities.MailConfigurations.Add(existingMailConfiguraiton);

                    }
                    existingMailConfiguraiton.ImapEmailId = mailConfiguration.ImapEmailId;
                    existingMailConfiguraiton.ImapEmailPassword = mailConfiguration.ImapEmailPassword;
                    existingMailConfiguraiton.SMTPEmailId = mailConfiguration.SMTPEmailId;
                    existingMailConfiguraiton.SMTPEmailPassword = mailConfiguration.SMTPEmailPassword;

                    entities.SaveChanges();
                    mailConfiguration.Id = existingMailConfiguraiton.Id;
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    message = "Error occured while modifying Mail Configurations";
                    logger.Error(message + " for userid: " + mailConfiguration.UserId + ". Error Message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                }
            }
            return new StatusResponse() { Status = isSuccess, Message = message, Id = mailConfiguration.Id };
        }
    }
}
