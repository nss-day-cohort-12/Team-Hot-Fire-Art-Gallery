using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Art_Gallery.Startup))]
namespace Art_Gallery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
