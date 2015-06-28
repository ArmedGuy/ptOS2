using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ptOS.Core.Statistics.Crunchers
{
    public class SystemEventsPerHour : IStatisticsCruncher
    {
        public string Name
        {
            get { return "EventsPerHour"; }
        }

        public StatType Type
        {
            get { return StatType.Other; }
        }

        public void Crunch(ptOSContainer context, object obj)
        {
            var now = DateTime.UtcNow;
            var stat = context.Statistics.FirstOrDefault(x => x.Type == Name);
            if (stat == null)
            {
                stat = context.Statistics.Add(new Statistic
                {
                    Type = Name,
                    At = now,
                });
            }
            var hourAgo = now.AddHours(-1);
            stat.Value = context.Events.Count(x => x.Submitted > hourAgo);

            context.SaveChanges();

        }
    }
}
