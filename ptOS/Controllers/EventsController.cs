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
    public class EventsController : ApiController
    {
        private ptOSContainer db = new ptOSContainer();

        // GET: api/Events
        public IQueryable<Event> GetEvents()
        {
            return db.Events.OrderByDescending(x => x.Id).Take(100);
        }

        // GET: api/Events/5
        [ResponseType(typeof(Event))]
        public IHttpActionResult GetEvent(long id)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(long id)
        {
            return db.Events.Count(e => e.Id == id) > 0;
        }
    }
}