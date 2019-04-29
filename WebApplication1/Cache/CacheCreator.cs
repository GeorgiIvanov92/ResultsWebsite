using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tracker.Models;
using WebApi.TransportObjects;
using WebApi.Models;
using WebApplication1.Models;
using System.Net.Http;

namespace WebApi.Cache
{
    public static class CacheCreator
    {
        private static Object _lock = new Object();
        private static Dictionary<int, string> DefaultImagesById;
        private static HttpClient Client;
        private static string MappingServiceUrl = string.Empty;
        static CacheCreator()
        {           
            Client = new HttpClient();
            DefaultImagesById = new Dictionary<int, string>()
            {
                { 1,"defaultLoLLogo" },
                { 2,"defaultCSLogo" },
                { 3,"defaultDota2Logo" },
            };
        }
        public static Sport CreateSportCacheById(int sportId, TrackerDBContext db, IConfiguration Configuration)
        {
            MappingServiceUrl = Configuration.GetSection("MappingServiceUrl").Value;
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
            var players = db.Player.ToList();
            var playersToAdd = new List<Player>();
            foreach(var player in players)
            {
                if(player.SportId != sportId)
                {
                    continue;
                }
                bool shouldAddPlayer = true;
                foreach(var playerToAdd in playersToAdd)
                {
                    if(player.Nickname == playerToAdd.Nickname)
                    {
                        shouldAddPlayer = false;
                        break;
                    }
                }
                if (shouldAddPlayer)
                {
                    playersToAdd.Add(player);
                }
            }
            sport.Players = playersToAdd;
            ConcurrentDictionary<string, HashSet<Team>> teams = new ConcurrentDictionary<string, HashSet<Team>>();

            Parallel.ForEach(teamsFromDb, (team) =>
            {
                if(team.SportId == sportId)
                {
                    if (teams.ContainsKey(team.Region))
                    {
                        lock (_lock)
                        {                          
                                teams[team.Region].Add(team);
                            
                        }
                    }
                    else
                    {
                        lock (_lock)
                        {
                            var masterLeague = GetMasterLeagueFromMappingService
                            ($"{MappingServiceUrl}/Mapping/GetLeague", sportId, team.Name.ToLowerInvariant().Contains("academy") ?
                            $"{team.Region}Academy" : team.Region).Result;
                            if (masterLeague == null || masterLeague.Equals("Bad POST params") || masterLeague.Equals("Could Not Locate League"))
                            {
                                teams.TryAdd(team.Region, new HashSet<Team>() { team });
                            }
                            else
                            {
                                team.Region = masterLeague;
                                if (teams.ContainsKey(masterLeague))
                                {
                                    teams[team.Region].Add(team);
                                }
                                else
                                {
                                    teams.TryAdd(team.Region, new HashSet<Team>() { team });
                                }
                            }
                        }
                    }               
                }
            });
            sport.TeamsInLeague = teams;
            ConcurrentDictionary<string, string> images = new ConcurrentDictionary<string, string>();
            var defaultImageByteArray = System.IO.File.ReadAllBytes
                              (Configuration.GetSection("ImagePathReader").Value + DefaultImagesById[sportId] + ".png");
            var defaultImageString = Convert.ToBase64String(defaultImageByteArray);
            images.TryAdd("default", defaultImageString);
            Parallel.ForEach(ResultEvents, (league) =>
            {
                foreach (var res in league.Value)
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
            Parallel.ForEach(teams, (league) =>
            {
                    foreach (var team in league.Value)
                    {
                        try
                        {
                            if (!images.ContainsKey(team.Name))
                            {
                                var imageByteArray = System.IO.File.ReadAllBytes
                                    (Configuration.GetSection("ImagePathReader").Value + team.Name + ".png");
                                var imageString = Convert.ToBase64String(imageByteArray);
                                images.TryAdd(team.Name.Trim(), imageString);
                            }                           
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                }
            });
            sport.TeamLogos = images;
            if (sportId == 1)
            {
                sport.ChampionStats = db.ChampionStat.ToList();
            }
            return sport;
        }
        private static async Task<string> GetMasterLeagueFromMappingService(string url,int sportId, string leagueName)
        {
            var values = new Dictionary<string, string>
            {
                { "league", $"{leagueName}@{sportId.ToString()}" },  
            };

            var content = new FormUrlEncodedContent(values);

            var response = await Client.PostAsync(url, content);

            return await response.Content.ReadAsStringAsync();
        }
    }

}
