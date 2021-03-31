using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using YuktiSolutions.MarketingFunnel.Models;
using YuktiSolutions.MarketingFunnel.Models.Database;

namespace YuktiSolutions.MarketingFunnel.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext context;
        protected ApplicationSignInManager _signInManager;
        protected ApplicationUserManager _userManager;
        private CultureInfo cultureInfo;

        protected override void Initialize(RequestContext requestContext)
        {

            /* VERY IMPORTANT NOTICE:
             * The formats specified here should not be changed.
             * Rather the client side formats should be changed to match the required 
             * server format.
             */


            /*TODO: Code here for setting the session language as per the browser's language preferences.
             * 1. Check if there is a language in the session. Session["Language"], if not, then move to step 2.
             * 2. Check if there is a language Cookie from the browser, if yes, then set it into the session variable. If not, move to step 3.
             * 3. Check if the user browser has language preference, pick the first language and set it into the Session variable (if our CMS
             * supports it). If not, set English language (as en-US) in the session variable.
             * */
            if (System.Web.HttpContext.Current.Session["Language"] == null)
            {
                if (requestContext.HttpContext.Response.Cookies.AllKeys.Contains("Language"))
                {
                    var getcookie = requestContext.HttpContext.Request.Cookies["Language"].Value;
                    if (getcookie != null)
                    {
                        System.Web.HttpContext.Current.Session["Language"] = getcookie;
                    }
                }
                else
                {
                    System.Web.HttpContext.Current.Session["Language"] = "en";
                }
            }

            cultureInfo = new CultureInfo(System.Web.HttpContext.Current.Session["Language"].ToString());
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            //set dot as basic separator
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ".";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "MM/dd/yyyy HH:mm:ss tt";
            Thread.CurrentThread.CurrentUICulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            Thread.CurrentThread.CurrentUICulture.DateTimeFormat.LongDatePattern = "MM/dd/yyyy HH:mm:ss tt";
            base.Initialize(requestContext);
        }

        public BaseController()
        {
            context = new ApplicationDbContext();
        }

        public BaseController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult KeepSessionAlive()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Setlanguage(string language)
        {
            Session["Language"] = language;
            cultureInfo = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            return Json(true);
        }
    }
}