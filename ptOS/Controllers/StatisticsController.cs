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
                        Weight = x.Count()
                    }).ToArray();
        }



        [Route("api/Statistics/EventsByHourLastDay")]
        [HttpGet]
        public IEnumerable<ChartStatistic> GetEventsByHourLastDay()
        {
            var lastActive = DateTime.UtcNow.AddDays(-1);
            return
                Context.Events.Where(x => x.Submitted > lastActive)
                    .GroupBy(x => x.Submitted.Hour)
                    .OrderBy(x => x.Key)
                    .Select(x => new ChartStatistic
                    {
                        Key = x.Key.ToString(),
                        Value = x.Count()
                    });
        }

        [Route("api/Statistics/EventsByServerLastDay")]
        [HttpGet]
        public IEnumerable<ChartStatistic> GetEventsByServerLastDay()
        {
            var lastActive = DateTime.UtcNow.AddDays(-1);
            return
                Context.Events.Where(x => x.Submitted > lastActive)
                    .GroupBy(x => x.Server)
                    .Select(x => new ChartStatistic
                    {
                        Key = x.Key.Name,
                        Value = x.Count()
                    });
        } 
    }
}
