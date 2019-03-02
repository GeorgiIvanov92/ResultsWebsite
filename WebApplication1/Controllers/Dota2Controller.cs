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
using WebApi.Entity;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class Dota2Controller : Controller
    {
        private IMemoryCache _cache;
        public Dota2Controller(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        [HttpGet("[action]")]
        public Object GetSport()
        {
            return _cache.Get("Dota2");
        }
    }
}
