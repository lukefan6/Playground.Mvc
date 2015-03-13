using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Playground.Mvc.Web.Startup))]
namespace Playground.Mvc.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
