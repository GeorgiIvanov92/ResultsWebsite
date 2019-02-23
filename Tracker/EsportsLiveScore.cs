using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Tracker.Models;
using Tracker.TrackerEssentials;
using static Tracker.TrackerEssentials.Communication.Sports;

namespace Tracker
{
    public static class EsportsLiveScore
    {
        private static HttpClient client;
        public static List<Link> ResultsLinks;
        private static string _baseUrl = "http://www.esportlivescore.com/";
        private static readonly string format = "dd/MM HH:mm";
        private static readonly Regex tournamentNameRegex = new Regex(@"title=""(.*?)""", RegexOptions.Compiled | RegexOptions.Multiline);
        private static List<Link> TeamLinks;
        private static readonly string _baseTeamUrl = "http://www.esportlivescore.com/t_{0}-{1}_g_{2}.html";
        public static void GetNewLinks()
        {
            ResultsLinks = new List<Link>();
            if (client == null)
            {
                client = new HttpClient();
            }
            var page = client.GetStringAsync(_baseUrl).Result;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);
            var linkNodes = doc.DocumentNode.SelectSingleNode("//ul[contains(@id,'games')]").SelectNodes(".//li/a");
            foreach(var node in linkNodes)
            {              
                var url = node.Attributes["href"].Value;
                Link link;
                if (url.Contains("csgo"))
                {
                    link = new Link(TrackerEssentials.Communication.Sports.SportEnum.CounterStrike, new Uri(_baseUrl+url));
                    ResultsLinks.Add(link);
                }else if (url.Contains("leagueoflegends"))
                {
                    link = new Link(TrackerEssentials.Communication.Sports.SportEnum.LeagueOfLegends, new Uri(_baseUrl + url));
                    ResultsLinks.Add(link);
                }else if (url.Contains("dota"))
                {
                    link = new Link(TrackerEssentials.Communication.Sports.SportEnum.Dota2, new Uri(_baseUrl + url));
                    ResultsLinks.Add(link);
                }
            }
        }
        public static List<Results> GetResultEvents()
        {
            List<Results> results = new List<Results>();
            foreach(var link in ResultsLinks)
            {
                try
                {
                    var page = client.GetStringAsync(link.Uri).Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    var finishedMatchesNodes = doc.DocumentNode.SelectSingleNode("//div[contains(@id,'finished')]").SelectNodes(".//div[contains(@id,'event_id')]");
                    foreach (var gameNode in finishedMatchesNodes)
                    {
                        try
                        {
                            var dateString = gameNode.SelectSingleNode(".//div[contains(@class,'event_date_day_month')]").InnerText.Trim();
                            var hourAndMinute = gameNode.SelectSingleNode(".//div[contains(@class,'event_date_hour_minutes')]").InnerText.Trim();
                            var entireString = $"{dateString} {hourAndMinute}";
                            DateTime dt;

                            if (!DateTime.TryParseExact(entireString, format, new CultureInfo("en-Us"), DateTimeStyles.AdjustToUniversal, out dt))
                            {

                            }
                            var homeTeam = gameNode.SelectSingleNode(".//div[contains(@class,'team-home')]").InnerText.Trim();
                            var awayTeam = gameNode.SelectSingleNode(".//div[contains(@class,'team-away')]").InnerText.Trim();
                            var homeScore = int.Parse(gameNode.SelectSingleNode(".//div[contains(@class,'event-main-scores')]/div[contains(@class,'team-home')]").InnerText);
                            var awayScore = int.Parse(gameNode.SelectSingleNode(".//div[contains(@class,'event-main-scores')]/div[contains(@class,'team-away')]").InnerText);
                            var leagueNameRaw = gameNode.SelectSingleNode(".//div[contains(@class,'event-tournament-info')]").InnerHtml;
                            var leagueName = tournamentNameRegex.Matches(leagueNameRaw)[0].Groups[1].Value;
                            Results res = new Results()
                            {
                                HomeTeam = homeTeam,
                                AwayTeam = awayTeam,
                                HomeScore = homeScore,
                                AwayScore = awayScore,
                                GameDate = dt,
                                GamePart = 1,
                                LeagueName = leagueName,
                                SportId = (int)link.Sport
                            };
                            results.Add(res);
                            
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return results;
        }
        public static void GetTeamLinksFromResultsTeams(List<Results> results)
        {
            TeamLinks = new List<Link>();
            foreach(var res in results)
            {
                var uri = new Uri(string.Format(_baseTeamUrl, res.HomeTeam.Replace(' ', '-')
                    , Utilities.Sport[res.SportId.Value], Utilities.Sport[res.SportId.Value]));
                TeamLinks.Add(new Link((SportEnum)res.SportId, uri));
                uri = new Uri(string.Format(_baseTeamUrl, res.AwayTeam.Replace(' ', '-')
                    , Utilities.Sport[res.SportId.Value], Utilities.Sport[res.SportId.Value]));
                TeamLinks.Add(new Link((SportEnum)res.SportId, uri));
            }
        }
        public static void DownloadTeamImages()
        {
            //todo : download images
        }
    }
}
