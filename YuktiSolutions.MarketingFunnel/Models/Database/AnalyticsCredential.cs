using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace YuktiSolutions.MarketingFunnel.Models.Database
{
    [Table("mrktfunnel_antcredential")]
    public class AnalyticsCredential
    {
        public Guid ID { get; set; }
        public Guid CreatedBy { get; set; }

        [Display(Name = "Application Name")]
        [StringLength(256, ErrorMessage = "Display name cannot be more than {1} characters long.")]
        public String ApplicationName { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime GAStartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime GAEndDate { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Google Analytics Email")]
        public String GAEmail { get; set; }
        [Required]
        [Display(Name = "Google Analytics View-Id")]
        public String GAViewID { get; set; }
        public String GAP12FilePath { get; set; }
        public String GAJsonFilePath { get; set; }
        public DateTime CreatedOn { get; set; }
        public void Save()
        {
            AnalyticsCredentialManager.SaveAnalyticsCredential(this);
        }
    }
    public static class AnalyticsCredentialManager
    {
        public static void SaveAnalyticsCredential(AnalyticsCredential analyticsCredential)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                SaveAnalyticsCredential(analyticsCredential, context);
                context.SaveChanges();
            }
        }
        public static void SaveAnalyticsCredential(AnalyticsCredential analyticsCredential, ApplicationDbContext context)
        {
            /*Check duplicate*/
            if (context.AnalyticsCredentials.Any(x => x.ID != analyticsCredential.ID && analyticsCredential.ApplicationName.Equals(x.ApplicationName, StringComparison.CurrentCultureIgnoreCase)))
                throw new ApplicationException("Application Name already exists. Duplicate appliation name are not allowed.");
            var record = context.AnalyticsCredentials.FirstOrDefault(x => x.ID == analyticsCredential.ID);
            if (record == null)
            {
                analyticsCredential.CreatedOn = DateTime.Now;
                record = context.AnalyticsCredentials.Add(analyticsCredential);
            }
            record.ApplicationName = analyticsCredential.ApplicationName;
            record.GAStartDate = analyticsCredential.GAStartDate;
            record.GAEndDate = analyticsCredential.GAEndDate;
            record.GAEmail = analyticsCredential.GAEmail;
            record.GAViewID = analyticsCredential.GAViewID;
            record.GAP12FilePath = analyticsCredential.GAP12FilePath;
            record.GAJsonFilePath = analyticsCredential.GAJsonFilePath;
        }
    }
}