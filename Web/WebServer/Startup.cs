using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Web.Startup), "Configuration")]
namespace Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Microsoft.AspNet.SignalR.StockTicker.Startup.ConfigureSignalR(app);
        }
    }
}