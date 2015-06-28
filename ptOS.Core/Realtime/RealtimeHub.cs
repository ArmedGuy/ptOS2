using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ptOS.Core.Realtime
{
    public class RealtimeHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}