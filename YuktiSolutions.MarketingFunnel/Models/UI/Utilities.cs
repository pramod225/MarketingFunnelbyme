using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using YuktiSolutions.MarketingFunnel.Models.Database;

namespace YuktiSolutions.MarketingFunnel.Models.UI
{
    public class Utilities
    {
        public static List<SelectListItem> GetCountryList()
        {

            var elements = new List<SelectListItem>
                {
                    new SelectListItem() {Value = "IN", Selected = false, Text = "India"},
                    new SelectListItem() {Value = "DE", Selected = false, Text = "Germany"},
                    new SelectListItem() {Value = "DK", Selected = false, Text = "Denmark"},
                    new SelectListItem() {Value = "NL", Selected = false, Text = "Netherlands"},
                    new SelectListItem() {Value = "CH", Selected = false, Text = "Switzerland"},
                    new SelectListItem() {Value = "FR", Selected = false, Text = "France"},
                    new SelectListItem() {Value = "GB", Selected = false, Text = "UK"},
                    new SelectListItem() {Value = "US", Selected = false, Text = "USA"},
                    new SelectListItem() {Value = "AU", Selected = false, Text = "Australia"},
                    new SelectListItem() {Value = "JP", Selected = false, Text = "Japan"},
                    new SelectListItem() {Value = "NZ", Selected = false, Text = "New Zealand"},
                    new SelectListItem() {Value = "SG", Selected = false, Text = "Singapore"},
                };

            return elements;
        }      
        public static void SendEmail(string to, string subject, string body, string fullName = "")
        {
            //uncomment this line after testing
            var receipient = string.IsNullOrEmpty(fullName) ? "Member" : fullName;
           
                //tv : get email confirmations
                var displayName = System.Configuration.ConfigurationManager.AppSettings.Get("DISPLAY_NAME");
               
                var from = "noreply@taskhours.com";
                MailAddress sender = new MailAddress(from, displayName, System.Text.Encoding.UTF8);
                MailAddress mainReceiver = new MailAddress(to);

                var msg = new MailMessage(sender, mainReceiver);
                //msg.Bcc.Add("rakesh.verma@yuktisolutions.com");
                //msg.Bcc.Add("tarun.verma@yuktisolutions.com");
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;

                SmtpClient client;
               
                //HOSTED : ONLINE
                client = new SmtpClient("dedrelay.secureserver.net", 25);
            
                System.Threading.Tasks.Task.Run(() => {
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


        public static TimeZoneInfo GetLocationTimezone(String LocationTimezone)
        {
            TimeZoneInfo locationTimezone = TimeZoneInfo.FindSystemTimeZoneById(LocationTimezone);
            return locationTimezone;
        }
        

        /// <summary>
        /// Converts hosting server specific time to locatioin specific time.
        /// </summary>
        /// <param name="Timezone"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetServerToLocationDateTime(String Timezone, DateTime? datetime)
        {
            if (datetime.HasValue == false) datetime = DateTime.Now;

            TimeZoneInfo serverTimezone = TimeZoneInfo.Local;
            DateTimeOffset serverOffset;

            if (datetime.Value.Kind != DateTimeKind.Utc)
            { 
                serverOffset = new DateTimeOffset(datetime.Value, serverTimezone.BaseUtcOffset);
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(serverOffset.UtcDateTime, TimeZoneInfo.FindSystemTimeZoneById(Timezone));

                return DateTime.SpecifyKind(localTime, DateTimeKind.Local);
            }
            else
            {
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(datetime.Value, TimeZoneInfo.FindSystemTimeZoneById(Timezone));

                return DateTime.SpecifyKind(localTime, DateTimeKind.Local);
            }
        }
        
        /// <summary>
        /// This method returns the date time as per the given timezone.
        /// Timezone is stored with the location in the timezone field.
        /// </summary>
        /// <param name="sourceDate">Source Date (it should be as per server timezone)</param>
        /// <param name="TimezoneName">Target Timezone</param>
        /// <returns></returns>
        public static DateTime GetLocationDateTime(DateTime sourceDate, String TimezoneName)
        {
            //Server timezone.
            TimeZoneInfo serverTimezone = TimeZoneInfo.Local;
            DateTimeOffset serverOffset = new DateTimeOffset(sourceDate, serverTimezone.BaseUtcOffset);
            
            DateTime locationDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverOffset.DateTime, serverTimezone.Id, TimezoneName);

            //Mention specifically that this datetime is local to a particular location.
            //so that database may store it accordingly as per UTC date time.
            locationDateTime = DateTime.SpecifyKind(locationDateTime, DateTimeKind.Local);

            return locationDateTime;
        }

        /// <summary>
        /// Gets the UTC from the local time for query purpose.
        /// </summary>
        /// <param name="locationDateTime"></param>
        /// <param name="LocationTimezone"></param>
        /// <returns></returns>
        public static DateTime GetLocationDateTimeToUTC(DateTime locationDateTime, String LocationTimezone)
        {
            TimeZoneInfo locationTimezone = TimeZoneInfo.FindSystemTimeZoneById(LocationTimezone);
            
            DateTimeOffset locationOffset = new DateTimeOffset(locationDateTime, locationTimezone.BaseUtcOffset);
            
            return locationOffset.UtcDateTime;
        }

    }


}
