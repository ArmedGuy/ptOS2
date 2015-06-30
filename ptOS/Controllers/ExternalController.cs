using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MaxMind.GeoIP2;
using ptOS.Core;
using ptOS.Core.Realtime;
using ptOS.Core.Statistics;
using ptOS.Models;

namespace ptOS.Controllers
{
    public class ExternalController : BaseApiController
    {
        [Route("api/External/RegisterPlayerEvent")]
        [HttpPost]
        public bool RegisterEvent(RegisterEventModel model)
        {
            var server = Context.Servers.FirstOrDefault(x => x.Guid == model.ServerGuid && x.AuthKey == model.ServerKey);
            if(server == null)
                throw new InvalidOperationException("No server matching guid/key pair");
            Task.Run(() =>
            {
                var player = Context.Players.FirstOrDefault(x => x.Guid == model.PlayerGuid);
                if (player == null)
                {
                    // Create new player
                    player = new Player
                    {
                        Guid = model.PlayerGuid,
                        FirstSeen = DateTime.UtcNow
                    };
                    Context.Players.Add(player);
                    Context.SaveChanges();
                }
                player.LastSeen = DateTime.UtcNow;
                player.LastServer = server;
                if (player.Username != model.PlayerName && !String.IsNullOrWhiteSpace(player.Username) && !String.IsNullOrWhiteSpace(model.PlayerName))
                {
                    // new event for namechange
                    Context.Events.Add(Event.NewChangeEvent(player, "namechanged", player.Username, model.PlayerName));
                    
                }
                player.Username = model.PlayerName;

                if (player.Ip != model.PlayerIp && !String.IsNullOrWhiteSpace(player.Ip) && !String.IsNullOrWhiteSpace(model.PlayerIp))
                {
                    // new event for ipchange
                    Context.Events.Add(Event.NewChangeEvent(player, "ipchanged", player.Ip, model.PlayerIp));
                    
                }
                player.Ip = model.PlayerIp;
                if(!String.IsNullOrWhiteSpace(player.Ip))
                    player.LastCountry = Startup.IpDatabase.City(player.Ip).Country.IsoCode;
                
                var evt = new Event
                {
                    PlayerId = player.Id,
                    ServerId = server.Id,
                    Type = model.EventType,
                    Submitted = DateTime.UtcNow
                };
                Context.Events.Add(evt);
                Context.SaveChanges();

                if (model.EventData != null)
                {
                    foreach (var evd in model.EventData.Select(kv => new EventData
                    {
                        EventId = evt.Id,
                        Key = kv.Key,
                        Value = kv.Value
                    }))
                    {
                        Context.EventDatas.Add(evd);
                    }
                }
                Context.SaveChanges();

                RealtimeDispatcher.Get().Broadcast("event.new", evt);

                // Calculate statistics
                new CrunchDispatcher(player).Crunch();
                new CrunchDispatcher(server).Crunch();
                new CrunchDispatcher().Crunch();
            });
            return true;
        }


    }
}
