using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewsWebsite.Startup))]
namespace NewsWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
