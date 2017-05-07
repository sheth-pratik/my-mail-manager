using DataRepository.Model;
using IMAPMailCommunicator.Model;
using MyMailWeb.Infrastructure;
using MyMailWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMailWeb.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult MailContainer()
        {
            MailClientWrapper client = null;
            if (CurrentSession.MailConfiurationId.HasValue)
            {
                client = IMAPMailCommunicator.MailContainerFactory.Instance.GetMailClientByMailConfigurationId(CurrentSession.MailConfiurationId.Value);
            }
            return PartialView(client);
        }

        public PartialViewResult MailList(int index, int page, int count)
        {
            MailMessageListModel listModel = null;
            if (CurrentSession.MailConfiurationId.HasValue)
            {
                var client = IMAPMailCommunicator.MailContainerFactory.Instance.GetMailClientByMailConfigurationId(CurrentSession.MailConfiurationId.Value);
                if (client.IsConnected)
                {
                    listModel = client.GetMessages(index, page, count);
                }
            }
            return PartialView(listModel);
        }

        public PartialViewResult MessageContent(int index, uint id)
        {
            MailMessageWrapper messageWrapper = null;
            if (CurrentSession.MailConfiurationId.HasValue)
            {
                var client = IMAPMailCommunicator.MailContainerFactory.Instance.GetMailClientByMailConfigurationId(CurrentSession.MailConfiurationId.Value);
                if (client.IsConnected)
                {
                    messageWrapper = client.GetMessage(index, id);
                }
            }
            return PartialView(messageWrapper);
        }

        [HttpGet]
        public PartialViewResult MessageCompose()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult MessageCompose(MessageComposeModel model)
        {
            StatusResponse response = new StatusResponse();
            if (CurrentSession.MailConfiurationId.HasValue)
            {
                var client = IMAPMailCommunicator.MailContainerFactory.Instance.GetMailClientByMailConfigurationId(CurrentSession.MailConfiurationId.Value);
                response = client.SendMail(model.ToEmailAddress, model.Subject, model.IsHTMLBody, model.Body);
            }
            return Json(response);
        }

        [HttpPost]
        public JsonResult DeleteSelectedMesseges(int index, List<uint> messageIds)
        {
            StatusResponse response = new StatusResponse();
            if (CurrentSession.MailConfiurationId.HasValue)
            {
                var client = IMAPMailCommunicator.MailContainerFactory.Instance.GetMailClientByMailConfigurationId(CurrentSession.MailConfiurationId.Value);
                response = client.DeleteMails(index, messageIds);
            }
            return Json(response);
        }

    }
}