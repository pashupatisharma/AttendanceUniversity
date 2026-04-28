using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eAttendance.Startup))]
namespace eAttendance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
