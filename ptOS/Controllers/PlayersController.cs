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

        // GET: api/Players
        [Route("api/Players/{page:int?}")]
        public IQueryable<Player> GetPlayers(int page = 0)
        {
            return db.Players.OrderByDescending(x => x.Id).Skip(page*20).Take(20);
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
        public IQueryable<Event> GetPlayerChat(long id)
        {
            return
                db.Events.Where(
                    x => x.PlayerId == id && (x.Type == "sayglobal" || x.Type == "sayteam" || x.Type == "saysquad"))
                    .OrderByDescending(x => x.Id);
        }

        [Route("api/Players/{id}/Chat")]
        [HttpGet]
        public IQueryable<Event> GetPlayerAdminEvents(long id)
        {
            return
                db.Events.Where(
                    x => x.PlayerId == id && (x.Type == "banned" || x.Type == "warned" || x.Type == "kicked"))
                    .OrderByDescending(x => x.Id);
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