using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Tracker.Models;
using WebApi.Cache;
using WebApi.TransportObjects;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class CounterStrikeController : Controller
    {
        TrackerDBContext db;
        IConfiguration Configuration;
        private IMemoryCache _cache;
        public CounterStrikeController(IMemoryCache memoryCache, TrackerDBContext db, IConfiguration config)
        {
            this.db = db;
            Configuration = config;
            _cache = memoryCache;
        }
        [HttpGet("[action]")]
        public Object GetSport()
        {
            var sport = _cache.Get("CSGO") as Sport;
            if (sport.LastUpdate < DateTime.UtcNow.AddSeconds(-int.Parse(Configuration.GetSection("CacheRefreshRateInSeconds").Value)))
            {
                _cache.Set("CSGO", CacheCreator.CreateSportCacheById(2, db, Configuration));
                sport = _cache.Get("CSGO") as Sport;
            }
            return sport;
        }
    }
}
