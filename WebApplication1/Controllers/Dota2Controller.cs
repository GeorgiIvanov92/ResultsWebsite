using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class Dota2Controller : Controller
    {
        private TrackerDBContext db;
        private ConcurrentDictionary<string, HashSet<Results>> Leagues = new ConcurrentDictionary<string, HashSet<Results>>();
        private Object _lock = new Object();
        public Dota2Controller(TrackerDBContext db)
        {  
            if (this.db == null)
            {
                this.db = db;
            }
        }
        [HttpGet("[action]")]
        public ConcurrentDictionary<string, HashSet<Results>> GetResults()
        {
            var results = db.Results.ToList();
            Parallel.ForEach(results, (result) =>
            {
                if (result.SportId == 3)
                {
                    if (Leagues.ContainsKey(result.LeagueName))
                    {
                        lock (_lock)
                        {
                            Leagues[result.LeagueName].Add(result);
                        }
                    }
                    else
                    {
                        lock (_lock)
                        {
                            Leagues.TryAdd(result.LeagueName, new HashSet<Results>() { result });
                        }
                    }
                       
                }
            });
            return Leagues;
            
        }
    }
}
