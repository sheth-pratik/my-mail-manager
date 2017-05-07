using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyMailWeb.Infrastructure;

namespace MyMailWeb.Controllers
{
    public class HomeController : Controller
    {
        DataRepository.IMailConfigurationRespository repository;

        public HomeController()
        {
            repository = new DataRepository.MailConfigurationRepository();
        }

        public ActionResult Index()
        {
            SetSessionData();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region Helper Methods
        private void SetSessionData()
        {
            string userId = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(CurrentSession.UserId) && !CurrentSession.MailConfiurationId.HasValue)
            {
                CurrentSession.UserId = userId;
                var mailConfiguration = repository.GetMailConfigurationsByUserId(userId).FirstOrDefault();
                if (mailConfiguration != null)
                {
                    CurrentSession.MailConfiurationId = mailConfiguration.Id;
                }

            }
        }
        #endregion
    }
}