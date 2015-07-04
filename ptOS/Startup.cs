using System.Web;
using System.Web.Hosting;
using Hangfire;
using MaxMind.GeoIP2;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using ptOS.Core;
using ptOS.Core.Realtime;
using ptOS.Core.Statistics;
using ptOS.Core.Statistics.Crunchers;

[assembly: OwinStartupAttribute(typeof(ptOS.Startup))]
namespace ptOS
{
    public partial class Startup
    {
        public static DatabaseReader IpDatabase;
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();

            GlobalConfiguration.Configuration
                .UseSqlServerStorage("hangfire");

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => JsonSerializer.Create(new JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }));

            IpDatabase = new DatabaseReader(HostingEnvironment.MapPath("/App_Data/GeoLite2-City.mmdb"));

            CrunchDispatcher.Crunchers.Add(new SystemEventsPerHour());
            CrunchDispatcher.Crunchers.Add(new ServerEventsPerHour());
        }
    }
}
