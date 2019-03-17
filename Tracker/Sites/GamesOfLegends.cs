using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tracker.Models;
using Tracker.TrackerEssentials;

namespace Tracker.Sites
{
    class GamesOfLegends : Site
    {
        private static readonly string _allTeamsLink = "http://gol.gg/teams/list/season-S9/split-Spring/region-ALL/tournament-ALL/week-ALL/";
        private static readonly string _teamsUrl = "http://gol.gg/teams/";
        private static List<Link> PlayerLinks = new List<Link>();
        private Regex scriptResultsRegex = new Regex(@"data : \[([\d,]+)]", RegexOptions.Compiled | RegexOptions.Multiline);
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
            Parallel.ForEach(links, (teamLink) =>
            {
                try
                {
                    var responseString = client.GetStringAsync(teamLink.Uri).Result;
                    if (responseString == null)
                    {
                        return;
                    }
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(responseString);
                    var teamName = doc.DocumentNode.SelectSingleNode("//h1[contains(@class,'panel-title')]")?.InnerText;
                    if (teamName == null)
                    {
                        return;
                    }
                    Team team = new Team()
                    {
                        SportId = 1,
                        Name = teamName,
                    };
                    var tables = doc.DocumentNode.SelectNodes("//table[contains(@class,'table_list')]");
                    foreach (var table in tables)
                    {
                        if (table.InnerText.Contains($"{teamName} - S9"))
                        {
                            var columns = table.SelectNodes(".//td");
                            for (int i = 0; i < columns.Count - 1; i += 2)
                            {
                                if (columns[i].InnerText.Contains("Region"))
                                {
                                    team.Region = columns[i + 1].InnerText;
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Win Rate"))
                                {
                                    var winrateArr = columns[i + 1].InnerText.Replace("W", "").Replace("L", "").Split(" - ");
                                    var wins = int.Parse(winrateArr[0].Trim());
                                    var losses = int.Parse(winrateArr[1].Trim());
                                    var totalGames = wins + losses;
                                    var winrate = (int)(((double)wins / (double)totalGames) * 100);
                                    team.Winrate = winrate;
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Average game duration"))
                                {
                                    var timeString = columns[i + 1].InnerText.Replace(":", ",");
                                    var time = float.Parse(timeString);
                                    team.AverageGameTime = (float)time;
                                    continue;
                                }
                            }
                            continue;
                        }
                        if (table.InnerText.Contains("Economy"))
                        {
                            var columns = table.SelectNodes(".//td");
                            for (int i = 0; i < columns.Count - 1; i += 2)
                            {
                                if (columns[i].InnerText.Contains("Gold Per Minute"))
                                {
                                    team.GoldPerMinute = int.Parse(columns[i + 1].InnerText);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Gold Differential per Minute"))
                                {
                                    team.GoldDifferencePerMinute = int.Parse(columns[i + 1].InnerText);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Gold Differential at 15 min"))
                                {
                                    team.GoldDifferenceAt15 = int.Parse(columns[i + 1].InnerText);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("CS Per Minute"))
                                {
                                    team.CSPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("CS Differential at 15 min"))
                                {
                                    team.CSDifferenceAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Tower Differential at 15 min"))
                                {
                                    team.TowerDifferenceAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("First Tower"))
                                {
                                    var percentString = columns[i + 1].InnerText.Replace("%", "").Replace(".", ",").Trim().Replace("&nbsp;\n", "").Trim();
                                    team.FirstTowerPercent = (int)(float.Parse(percentString));
                                    continue;
                                }
                            }
                        }
                        if (table.InnerText.Contains("Aggression"))
                        {
                            var columns = table.SelectNodes(".//td");
                            for (int i = 0; i < columns.Count - 1; i += 2)
                            {
                                if (columns[i].InnerText.Contains("Damage Per Minute"))
                                {
                                    team.DamagePerMinute = int.Parse(columns[i + 1].InnerText);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("First Blood"))
                                {
                                    var percentString = columns[i + 1].InnerText.Replace("%", "").Replace(".", ",").Trim().Replace("&nbsp;\n", "").Trim();
                                    team.FirstBloodPercent = (int)(float.Parse(percentString));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Kills Per Game"))
                                {
                                    team.KillsPerGame = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Deaths Per Game"))
                                {
                                    team.DeathsPerGame = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Kill / Death Ratio"))
                                {
                                    team.KDRatio = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }

                            }
                        }
                        if (table.InnerText.Contains("Vision"))
                        {
                            var columns = table.SelectNodes(".//td");
                            for (int i = 0; i < columns.Count - 1; i += 2)
                            {
                                if (columns[i].InnerText.Contains("Wards Per Minute"))
                                {
                                    team.WardsPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Vision Wards Per Minute"))
                                {
                                    team.VisionWardsPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Wards Cleared Per Minute"))
                                {
                                    team.WardsClearedPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("% Wards Cleared"))
                                {
                                    var percentString = columns[i + 1].InnerText.Replace("%", "").Replace(".", ",").Trim().Replace("&nbsp;\n", "").Trim();
                                    team.WardsClearedPercent = (int)(float.Parse(percentString));
                                    continue;
                                }
                            }
                        }
                        if (table.InnerText.Contains("Objectives"))
                        {
                            var columns = table.SelectNodes(".//td");
                            for (int i = 0; i < columns.Count - 1; i += 2)
                            {
                                if (columns[i].InnerText.Contains("Dragons / game"))
                                {
                                    team.DragonsPerGame = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Split('(')[0]);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Dragons at 15 min"))
                                {
                                    team.DragonsAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Herald / game"))
                                {
                                    team.HeraldPerGame = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Split('(')[0]);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Nashors / game"))
                                {
                                    team.NashorsPerGame = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Split('(')[0]);
                                    continue;
                                }
                            }
                        }
                        if (table.InnerText.Contains("Wins/Losses by side"))
                        {
                            var script = table.SelectSingleNode(".//script")?.InnerHtml;
                            if (scriptResultsRegex.Matches(script).Count == 2)
                            {
                                var wins = scriptResultsRegex.Matches(script)[0]?.Groups[1].Value.Split(',');
                                var losses = scriptResultsRegex.Matches(script)[1]?.Groups[1].Value.Split(',');
                                team.BlueSideWins = int.Parse(wins[0]);
                                team.RedSideWins = int.Parse(wins[1]);
                                team.BlueSideLosses = int.Parse(losses[0]);
                                team.RedSideLosses = int.Parse(losses[1]);
                            }
                        }
                        if (table.InnerText.Contains("Dragon priority"))
                        {
                            var script = table.SelectSingleNode(".//script")?.InnerHtml;
                            if (scriptResultsRegex.Matches(script).Count == 2)
                            {
                                var killedDragons = scriptResultsRegex.Matches(script)[0]?.Groups[1].Value.Split(',');
                                var lostDragons = scriptResultsRegex.Matches(script)[1]?.Groups[1].Value.Split(',');
                                team.CloudDrakesKilled = int.Parse(killedDragons[0]);
                                team.OceanDrakesKilled = int.Parse(killedDragons[1]);
                                team.InfernalDrakesKilled = int.Parse(killedDragons[2]);
                                team.MountainDrakesKilled = int.Parse(killedDragons[3]);
                                team.ElderDrakesKilled = int.Parse(killedDragons[4]);

                                team.CloudDrakesLost = int.Parse(lostDragons[0]);
                                team.OceanDrakesLost = int.Parse(lostDragons[1]);
                                team.InfernalDrakesLost = int.Parse(lostDragons[2]);
                                team.MountainDrakesLost = int.Parse(lostDragons[3]);
                                team.ElderDrakesLost = int.Parse(lostDragons[4]);
                            }
                        }
                        if (table.InnerText.Contains("Player"))
                        {
                            var linkNodes = table.SelectNodes(".//a");
                            foreach (var node in linkNodes)
                            {
                                if (node.OuterHtml.Contains("player-stats"))
                                {
                                    var uriString = node.Attributes["href"].Value;
                                    uriString = uriString.Substring(2);
                                    var uri = new Uri("http://gol.gg" + uriString);
                                    Link link = new Link(TrackerEssentials.Communication.Sports.SportEnum.LeagueOfLegends, uri, team.Name);
                                    PlayerLinks.Add(link);
                                }
                            }
                        }
                    }
                    teams.Add(team);

                }
                catch (Exception ex)
                {
                    return;
                }

            });
            return teams;
        }
        public List<Player> GetPlayers(TrackerDBContext db)
        {
            List<Player> players = new List<Player>();
            List<Team> teams = db.Team.ToList();
            foreach (var playerLink in PlayerLinks)
            {
                try
                {
                    var responseString = client.GetStringAsync(playerLink.Uri).Result;
                    if (responseString == null)
                    {
                        continue;
                    }
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(responseString);
                    var playerName = doc.DocumentNode.SelectSingleNode("//h1[contains(@class,'panel-title')]")?.InnerText;
                    if (playerName == null)
                    {
                        continue;
                    }
                    Player player = new Player()
                    {
                        SportId = 1,
                        Nickname = playerName,
                    };
                    foreach(var pl in players)
                    {
                        if(pl.Nickname == player.Nickname)
                        {
                            continue;
                        }
                    }
                    foreach (var team in teams)
                    {
                        if (team.Name == playerLink.AdditionalData)
                        {
                            player.TeamId = team.Id;
                            break;
                        }
                    }
                    if (player.TeamId == 0)
                    {
                        continue;
                    }
                    var tasks = new List<Task>();
                    var tables = doc.DocumentNode.SelectNodes("//table[contains(@class,'table_list')]");
                    foreach (var table in tables)
                    {
                        try
                        {
                            if (table.InnerText.Contains("General stats"))
                            {
                                var columns = table.SelectNodes(".//td");
                                tasks.Add(Task.Factory.StartNew(() =>
                                {
                                    for (int i = 0; i < columns.Count - 1; i += 2)
                                    {
                                        if (columns[i].InnerText.Contains("Record"))
                                        {
                                            var winrateArr = columns[i + 1].InnerText.Replace("W", "").Replace("L", "").Split(" - ");
                                            var wins = int.Parse(winrateArr[0].Trim());
                                            var losses = int.Parse(winrateArr[1].Trim());
                                            player.Wins = wins;
                                            player.Losses = losses;
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("KDA"))
                                        {
                                            player.KDA = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("CS per Minute"))
                                        {
                                            player.CSPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Gold Per Minute"))
                                        {
                                            player.GoldPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Gold%"))
                                        {
                                            player.GoldPercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Kill Participation"))
                                        {
                                            player.KillParticipation = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                            continue;
                                        }
                                    }
                                }));
                            }
                            if (table.InnerText.Contains("Early game"))
                            {
                                var columns = table.SelectNodes(".//td");
                                tasks.Add(Task.Factory.StartNew(() =>
                                {
                                    for (int i = 0; i < columns.Count - 1; i += 2)
                                    {
                                        if (columns[i].InnerText.Contains("Ahead in CS at 15 min"))
                                        {
                                            var percentString = columns[i + 1].InnerText.Replace("%", "").Replace(".", ",").Trim().Replace("&nbsp;\n", "").Trim();
                                            player.AheadInCSAt15Percent = float.Parse(percentString);
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("CS Differential at 15 min"))
                                        {
                                            player.CSDifferenceAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Gold Differential at 15 min"))
                                        {
                                            player.GoldDifferenceAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("XP Differential at 15 min"))
                                        {
                                            player.XPDifferenceAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("First Blood Participation"))
                                        {
                                            player.FirstBloodParticipationPercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("First Blood Victim"))
                                        {
                                            player.FirstBloodVictimPercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                            continue;
                                        }

                                    }
                                }));
                            }
                            if (table.InnerText.Contains("Aggression"))
                            {
                                var columns = table.SelectNodes(".//td");
                                tasks.Add(Task.Factory.StartNew(() =>
                                {
                                    for (int i = 0; i < columns.Count - 1; i += 2)
                                    {
                                        if (columns[i].InnerText.Contains("Damage Per Minute"))
                                        {
                                            player.DamagePerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Damage%"))
                                        {
                                            player.DamagePercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("K+A Per Minute"))
                                        {
                                            player.KillsAndAssistsPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Solo kills"))
                                        {
                                            if (columns[i + 1].InnerText != "-")
                                            {
                                                player.SoloKills = int.Parse(columns[i + 1].InnerText);
                                            }
                                            else
                                            {
                                                player.SoloKills = 0;
                                            }
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Pentakills"))
                                        {
                                            player.Pentakills = int.Parse(columns[i + 1].InnerText);
                                            continue;
                                        }
                                    }
                                }));
                            }
                            if (table.InnerText.Contains("Vision"))
                            {
                                var columns = table.SelectNodes(".//td");
                                tasks.Add(Task.Factory.StartNew(() =>
                                {
                                    for (int i = 0; i < columns.Count - 1; i += 2)
                                    {
                                        if (columns[i].InnerText.Contains("Vision score Per Minute"))
                                        {
                                            player.VisionScorePerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Equals("Ward Per Minute: "))
                                        {
                                            player.WardPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Vision Ward Per Minute"))
                                        {
                                            player.VisionWardsPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Ward Cleared Per Minute"))
                                        {
                                            player.WardsClearedPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                    }
                                }));
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                    }
                    Task.WaitAll(tasks.ToArray());
                    players.Add(player);
                }
                catch (Exception ex)
                {
                    var dt = DateTime.UtcNow;
                    while (dt.AddSeconds(10) > DateTime.UtcNow)
                    {

                    }
                }
            }
            return players;

        }
         
    }
}
