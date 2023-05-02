using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JobPortal.Startup))]
namespace JobPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
