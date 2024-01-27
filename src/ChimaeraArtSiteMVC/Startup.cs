using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChimaeraArtSiteMVC.Startup))]
namespace ChimaeraArtSiteMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
