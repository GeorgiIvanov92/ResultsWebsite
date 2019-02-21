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
    public class LeagueOfLegendsController : Controller
    {
        private TrackerDBContext db;
        private ConcurrentDictionary<string, HashSet<Results>> Leagues = new ConcurrentDictionary<string, HashSet<Results>>();
        private Object _lock = new Object();
        public LeagueOfLegendsController(TrackerDBContext db)
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
                if (result.SportId == 1)
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
        //useless since after calling fetch the list isnt ordered
        private List<Results> BubbleSort(List<Results> results)
        {
            Results tempRes;
            for(int a=0; a<results.Count; a++)
            {
                for (int i = 0; i < results.Count - 1; i++)
                {
                    if (results[i].GameDate < results[i + 1].GameDate)
                    {
                        tempRes = results[i + 1];
                        results[i + 1] = results[i];
                        results[i] = tempRes;
                    }
                }
            }
            return results;
        }
    }
}
