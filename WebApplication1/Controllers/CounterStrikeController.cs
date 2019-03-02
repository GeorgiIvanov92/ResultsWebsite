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
using WebApi.Entity;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class CounterStrikeController : Controller
    {
        private IMemoryCache _cache;
        public CounterStrikeController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;         
        }
        [HttpGet("[action]")]
        public Object GetSport()
        {
            return _cache.Get("CSGO");
        }
    }
}
