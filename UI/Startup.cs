using Microsoft.Owin;
using Owin;
using UI;

[assembly: OwinStartup(typeof(Startup))]
namespace UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
