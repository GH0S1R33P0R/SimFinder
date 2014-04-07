using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SeniorProjectWeb.Startup))]
namespace SeniorProjectWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
