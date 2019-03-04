using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker.Models;
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
        public List<Team> GetTeams()
        {
            List<Team> teams = new List<Team>();
            foreach(var teamLink in links)
            {
                try
                {
                    var responseString = client.GetStringAsync(teamLink.Uri).Result;
                    if (responseString == null)
                    {
                        continue;
                    }
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(responseString);
                    var teamName = doc.DocumentNode.SelectSingleNode("//h1[contains(@class,'panel-title')]")?.InnerText;
                    if (teamName == null)
                    {
                        continue;
                    }
                    Team team = new Team()
                    {
                        SportId = 1,
                        Name = teamName,
                    };
                    var tables = doc.DocumentNode.SelectNodes("//table[contains(@class,'table_list')]");
                    foreach(var table in tables)
                    {
                        if (table.InnerText.Contains($"{teamName} - S9"))
                        {
                            var columns = table.SelectNodes(".//td");
                            for(int i=0; i<columns.Count-1; i+=2)
                            {
                                if (columns[i].InnerText.Contains("Region"))
                                {
                                    team.Region = columns[i + 1].InnerText;
                                    continue;
                                }
                                if(columns[i].InnerText.Contains("Win Rate"))
                                {
                                    var winrateArr = columns[i + 1].InnerText.Replace("W","").Replace("L","").Split(" - ");
                                    var wins = int.Parse(winrateArr[0].Trim());
                                    var losses = int.Parse(winrateArr[1].Trim());
                                    var totalGames = wins + losses;
                                    var winrate = (int)(((double)wins / (double)totalGames)*100);
                                    team.Winrate = winrate;
                                    continue;
                                }
                                if(columns[i].InnerText.Contains("Average game duration"))
                                {
                                    var timeString = columns[i + 1].InnerText.Replace(":", ",");
                                    var time = float.Parse(timeString);
                                    team.AverageGameTime = (float)time;
                                }
                            }
                        }
                    }
                    teams.Add(team);

                }
                catch (Exception ex)
                {
                    continue;
                }

            }
            return teams;
        }
         
    }
}
