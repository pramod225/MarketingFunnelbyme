using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YuktiSolutions.MarketingFunnel.Startup))]
namespace YuktiSolutions.MarketingFunnel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
