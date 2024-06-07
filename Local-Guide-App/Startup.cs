using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Local_Guide_App.Startup))]
namespace Local_Guide_App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
