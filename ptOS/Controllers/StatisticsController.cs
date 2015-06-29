using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ptOS.Core;
using ptOS.Models;

namespace ptOS.Controllers
{
    public class StatisticsController : BaseApiController
    {
        [Route("api/Statistics/System")]
        [HttpGet]
        public IEnumerable<Statistic> SystemStats()
        {
            return Context.Statistics.Where(x => x.ServerId == null && x.PlayerId == null);
        }

        [Route("api/Statistics/Server/{id:int}")]
        [HttpGet]
        public IEnumerable<Statistic> ServerStats(int id)
        {
            return Context.Statistics.Where(x => x.ServerId == id);
        }

        [Route("api/Statistics/CountriesLastHour")]
        [HttpGet]
        public IEnumerable<CountryWeight> GetCountriesLastHour()
        {
            var lastActive = DateTime.UtcNow.AddHours(-1);
            return
                Context.Players.Where(x => x.LastSeen > lastActive)
                    .GroupBy(x => x.LastCountry)
                    .Select(x => new CountryWeight
                    {
                        Country = x.Key,
                        Weight = x.Sum(y => 1)
                    }).ToArray();
        } 
    }
}
