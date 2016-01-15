using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LendingLibrary.Web.Startup))]
namespace LendingLibrary.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
