using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using Tracker.Models;
using Tracker.TrackerEssentials;

namespace Tracker
{
    public static class EsportsLiveScore
    {
        private static HttpClient client;
        public static List<Link> Links;
        private static string _baseUrl = "http://www.esportlivescore.com/";
        private static readonly string format = "dd/MM hh:mm";
        public static void GetNewLinks()
        {
            Links = new List<Link>();
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
                    Links.Add(link);
                }else if (url.Contains("leagueoflegends"))
                {
                    link = new Link(TrackerEssentials.Communication.Sports.SportEnum.LeagueOfLegends, new Uri(_baseUrl + url));
                    Links.Add(link);
                }else if (url.Contains("dota"))
                {
                    link = new Link(TrackerEssentials.Communication.Sports.SportEnum.Dota2, new Uri(_baseUrl + url));
                    Links.Add(link);
                }
            }
        }
        public static List<Results> GetResultEvents()
        {
            List<Results> results = new List<Results>();
            foreach(var link in Links)
            {
                var page = client.GetStringAsync(link.Uri).Result;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(page);
                var finishedMatchesNodes = doc.DocumentNode.SelectSingleNode("//div[contains(@id,'finished')]").SelectNodes(".//div[contains(@id,'event_id')]");
                foreach(var gameNode in finishedMatchesNodes)
                {
                    var dateString = gameNode.SelectSingleNode(".//div[contains(@class,'event_date_day_month')]").InnerText.Trim();
                    var hourAndMinute = gameNode.SelectSingleNode(".//div[contains(@class,'event_date_hour_minutes')]").InnerText.Trim();
                    var entireString = $"{dateString} {hourAndMinute}";
                    DateTime dt;

                    if(!DateTime.TryParseExact(entireString, format,new CultureInfo("en-Us"), DateTimeStyles.AdjustToUniversal, out dt))
                    {

                    }
                }
            }
            return results;
        }
    }
}
