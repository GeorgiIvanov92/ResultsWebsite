using HtmlAgilityPack;
using RabbitMQ.TrackerEssentials;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tracker.Models;
using static RabbitMQ.TrackerEssentials.Communication.Sports;

namespace Tracker
{
    public static class EsportsLiveScore
    {
        private static HttpClient HttpClient;
        public static List<Link> LinksForLeagues;
        private static string _baseUrl = "http://www.esportlivescore.com/";
        private static readonly string format = "dd/MM HH:mm";
        private static readonly Regex tournamentNameRegex = new Regex(@"title=""(.*?)""", RegexOptions.Compiled | RegexOptions.Multiline);
        public static List<Link> TeamLinks = new List<Link>();
        public static bool HaveGatheredLinks = false;
        public static void GetNewLinks()
        {
            HaveGatheredLinks = false;
            LinksForLeagues = new List<Link>();
            if (HttpClient == null)
            {
                HttpClient = new HttpClient();
            }
            var page = HttpClient.GetStringAsync(_baseUrl).Result;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);
            var linkNodes = doc.DocumentNode.SelectSingleNode("//ul[contains(@id,'games')]").SelectNodes(".//li/a");
            foreach(var node in linkNodes)
            {              
                var url = node.Attributes["href"].Value;
                Link link;
                if (url.Contains("csgo"))
                {
                    link = new Link(SportEnum.CounterStrike, new Uri(_baseUrl+url));
                    LinksForLeagues.Add(link);
                }else if (url.Contains("leagueoflegends"))
                {
                    link = new Link(SportEnum.LeagueOfLegends, new Uri(_baseUrl + url));
                    LinksForLeagues.Add(link);
                }else if (url.Contains("dota"))
                {
                    link = new Link(SportEnum.Dota2, new Uri(_baseUrl + url));
                    LinksForLeagues.Add(link);
                }
            }
        }
        public static List<Results> GetResultEvents()
        {
            List<Results> results = new List<Results>();
            foreach(var link in LinksForLeagues)
            {
                try
                {
                    var page = HttpClient.GetStringAsync(link.Uri).Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    var finishedMatchesNodes = doc.DocumentNode.SelectSingleNode("//div[contains(@id,'finished')]").SelectNodes(".//div[contains(@id,'event_id')]");
                    if(finishedMatchesNodes == null)
                    {
                        continue;
                    }
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
                            if (homeTeam.EndsWith("6"))
                            {
                                homeTeam = homeTeam.Substring(0, homeTeam.Length - 1).Trim();
                            }
                            if (awayTeam.EndsWith("6"))
                            {
                                awayTeam = awayTeam.Substring(0, awayTeam.Length - 1).Trim();
                            }
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
                            var homeTeamLink = gameNode.SelectSingleNode(".//div[contains(@class,'team-home')]//div[contains(@class,'event-team-info')]/a")?.Attributes["href"].Value;
                            if (homeTeamLink != null)
                            {
                                TeamLinks.Add(new Link((SportEnum)res.SportId, new Uri(_baseUrl + homeTeamLink), $"{res.HomeTeam}"));
                            }
                            var awayTeamLink = gameNode.SelectSingleNode(".//div[contains(@class,'team-away')]//div[contains(@class,'event-team-info')]/a")?.Attributes["href"].Value;
                            if (awayTeamLink != null)
                            {
                                TeamLinks.Add(new Link((SportEnum)res.SportId, new Uri(_baseUrl + awayTeamLink),$"{res.AwayTeam}"));
                            }

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
            HaveGatheredLinks = true;
            return results;
        }
        public static void WriteTeamIconsToDisk()
        {
            FileInfo file = new FileInfo(ConfigurationManager.AppSettings["ImagesSavePath"]);
            file.Directory.Create();
            List<Link> tempTeamLinks = new List<Link>();
            TeamLinks.ForEach(link => tempTeamLinks.Add(link));

            foreach (var team in tempTeamLinks)
            {
                try
                {
                    var pageString = HttpClient.GetStringAsync(team.Uri).Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(pageString);
                    var imgPath = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'table-container')]//div[contains(@class,'logo')]//img");
                    var stringUrl = imgPath?.Attributes["src"].Value;
                    var imageByteArray = HttpClient.GetByteArrayAsync(stringUrl).Result;
                    MemoryStream ms = new MemoryStream(imageByteArray);
                    ms.Write(imageByteArray);
                    FileStream fileStream = File.Create(ConfigurationManager.AppSettings["ImagesSavePath"] + team.AdditionalData + ".png");
                    fileStream.Write(imageByteArray);
                    fileStream.Close();
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }
        public static List<Prelive> GetPreliveEvents()
        {
            List<Prelive> preliveEvents = new List<Prelive>();
            Parallel.ForEach(LinksForLeagues, link =>
            {
                {
                    try
                    {
                        var page = HttpClient.GetStringAsync(link.Uri).Result;
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(page);
                        var league = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'page-description tournament-description')]//h1").InnerText;
                        var preliveNodes = doc.DocumentNode.SelectSingleNode("//div[contains(@id,'upcoming')]")
                            .SelectNodes(".//div[contains(@id,'event_id_')]");
                        foreach (var node in preliveNodes)
                        {
                            Prelive prelive = new Prelive();
                            var tournamentInfoNode = node.SelectSingleNode(".//div[contains(@class,'event-tournament-info')]");
                            var bestOf = tournamentInfoNode.InnerText;
                            if (bestOf != null && bestOf.Contains("BO"))
                            {
                                prelive.BestOf = int.Parse(bestOf.Replace("BO", ""));

                            }
                            prelive.LeagueName = league;
                            prelive.SportId = (int)link.Sport;
                            var homeTeam = node.SelectSingleNode(".//div[contains(@class,'team-home')]").InnerText.Trim();
                            var awayTeam = node.SelectSingleNode(".//div[contains(@class,'team-away')]").InnerText.Trim();
                            if (homeTeam != null && awayTeam != null)
                            {
                                prelive.HomeTeam = homeTeam;
                                prelive.AwayTeam = awayTeam;
                            }
                            var dateString = node.SelectSingleNode(".//div[contains(@class,'event_date_day_month')]").InnerText.Trim();
                            var hourAndMinute = node.SelectSingleNode(".//div[contains(@class,'event_date_hour_minutes')]").InnerText.Trim();
                            var entireString = $"{dateString} {hourAndMinute}";
                            DateTime dt;

                            if (!DateTime.TryParseExact(entireString, format, new CultureInfo("en-Us"), DateTimeStyles.AdjustToUniversal, out dt))
                            {

                            }
                            prelive.GameDate = dt;
                            preliveEvents.Add(prelive);

                        }
                    }
                    catch (Exception ex)
                    {
                        return;
                    }
                }
            });
            return preliveEvents;
        }
    }
}
