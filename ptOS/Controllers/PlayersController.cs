using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ptOS.Core;

namespace ptOS.Controllers
{
    public class PlayersController : ApiController
    {
        private ptOSContainer db = new ptOSContainer();

        [Route("api/Players/Search")]
        [HttpGet]
        public IQueryable<Player> GetLastPlayers()
        {
            return db.Players.OrderByDescending(x => x.Id).Take(50);
        }
        
        [Route("api/Players/Search/{query}")]
        [HttpGet]
        public IEnumerable<Player> SearchPlayers(string query)
        {
            if(String.IsNullOrWhiteSpace(query))
                return db.Players.OrderByDescending(x => x.Id).Take(50);
            var ips =
                db.EventDatas.Where(x => x.Event.Type == "ipchanged" && x.Value.Contains(query))
                    .Select(x => x.Event.Player).ToArray();
            var names =
                db.EventDatas.Where(x => x.Event.Type == "namechanged" && x.Value.Contains(query))
                    .Select(x => x.Event.Player).ToArray();

            var players =
                db.Players.Where(
                    x =>
                        x.Username.Contains(query) || x.Ip.Contains(query) || x.LastCountry.Contains(query) ||
                        x.Guid.Contains(query)).ToArray();

            var ret = ips.ToList();
            foreach (var name in names.Where(name => !ret.Contains(name)))
                ret.Add(name);
            foreach (var plr in players.Where(player => !ret.Contains(player)))
                ret.Add(plr);
            return ret;
        }

            // GET: api/Players/5
        [ResponseType(typeof(Player))]
        public IHttpActionResult GetPlayer(long id)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [Route("api/Players/{id}/Chat")]
        [HttpGet]
        public IEnumerable<Event> GetPlayerChat(long id)
        {
            return
                db.Events.Where(
                    x => x.PlayerId == id && (x.Type == "sayall" || x.Type == "sayteam" || x.Type == "saysquad"))
                    .OrderByDescending(x => x.Id).Take(500).ToArray();
        }

        [Route("api/Players/{id}/AdminEvents")]
        [HttpGet]
        public IEnumerable<Event> GetPlayerAdminEvents(long id)
        {
            return
                db.Events.Where(
                    x =>
                        x.PlayerId == id &&
                        (x.Type == "banned" || x.Type == "warned" || x.Type == "kicked" || x.Type == "gotbanned" ||
                         x.Type == "gotkicked" || x.Type == "gotwarned"))
                    .OrderByDescending(x => x.Id).ToArray();
        }

        [Route("api/Players/{id}/IpChanges")]
        [HttpGet]
        public IEnumerable<Event> GetPlayerIpChanges(long id)
        {
            return
                db.Events.Where(
                    x => x.PlayerId == id && (x.Type == "ipchanged"))
                    .OrderByDescending(x => x.Id).ToArray();
        }

        [Route("api/Players/{id}/NameChanges")]
        [HttpGet]
        public IEnumerable<Event> GetPlayerNameChanges(long id)
        {
            return
                db.Events.Where(
                    x => x.PlayerId == id && (x.Type == "namechanged"))
                    .OrderByDescending(x => x.Id).ToArray();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerExists(long id)
        {
            return db.Players.Count(e => e.Id == id) > 0;
        }
    }
}