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

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class LeagueOfLegendsController : Controller
    {
        private TrackerDBContext db;
        private ConcurrentDictionary<string, HashSet<Results>> Leagues = new ConcurrentDictionary<string, HashSet<Results>>();
        private Object _lock = new Object();
        private IConfiguration Configuration;
        public LeagueOfLegendsController(TrackerDBContext db, IConfiguration Configuration)
        {  
            if (this.db == null)
            {
                this.db = db;
            }
            if(this.Configuration == null)
            {
                this.Configuration = Configuration;
            }
        }
        [HttpGet("[action]")]
        public ConcurrentDictionary<string, HashSet<Results>> GetResults()
        {
            Leagues = new ConcurrentDictionary<string, HashSet<Results>>();
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
        [HttpGet("[action]")]
        public ConcurrentDictionary<string, string> GetImages()
        {
            Leagues = new ConcurrentDictionary<string, HashSet<Results>>();
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
            ConcurrentDictionary<string, string> images = new ConcurrentDictionary<string, string>();
            var defaultImageByteArray = System.IO.File.ReadAllBytes
                              (Configuration.GetSection("ImagePathReader").Value + "defaultLoLLogo" + ".png");
            var defaultImageString = Convert.ToBase64String(defaultImageByteArray);
            images.TryAdd("default", defaultImageString);
            Parallel.ForEach(Leagues, (league) =>
            {
                var results2 = league.Value;
                foreach (var res in results2)
                {
                    try
                    {
                        if (!images.ContainsKey(res.HomeTeam))
                        {
                            var imageByteArray = System.IO.File.ReadAllBytes
                                (Configuration.GetSection("ImagePathReader").Value + res.HomeTeam + ".png");
                            var imageString = Convert.ToBase64String(imageByteArray);
                            images.TryAdd(res.HomeTeam.Trim(), imageString);
                        }
                        else if (!images.ContainsKey(res.AwayTeam))
                        {
                            var imageByteArray = System.IO.File.ReadAllBytes
                                                            (Configuration.GetSection("ImagePathReader").Value + res.AwayTeam + ".png");
                            var imageString = Convert.ToBase64String(imageByteArray);
                            images.TryAdd(res.AwayTeam.Trim(), imageString);
                        }
                    }catch(Exception ex)
                    {
                        continue;
                    }
                }
            });
            return images;
        }

    }
}
