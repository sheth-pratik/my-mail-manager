using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyMailWeb.Startup))]
namespace MyMailWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
