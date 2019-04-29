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
using WebApi.TransportObjects;
using Tracker.Models;
using WebApi.Cache;
using WebApi.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class LeagueOfLegendsController : Controller
    {
        private TrackerDBContext db;
        private IConfiguration Configuration;
        private IMemoryCache _cache;
        public LeagueOfLegendsController(TrackerDBContext db, IConfiguration config, IMemoryCache memoryCache)
        {
            this.db = db;
            Configuration = config;
            _cache = memoryCache;
        }

        [HttpGet("GetSport")]
        public Sport GetSport()
        {
            var sport = _cache.Get("LeagueOfLegends") as Sport;
            if (sport.LastUpdate < DateTime.UtcNow.AddSeconds(-int.Parse(Configuration.GetSection("CacheRefreshRateInSeconds").Value)))
            {
                _cache.Set("LeagueOfLegends",CacheCreator.CreateSportCacheById(1, db, Configuration));
                sport = _cache.Get("LeagueOfLegends") as Sport;
            }
                return sport;
        }
    }
}
