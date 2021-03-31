using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YuktiSolutions.MarketingFunnel.Models
{
    public class AjaxResponse
    {
        /// <summary>
        /// Whether the ajax operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message from the server (mostly in case of errors).
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// If the server redirects the user to somewhere.
        /// </summary>
        public String RedirectURL { get; set; }
    }
}