using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(toddt_weather_forecast.Startup))]
namespace toddt_weather_forecast
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
