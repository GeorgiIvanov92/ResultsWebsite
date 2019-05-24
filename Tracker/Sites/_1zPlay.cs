using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Tracker.TrackerEssentials;

namespace Tracker.Sites
{
    class _1zPlay
    {
        private static HttpClient _client = new HttpClient();
        private static List<Link> _links;
        private static string _specificLoLUrl = "http://1zplay.com/api/lol_match/{0}?_={1}";
        private static string _specificDota2Url = "http://1zplay.com/api/dota2_match/{0}?_=1558706649155";
        private static HashSet<string> _csIds = new HashSet<string>();

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
                    TrackerEssentials.Communication.Sports.SportEnum sport = TrackerEssentials.Communication.Sports.SportEnum.Undefined;
                    Uri uri = null;
                    switch (category)
                    {
                        case "csgo":
                            var websocketHandshake = game["csgo_schedule"].ToString();
                            _csIds.Add(websocketHandshake);
                            continue;
                        case "lol":
                            var id = game["live_match"]["id"].ToString();
                            sport = TrackerEssentials.Communication.Sports.SportEnum.LeagueOfLegends;
                            uri = new Uri(string.Format(_specificLoLUrl, id.Trim(), getEpochSeconds()));                            
                            break;
                        case "dota2":
                            var dotaIds = game["dota2_matches"].ToString().Replace("[", "").Replace("]", "").Trim().Split(",");
                            sport = TrackerEssentials.Communication.Sports.SportEnum.Dota2;
                            if (dotaIds.Length == 2)
                            {
                                uri = new Uri(string.Format(_specificDota2Url, dotaIds[0].Trim()));
                                _links.Add(new Link(sport, uri));
                                uri = new Uri(string.Format(_specificDota2Url, dotaIds[1].Trim()));
                            }else if(dotaIds.Length == 1)
                            {
                                uri = new Uri(string.Format(_specificDota2Url, dotaIds[0].Trim()));
                            }
                            break;
                    }
                    var response2 = _client.GetStringAsync(uri).Result;
                    _links.Add(new Link(sport,uri));
                }
            }
        }
    }
}
