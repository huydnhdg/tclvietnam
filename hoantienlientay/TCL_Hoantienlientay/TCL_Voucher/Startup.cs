using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TCL_Voucher.Startup))]
namespace TCL_Voucher
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
