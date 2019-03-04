using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker.TrackerEssentials;

namespace Tracker.Sites
{
    class GamesOfLegends : Site
    {
        private static readonly string _allTeamsLink = "http://gol.gg/teams/list/season-S9/split-Spring/region-ALL/tournament-ALL/week-ALL/";
        private static readonly string _teamsUrl = "http://gol.gg/teams/";
        private static List<Link> PlayerLinks = new List<Link>();
        public override void GetLinks()
        {
            var responseString=client.GetStringAsync(_allTeamsLink).Result;
            if(responseString == null)
            {
                return;
            }
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(responseString);
            var teams = doc.DocumentNode.SelectSingleNode("//table[contains(@class,'table_list')]")?.SelectNodes(".//tr");
            foreach(var team in teams)
            {
                try
                {
                    var teamLink = team.SelectSingleNode(".//a/@href")?.Attributes["href"]?.Value;
                    if(teamLink != null)
                    {
                        Uri uri = new Uri(_teamsUrl + teamLink);
                        Link link = new Link(TrackerEssentials.Communication.Sports.SportEnum.LeagueOfLegends,uri);
                        links.Add(link);
                    }
                    
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }
        public void GetTeams()
        {

        }
         
    }
}
