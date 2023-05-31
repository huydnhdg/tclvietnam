using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SALE_PRODUCT.Startup))]
namespace SALE_PRODUCT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
