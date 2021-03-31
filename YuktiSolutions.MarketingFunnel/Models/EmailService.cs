using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;


namespace YuktiSolutions.MarketingFunnel.Models
{
    public class EmailService
    {
        /// <summary>
        /// sender
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Receiver
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Subject to be deliver in the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Html content to be deliver in the email.
        /// </summary>
        public string Body { get; set; }
    }

    /// <summary>
    /// Define email contents
    /// </summary>
    public class EmailContent : EmailService
    {

        public List<string> Recipients { get; set; }

        /// <summary>
        /// User whose email configuration will be used to send email.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Define the order in which emails will be sent.
        /// </summary>
        public int Priority { get; set; }

        public EmailContent()
        {
            this.Recipients = new List<string>();
        }
    }

    public static class EmailHelper
    {
        /// <summary>
        /// Send email to the specified person.
        /// </summary>
        /// <param name="content">email details</param>
        /// <param name="credential">credentails to be used to send email</param>
        /// <returns></returns>
        public static Notify SendEmail(EmailService content, NetworkDetail credential)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(content.Destination);
                mail.From = new MailAddress(content.From);
                mail.Subject = content.Subject;
                mail.Body = content.Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = credential.SmtpServer; // "smtp.gmail.com";
                if (credential.Port > 0)
                    smtp.Port = credential.Port; // 587;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.EnableSsl = credential.EnableSSL;
                smtp.Send(mail);

                return new Notify() { Message = "Email successfully sent to " + content.Destination, IsSent = true };
            }
            catch (Exception ex)
            {
                return new Notify() { Message = "Email to : " + content.Destination + " failed due to : " + ex.Message, IsSent = false };
            }
        }
    }

    /// <summary>
    /// Email Notify unit.
    /// </summary>
    public class Notify
    {
        /// <summary>
        /// Whether email is sent or not
        /// </summary>
        public bool IsSent { get; set; }

        /// <summary>
        /// Email sent description
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Network credentail details to be used to sent email.
    /// </summary>
    public class NetworkDetail : NetworkCredential
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public NetworkDetail(ApiEmailConfiguration config, string pwd) : base(config.LoginName, pwd)
        {
            EnableSSL = config.EnableSSL;
            SmtpServer = config.smtpServer;
            if (config.PortNumber.HasValue)
                Port = config.PortNumber.Value;
            UserName = config.LoginName;
            Password = pwd;
        }
    }

    public class ApiEmailConfiguration
    {
        public bool EnableSSL { get; set; }
        public string smtpServer { get; set; }
        public int? PortNumber { get; set; }
        public string LoginName { get; set; }

    }
}