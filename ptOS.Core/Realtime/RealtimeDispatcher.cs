using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ptOS.Core.Realtime
{
    public class RealtimeDispatcher
    {
        private static Lazy<RealtimeDispatcher> _dispatcher = new Lazy<RealtimeDispatcher>(() => new RealtimeDispatcher(GlobalHost.ConnectionManager.GetHubContext<RealtimeHub>().Clients));

        public static RealtimeDispatcher Get()
        {
            return _dispatcher.Value;
        }

        private IHubConnectionContext<dynamic> _clients;

        public RealtimeDispatcher(IHubConnectionContext<dynamic> clients)
        {
            _clients = clients;
        }

        public void Broadcast(string evt, object data)
        {
            _clients.All.Broadcast(evt, data);
        }
    }
}
