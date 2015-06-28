using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using ptOS.Core.Realtime;
using ptOS.Core.Statistics;
using ptOS.Core.Statistics.Crunchers;

[assembly: OwinStartupAttribute(typeof(ptOS.Startup))]
namespace ptOS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();

            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => JsonSerializer.Create(new JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                MaxDepth = 2
            }));


            CrunchDispatcher.Crunchers.Add(new SystemEventsPerHour());
            CrunchDispatcher.Crunchers.Add(new ServerEventsPerHour());
        }
    }
}
