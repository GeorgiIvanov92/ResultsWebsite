using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tracker.Models;
using WebApi.Entity;
using WebApplication1.Models;

namespace WebApi.Cache
{
    public class CacheCreator
    {
        private TrackerDBContext db;
        private Object _lock = new Object();
        private IConfiguration Configuration;
        private Dictionary<int, string> DefaultImagesById;
        private IMemoryCache _cache;
        public CacheCreator(TrackerDBContext db, IConfiguration config, IMemoryCache cache,TimeSpan time)
        {
            this.db = db;
            Configuration = config;
            _cache = cache;
            DefaultImagesById = new Dictionary<int, string>()
            {
                { 1,"defaultLoLLogo" },
                { 2,"defaultCSLogo" },
                { 3,"defaultDota2Logo" },
            };
        }

        public void UpdateMemoryCacheOnInterval(TimeSpan time)
        {
            while (true)
            {
                _cache.Set("LeagueOfLegends", CreateSportCacheById(1));
                _cache.Set("CSGO", CreateSportCacheById(2));
                _cache.Set("Dota2", CreateSportCacheById(3));
                Thread.Sleep(time);
            }
        }
        public Sport CreateSportCacheById(int sportId)
        {
            Sport sport = new Sport();
            ConcurrentDictionary<string, HashSet<Results>> ResultEvents = new ConcurrentDictionary<string, HashSet<Results>>();
            ConcurrentDictionary<string, HashSet<Prelive>> PreliveEvents = new ConcurrentDictionary<string, HashSet<Prelive>>();
            var results = db.Results.ToList();
            Parallel.ForEach(results, (result) =>
            {
                if (result.SportId == sportId)
                {
                    if (ResultEvents.ContainsKey(result.LeagueName))
                    {
                        lock (_lock)
                        {
                            ResultEvents[result.LeagueName].Add(result);
                        }
                    }
                    else
                    {
                        lock (_lock)
                        {
                            ResultEvents.TryAdd(result.LeagueName, new HashSet<Results>() { result });
                        }
                    }

                }
            });
            sport.ResultsEvents = ResultEvents;
            ConcurrentDictionary<string, string> images = new ConcurrentDictionary<string, string>();
            var defaultImageByteArray = System.IO.File.ReadAllBytes
                              (Configuration.GetSection("ImagePathReader").Value + DefaultImagesById[sportId] + ".png");
            var defaultImageString = Convert.ToBase64String(defaultImageByteArray);
            images.TryAdd("default", defaultImageString);
            Parallel.ForEach(ResultEvents, (league) =>
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
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            });
            sport.TeamLogos = images;

            var preliveEventsFromDb = db.Prelive.ToList();

            Parallel.ForEach(preliveEventsFromDb, (preliveEvent) =>
            {
                if (preliveEvent.SportId == sportId)
                {
                    if (PreliveEvents.ContainsKey(preliveEvent.LeagueName))
                    {
                        lock (_lock)
                        {
                            PreliveEvents[preliveEvent.LeagueName].Add(preliveEvent);
                        }
                    }
                    else
                    {
                        lock (_lock)
                        {
                            PreliveEvents.TryAdd(preliveEvent.LeagueName, new HashSet<Prelive>() { preliveEvent });
                        }
                    }

                }
            });
            sport.PreliveEvents = PreliveEvents;
            return sport;
        } 
    }
}
