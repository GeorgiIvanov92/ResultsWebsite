using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using RabbitMQ.TransportObject;
using RabbitMQ.TrackerEssentials;
using static RabbitMQ.TrackerEssentials.Communication.Sports;
using RabbitMQ.RabbitMQ;

namespace Tracker.Sites
{
    public static class ZPlay
    {
        private static HttpClient _client;
        private static HttpClientHandler _handler;
        private static List<Link> _links;
        private static string _specificLoLUrl = "http://1zplay.com/api/lol_match/{0}?_={1}";
        private static string _specificDota2Url = "http://1zplay.com/api/dota2_match/{0}?_=1558706649155";
        private static HashSet<string> _csIds = new HashSet<string>();

        static ZPlay()
        {
            _handler = new HttpClientHandler();
            //_handler.UseProxy = true;
            //_handler.Proxy = new System.Net.WebProxy("163.172.182.5", 3128);          
            _client = new HttpClient(_handler);
        }
        private static string getEpochSeconds()
        {
            long epoch = (long) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            return epoch.ToString();
        }
        public static void GetActiveGameIds()
        {
            _links = new List<Link>();
            var response = _client.GetStringAsync("http://1zplay.com/api/live_schedules?_=1558693188967&category=all").Result;
            if (string.IsNullOrEmpty(response))
            {
                return;
            }
            var json = JArray.Parse(response);
            foreach(var game in json)
            {               
                var category = game["category"]?.ToString();
                var gameState = game["state"]?.ToString();
                if (!string.IsNullOrEmpty(category) && gameState != null && gameState == "start")
                {
                    SportEnum sport = SportEnum.Undefined;
                    Uri uri = null;
                    string league = string.Empty;
                    int bestOf = 0;
                    int mapNumber = 0;
                    switch (category)
                    {
                        case "csgo":
                            var websocketHandshake = game["csgo_schedule"].ToString();
                            _csIds.Add(websocketHandshake);
                            continue;
                        case "lol":
                            var id = game["live_match"]?["id"]?.ToString() ?? game["_id"].ToString();
                            bestOf = int.Parse(game["round"].ToString());
                            sport = SportEnum.LeagueOfLegends;
                            league = game["league"]["name"].ToString();
                            uri = new Uri(string.Format(_specificLoLUrl, id.Trim(), getEpochSeconds()));
                            _links.Add(new Link(sport, uri, league) { BestOf = bestOf});
                            continue;
                        case "dota2":
                            var dotaIds = game["dota2_matches"].ToString().Replace("[", "").Replace("]", "").Trim().Split(",");
                            mapNumber = dotaIds.Length;
                            bestOf = int.Parse(game["round"].ToString());
                            league = game["league"]["name"].ToString();
                            sport = SportEnum.Dota2;
                            sport = SportEnum.Dota2;
                            uri = new Uri(string.Format(_specificDota2Url, dotaIds[mapNumber-1].Trim()));
                            _links.Add(new Link(sport, uri,league,mapNumber,bestOf));
                            continue;
                    }
                }
            }
        }
        public static void SendActiveGames()
        {
            foreach(var link in _links)
            {
                try
                {
                    LiveEvent live;
                    switch (link.Sport)
                    {
                        case SportEnum.Dota2:
                            live = ParseDota2(link);
                            if(live != null) 
                            {
                                RabbitMQMessageSender.Send(live);
                            }
                            break;
                        case SportEnum.LeagueOfLegends:
                            live = ParseLeagueOfLegends(link);
                            if (live != null)
                            {
                                RabbitMQMessageSender.Send(live);
                            }
                            break;

                    }
                    
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }
        private static LiveEvent ParseDota2(Link link)
        {
            var json = JObject.Parse(_client.GetStringAsync(link.Uri).Result);
            LiveEvent ev = new LiveEvent();
            ev.Sport = SportEnum.Dota2;
            ev.GameTime = int.Parse(json["game_time"].ToString());
            ev.MapNumber = link.MapNumber;
            ev.LeagueName = link.LeagueName;
            ev.BestOf = link.BestOf;
            switch (json["first_tower"].ToString())
            {
                case "radiant":
                    ev.FirstTower = 2;
                    break;
                case "dire":
                    ev.FirstTower = 1;
                    break;
            }
            switch (json["first_blood"].ToString())
            {
                case "radiant":
                    ev.FirstBlood = 2;
                    break;
                case "dire":
                    ev.FirstBlood = 1;
                    break;
            }
            LiveTeam homeTeam = new LiveTeam();
            homeTeam.Players = new List<LivePlayer>();
            homeTeam.TeamName = json["radiant_team"]["name"].ToString();
            homeTeam.Gold = int.Parse(json["radiant"]["gold"].ToString());
            homeTeam.Kills = int.Parse(json["radiant"]["score"].ToString());
            ev.HomeTeam = homeTeam;
            LiveTeam awayTeam = new LiveTeam();
            awayTeam.Players = new List<LivePlayer>();
            awayTeam.TeamName = json["dire_team"]["name"].ToString();
            awayTeam.Gold = int.Parse(json["dire"]["gold"].ToString());
            awayTeam.Kills = int.Parse(json["dire"]["score"].ToString());
            ev.AwayTeam = awayTeam;
            foreach (var player in json["players"])
            {
                LivePlayer pl = new LivePlayer();
                pl.Nickname = player["account"]["name"].ToString();
                pl.ChampionName = player["hero"]["name_en"].ToString();
                pl.ChampionImageUrl = player["hero"]["image"].ToString();
                if (player["team"].ToString() == "radiant")
                {
                    ev.HomeTeam.Players.Add(pl);
                }
                else
                {
                    ev.AwayTeam.Players.Add(pl);
                }
            }
            return ev;
        }
        private static LiveEvent ParseLeagueOfLegends(Link link)
        {
            var json = JObject.Parse(_client.GetStringAsync(link.Uri).Result);
            LiveEvent ev = new LiveEvent();
            ev.Sport = SportEnum.LeagueOfLegends;
            ev.GameTime = int.Parse(json["game_time"].ToString());
            ev.MapNumber = link.MapNumber;
            ev.LeagueName = link.LeagueName;
            ev.BestOf = link.BestOf;
            switch (json["first_tower"].ToString())
            {
                case "red":
                    ev.FirstTower = 2;
                    break;
                case "blue":
                    ev.FirstTower = 1;
                    break;
            }
            switch (json["first_blood"].ToString())
            {
                case "red":
                    ev.FirstBlood = 2;
                    break;
                case "blue":
                    ev.FirstBlood = 1;
                    break;
            }
            LiveTeam homeTeam = new LiveTeam();
            homeTeam.Players = new List<LivePlayer>();
            homeTeam.TeamName = json["blue_team"]["name"].ToString();
            homeTeam.Gold = int.Parse(json["blue"]["gold"].ToString());
            homeTeam.Kills = int.Parse(json["blue"]["score"].ToString());
            ev.HomeTeam = homeTeam;
            LiveTeam awayTeam = new LiveTeam();
            awayTeam.Players = new List<LivePlayer>();
            awayTeam.TeamName = json["red_team"]["name"].ToString();
            awayTeam.Gold = int.Parse(json["red"]["gold"].ToString());
            awayTeam.Kills = int.Parse(json["red"]["score"].ToString());
            ev.AwayTeam = awayTeam;
            foreach (var player in json["blue"]["players"])
            {
                LivePlayer pl = new LivePlayer();
                pl.Nickname = player["name"].ToString();
                pl.ChampionName = player["hero"]["name"].ToString();
                pl.ChampionImageUrl = player["hero"]["image_url"].ToString();
                if (player["color"].ToString() == "blue")
                {
                    ev.HomeTeam.Players.Add(pl);
                }
            }
            foreach (var player in json["red"]["players"])
            {
                LivePlayer pl = new LivePlayer();
                pl.Nickname = player["name"].ToString();
                pl.ChampionName = player["hero"]["name"].ToString();
                pl.ChampionImageUrl = player["hero"]["image_url"].ToString();
                if (player["color"].ToString() == "red")
                {
                    ev.AwayTeam.Players.Add(pl);
                }
            }
            return ev;
        }
    }
}
