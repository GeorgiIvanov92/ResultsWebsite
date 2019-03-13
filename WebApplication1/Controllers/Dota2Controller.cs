using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApplication1.Models;
using Microsoft.Extensions.Configuration;
using Tracker.Models;
using WebApi.TransportObjects;
using WebApi.Cache;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class Dota2Controller : Controller
    {
        TrackerDBContext db;
        IConfiguration Configuration;
        private IMemoryCache _cache;
        public Dota2Controller(IMemoryCache memoryCache,TrackerDBContext db, IConfiguration config)
        {
            this.db = db;
            Configuration = config;
            _cache = memoryCache;
        }
        [HttpGet("[action]")]
        public Object GetSport()
        {
            var sport = _cache.Get("Dota2") as Sport;
            if (sport.LastUpdate < DateTime.UtcNow.AddSeconds(-int.Parse(Configuration.GetSection("CacheRefreshRateInSeconds").Value)))
            {
                _cache.Set("Dota2", CacheCreator.CreateSportCacheById(1, db, Configuration));
                sport = _cache.Get("Dota2") as Sport;
            }
            return sport;
        }
    }
}
