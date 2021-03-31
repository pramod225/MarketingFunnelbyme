using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace YuktiSolutions.MarketingFunnel.Models
{
    public static class EmailManager
    {
        private static MailAddress DefaultSender = new MailAddress("info@yuktisolutions.com", "CRM");

        /// <summary>
        /// Alerts should be sent for critical actions. For example, new user 
        /// creation. Data deletion etc.
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        public static void SendAlert(String Subject, String Body)
        {
            SendEmail(DefaultSender, new MailAddress("rakesh.verma@yuktisolutions.com","Rakesh Verma"), "Alert : " + Subject, Body, MailPriority.High);
            SendEmail(DefaultSender, new MailAddress("tarun.verma@yuktisolutions.com","Tarun Verma"), "Alert : " + Subject, Body, MailPriority.High);
        }

        public static void SendInfo(MailAddress To, String Subject, String Body)
        {
            SendEmail(DefaultSender, To, Subject , Body, MailPriority.High);
        }


        public static void SendEmail(MailAddress From, MailAddress To, String Subject, String Message, MailPriority Priority = MailPriority.Normal)
        {

            var msg = new MailMessage(From, To);
            msg.Subject = Subject;

            msg.Body = Message;

            msg.IsBodyHtml = true;
            msg.Priority = Priority;
            SmtpClient client;

            //HOSTED : ONLINE
            client = new SmtpClient("dedrelay.secureserver.net", 25);


            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    client.Send(msg);
                }
                catch (Exception)
                {
                    //Emails will not be relayed.
                }
            });
        }
    }
}