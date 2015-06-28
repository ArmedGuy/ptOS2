using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ptOS.Core
{
    public partial class Event
    {
        public static Event NewChangeEvent(Player p, string changed, string from, string to)
        {
            var evt = new Event
            {
                Player = p,
                Submitted = DateTime.UtcNow,
                Type = changed
            };

            evt.EventDatas.Add(new EventData
            {
                Event = evt,
                Key = "from",
                Value = from
            });
            evt.EventDatas.Add(new EventData
            {
                Event = evt,
                Key = "to",
                Value = to
            });
            return evt;
        }
    }
}
