using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YuktiSolutions.MarketingFunnel.Models
{
    public class GAListViewModel
    {
        public string Dimensions { get; set; }
        public string Metrics { get; set; }

       public GAListViewModel(string dimensions, string metrics)
        {
            this.Dimensions = dimensions;
            this.Metrics = metrics;
        }
    }
}