using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class MailTest
    {
        [TestMethod]
        public void ConnectServer()
        {
            Assert.IsTrue(IMAPMailCommunicator.MailCommunicator.ConnectToServer());
        }

        [TestMethod]
        public void SendMail()
        {
            Assert.IsTrue(IMAPMailCommunicator.MailCommunicator.SendMailToServer());
        }
    }
}
