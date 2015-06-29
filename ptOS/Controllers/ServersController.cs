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
    public class ServersController : ApiController
    {
        private ptOSContainer db = new ptOSContainer();

        // GET: api/Servers
        public IQueryable<Server> GetServers()
        {
            return db.Servers;
        }

        // GET: api/Servers/5
        [ResponseType(typeof(Server))]
        public IHttpActionResult GetServer(int id)
        {
            Server server = db.Servers.Find(id);
            if (server == null)
            {
                return NotFound();
            }

            return Ok(server);
        }

        // PUT: api/Servers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutServer(int id, Server server)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != server.Id)
            {
                return BadRequest();
            }

            db.Entry(server).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Servers
        [ResponseType(typeof(Server))]
        public IHttpActionResult PostServer(Server server)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            server.Guid = Guid.NewGuid().ToString();
            server.AuthKey = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 32);

            db.Servers.Add(server);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ServerExists(server.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("ptOSApi", new { id = server.Id }, server);
        }

        // DELETE: api/Servers/5
        [ResponseType(typeof(Server))]
        public IHttpActionResult DeleteServer(int id)
        {
            Server server = db.Servers.Find(id);
            if (server == null)
            {
                return NotFound();
            }

            db.Servers.Remove(server);
            db.SaveChanges();

            return Ok(server);
        }

        [Route("api/Servers/{id}/Players")]
        [HttpGet]
        public IQueryable<Player> GetPlayersOnServer(int id)
        {
            var lastSeen = DateTime.UtcNow.AddMinutes(-5);
            return db.Players.Where(x => x.LastServer.Id == id && x.LastSeen > lastSeen).OrderByDescending(x => x.LastSeen);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServerExists(int id)
        {
            return db.Servers.Count(e => e.Id == id) > 0;
        }
    }
}