using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YanZhiwei.DotNet.MVC.AdminPanel.UI.Startup))]
namespace YanZhiwei.DotNet.MVC.AdminPanel.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
