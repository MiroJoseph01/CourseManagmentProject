using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CourseManagment.Web.Startup))]
namespace CourseManagment.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
