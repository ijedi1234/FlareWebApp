using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FlareWebApp.Startup))]
namespace FlareWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
