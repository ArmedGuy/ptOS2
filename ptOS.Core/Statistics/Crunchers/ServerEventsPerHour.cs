using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ptOS.Core.Statistics.Crunchers
{
    public class ServerEventsPerHour : IStatisticsCruncher
    {
        static ServerEventsPerHour()
        {
            CrunchDispatcher.Crunchers.Add(new ServerEventsPerHour());
        }
        public string Name
        {
            get { return "ServerEventsPerHour"; }
        }

        public StatType Type
        {
            get { return StatType.Server; }
        }

        public void Crunch(ptOSContainer context, object obj)
        {
            var now = DateTime.UtcNow;
            var server = obj as Server;
            var stat = context.Statistics.FirstOrDefault(x => x.Type == Name && x.ServerId == server.Id);
            if (stat == null)
            {
                stat = context.Statistics.Add(new Statistic
                {
                    Type = Name,
                    At = now,
                    ServerId = server.Id
                });
            }

            var hourAgo = now.AddHours(-1);
            stat.Value = context.Events.Count(x => x.Submitted > hourAgo && x.ServerId == server.Id);

            context.SaveChanges();

        }
    }
}
