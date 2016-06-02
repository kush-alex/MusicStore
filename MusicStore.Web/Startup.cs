using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusicStore.Web.Startup))]
namespace MusicStore.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
