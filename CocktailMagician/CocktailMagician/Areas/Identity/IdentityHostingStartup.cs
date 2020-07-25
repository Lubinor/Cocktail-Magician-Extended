using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CocktailMagician.Web.Areas.Identity.IdentityHostingStartup))]
namespace CocktailMagician.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}