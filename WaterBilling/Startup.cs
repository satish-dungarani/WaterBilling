using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WaterBilling.Startup))]
namespace WaterBilling
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
