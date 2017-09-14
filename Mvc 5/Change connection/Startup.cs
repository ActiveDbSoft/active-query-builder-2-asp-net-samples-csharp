using Microsoft.Owin;
using MVC5ChangeConnection;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace MVC5ChangeConnection
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

