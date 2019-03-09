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
using WebApi.Models;
using WebApplication1.Models;

namespace WebApi.Cache
{
    public static class CacheCreator
    {
        private static Object _lock = new Object();
        private static Dictionary<int, string> DefaultImagesById;
        static CacheCreator()
        {
            DefaultImagesById = new Dictionary<int, string>()
            {
                { 1,"defaultLoLLogo" },
                { 2,"defaultCSLogo" },
                { 3,"defaultDota2Logo" },
            };
        }
        public static Sport CreateSportCacheById(int sportId, TrackerDBContext db, IConfiguration Configuration)
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
            sport.LastUpdate = DateTime.UtcNow;

            var teamsFromDb = db.Team.ToList();
            var playersFromDb = db.Player.ToList();

            ConcurrentDictionary<string, ConcurrentDictionary<Team, HashSet<Player>>> leaguesAndTeams = 
                new ConcurrentDictionary<string, ConcurrentDictionary<Team, HashSet<Player>>>();

            Parallel.ForEach(teamsFromDb, (team) =>
            {
                if(team.SportId == sportId)
                {                   
                    HashSet<Player> playersInTeam = new HashSet<Player>();
                    foreach (var player in playersFromDb)
                    {
                        if(player.TeamId == team.Id)
                        {
                            playersInTeam.Add(player);
                        }
                    }
                    if (leaguesAndTeams.ContainsKey(team.Region))
                    {
                        leaguesAndTeams[team.Region].TryAdd(team, playersInTeam);
                    }
                    else
                    {
                        ConcurrentDictionary<Team, HashSet<Player>> teams = new ConcurrentDictionary<Team, HashSet<Player>>();
                        teams.TryAdd(team, playersInTeam);
                        leaguesAndTeams.TryAdd(team.Region, teams);
                    }
                }
            });
            sport.TeamsInLeagues = leaguesAndTeams;
                return sport;
        } 
    }
}
