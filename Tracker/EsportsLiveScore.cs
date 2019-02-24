using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
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
        private static HttpClient HttpClient;
        public static List<Link> ResultsLinks;
        private static string _baseUrl = "http://www.esportlivescore.com/";
        private static readonly string format = "dd/MM HH:mm";
        private static readonly Regex tournamentNameRegex = new Regex(@"title=""(.*?)""", RegexOptions.Compiled | RegexOptions.Multiline);
        private static List<Link> TeamLinks = new List<Link>();
        public static void GetNewLinks()
        {
            ResultsLinks = new List<Link>();
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
                    ResultsLinks.Add(link);
                }else if (url.Contains("leagueoflegends"))
                {
                    link = new Link(SportEnum.LeagueOfLegends, new Uri(_baseUrl + url));
                    ResultsLinks.Add(link);
                }else if (url.Contains("dota"))
                {
                    link = new Link(SportEnum.Dota2, new Uri(_baseUrl + url));
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
                    var page = HttpClient.GetStringAsync(link.Uri).Result;
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
            return results;
        }
        public static void WriteTeamIconsToDisk()
        {
            foreach(var team in TeamLinks)
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
        public static void DownloadTeamImages()
        {
            //todo : download images
        }
    }
}
