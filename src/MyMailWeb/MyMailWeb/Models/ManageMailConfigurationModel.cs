using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyMailWeb.Models
{
    public class ManageMailConfigurationModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "IMAP Email Address")]
        public string ImapEmailId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "IMAP Email Password")]
        [MinLength(5)]
        public string ImapEmailPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm IMAP Email Password")]
        [Compare("ImapEmailPassword", ErrorMessage = "The IMAP Email Password and Confirm IMAP Email Password do not match.")]
        public string ConfirmImapEmailPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "SMTP Email Address")]
        public string SMTPEmailId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "SMTP Email Password")]
        [MinLength(5)]
        public string SMTPEmailPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm SMTP Email Password")]
        [Compare("SMTPEmailPassword", ErrorMessage = "The SMTP Email Password and Confirm SMTP Email Password do not match.")]
        public string ConfirmSMTPEmailPassword { get; set; }

        [Required]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        [Required]
        [System.Web.Mvc.HiddenInput]
        public int Id { get; set; }

        public static ManageMailConfigurationModel MapObject(DataRepository.DataSource.MailConfiguration mailConfiguration)
        {
            return mailConfiguration != null ? new ManageMailConfigurationModel()
            {
                Id = mailConfiguration.Id,
                UserId = mailConfiguration.UserId,
                ImapEmailId = mailConfiguration.ImapEmailId,
                ImapEmailPassword = mailConfiguration.ImapEmailPassword,
                ConfirmImapEmailPassword = mailConfiguration.ImapEmailPassword,
                SMTPEmailId = mailConfiguration.SMTPEmailId,
                SMTPEmailPassword = mailConfiguration.SMTPEmailPassword,
                ConfirmSMTPEmailPassword = mailConfiguration.SMTPEmailPassword,
            } : new ManageMailConfigurationModel();
        }

        public static DataRepository.DataSource.MailConfiguration MapObject(ManageMailConfigurationModel mailConfiguration)
        {
            return mailConfiguration != null ? new DataRepository.DataSource.MailConfiguration()
            {
                Id = mailConfiguration.Id,
                UserId = mailConfiguration.UserId,
                ImapEmailId = mailConfiguration.ImapEmailId,
                ImapEmailPassword = mailConfiguration.ImapEmailPassword,
                SMTPEmailId = mailConfiguration.SMTPEmailId,
                SMTPEmailPassword = mailConfiguration.SMTPEmailPassword,
            } : new DataRepository.DataSource.MailConfiguration();
        }

    }
}