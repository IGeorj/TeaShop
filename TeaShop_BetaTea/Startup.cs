using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeaShop_BetaTea.Startup))]

namespace TeaShop_BetaTea
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}