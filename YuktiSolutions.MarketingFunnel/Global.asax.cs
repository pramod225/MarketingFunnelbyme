using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using YuktiSolutions.MarketingFunnel.Migrations;
using YuktiSolutions.MarketingFunnel.Models.Database;


namespace YuktiSolutions.MarketingFunnel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {           
            //Automatically upgradation to the new database version.
            Database.SetInitializer(new System.Data.Entity.MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
