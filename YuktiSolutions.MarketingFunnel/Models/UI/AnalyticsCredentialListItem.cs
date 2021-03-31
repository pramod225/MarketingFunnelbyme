using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YuktiSolutions.MarketingFunnel.Models.UI
{
    public class AnalyticsCredentialListItem
    {
        public Guid ID { get; set; }
        public Guid CreatedBy { get; set; }
        public String ApplicationName { get; set; }
        public String GAEmail { get; set; }
        public String GAViewID { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}