using HtmlAgilityPack;
using RabbitMQ.TrackerEssentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tracker.Models;
using static RabbitMQ.TrackerEssentials.Communication.Sports;

namespace Tracker.Sites
{
    class GamesOfLegends : Site
    {
        private static readonly string _allTeamsLink = "http://gol.gg/teams/list/season-S9/split-Summer/region-ALL/tournament-ALL/week-ALL/";
        private static readonly string _teamsUrl = "http://gol.gg/teams/";
        private static List<Link> PlayerLinks = new List<Link>();
        private Dictionary<string,List<ChampionStat>> ChampionStats;
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
                        Link link = new Link(SportEnum.LeagueOfLegends,uri);
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
                    foreach (var table in tables)
                    {
                        if (table.InnerText.Contains($"{teamName} - S9"))
                        {
                            var columns = table.SelectNodes(".//td");
                            for (int i = 0; i < columns.Count - 1; i += 2)
                            {
                                if (columns[i].InnerText.Contains("Region") && columns[i].InnerText != "-")
                                {
                                    team.Region = columns[i + 1].InnerText;
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Win Rate") && columns[i].InnerText != "-")
                                {
                                    var winrateArr = columns[i + 1].InnerText.Replace("W", "").Replace("L", "").Split(" - ");
                                    var wins = int.Parse(winrateArr[0].Trim());
                                    var losses = int.Parse(winrateArr[1].Trim());
                                    var totalGames = wins + losses;
                                    var winrate = (int)(((double)wins / (double)totalGames) * 100);
                                    team.Winrate = winrate;
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Average game duration") && columns[i].InnerText != "-")
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
                                if (columns[i].InnerText.Contains("Gold Per Minute") && columns[i+1].InnerText != "-")
                                {
                                    team.GoldPerMinute = int.Parse(columns[i + 1].InnerText);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Gold Differential per Minute") && columns[i+1].InnerText != "-")
                                {
                                    team.GoldDifferencePerMinute = int.Parse(columns[i + 1].InnerText);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Gold Differential at 15 min") && columns[i+1].InnerText != "-")
                                {
                                    team.GoldDifferenceAt15 = int.Parse(columns[i + 1].InnerText);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("CS Per Minute") && columns[i+1].InnerText != "-")
                                {
                                    team.CSPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("CS Differential at 15 min") && columns[i+1].InnerText != "-")
                                {
                                    team.CSDifferenceAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Tower Differential at 15 min") && columns[i+1].InnerText != "-")
                                {
                                    team.TowerDifferenceAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("First Tower") && columns[i+1].InnerText != "-")
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
                                if (columns[i].InnerText.Contains("Damage Per Minute") && columns[i + 1].InnerText != "-")
                                {
                                    team.DamagePerMinute = int.Parse(columns[i + 1].InnerText);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("First Blood") && columns[i + 1].InnerText != "-")
                                {
                                    var percentString = columns[i + 1].InnerText.Replace("%", "").Replace(".", ",").Trim().Replace("&nbsp;\n", "").Trim();
                                    team.FirstBloodPercent = (int)(float.Parse(percentString));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Kills Per Game") && columns[i + 1].InnerText != "-")
                                {
                                    team.KillsPerGame = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Deaths Per Game") && columns[i + 1].InnerText != "-")
                                {
                                    team.DeathsPerGame = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Kill / Death Ratio") && columns[i + 1].InnerText != "-")
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
                                if (columns[i].InnerText.Contains("Wards Per Minute") && columns[i + 1].InnerText != "-")
                                {
                                    team.WardsPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Vision Wards Per Minute") && columns[i + 1].InnerText != "-")
                                {
                                    team.VisionWardsPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Wards Cleared Per Minute") && columns[i + 1].InnerText != "-")
                                {
                                    team.WardsClearedPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("% Wards Cleared") && columns[i + 1].InnerText != "-")
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
                                if (columns[i].InnerText.Contains("Dragons / game") && columns[i + 1].InnerText != "-")
                                {
                                    team.DragonsPerGame = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Split('(')[0]);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Dragons at 15 min") && columns[i + 1].InnerText != "-")
                                {
                                    team.DragonsAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Herald / game") && columns[i + 1].InnerText != "-")
                                {
                                    team.HeraldPerGame = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Split('(')[0]);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Nashors / game") && columns[i + 1].InnerText != "-")
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
                                var emptyString = killedDragons.FirstOrDefault(dr => string.IsNullOrEmpty(dr));
                                if(emptyString != null)
                                {
                                    continue;
                                }
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
                                    Link link = new Link(SportEnum.LeagueOfLegends, uri, team.Name);
                                    PlayerLinks.Add(link);
                                }
                            }
                        }
                    }
                    teams.Add(team);

                }
                catch (Exception ex)
                {
                    var dt = DateTime.UtcNow;
                    while (dt.AddSeconds(10) > DateTime.UtcNow)
                    {

                    }
                }

            };
            return teams;
        }
        public List<Player> GetPlayers(List<Team> teams)
        {
            List<Player> players = new List<Player>();
            ChampionStats = new Dictionary<string, List<ChampionStat>>();
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
                    if (player.TeamId == null)
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
                                        if (columns[i].InnerText.Contains("Record") && columns[i + 1].InnerText != "-")
                                        {
                                            var winrateArr = columns[i + 1].InnerText.Replace("W", "").Replace("L", "").Split(" - ");
                                            var wins = int.Parse(winrateArr[0].Trim());
                                            var losses = int.Parse(winrateArr[1].Trim());
                                            player.Wins = wins;
                                            player.Losses = losses;
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("KDA") && columns[i+1].InnerText != "-")
                                        {
                                            try
                                            {
                                                
                                                player.KDA = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            }
                                            catch
                                            {

                                            }
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("CS per Minute") && columns[i + 1].InnerText != "-")
                                        {
                                            player.CSPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Gold Per Minute") && columns[i + 1].InnerText != "-")
                                        {
                                            player.GoldPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Gold%") && columns[i + 1].InnerText != "-")
                                        {
                                            player.GoldPercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Kill Participation") && columns[i + 1].InnerText != "-")
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
                                        if (columns[i].InnerText.Contains("Ahead in CS at 15 min") && columns[i+1].InnerText != "-")
                                        {
                                            var percentString = columns[i + 1].InnerText.Replace("%", "").Replace(".", ",").Trim().Replace("&nbsp;\n", "").Trim();
                                            player.AheadInCSAt15Percent = float.Parse(percentString);
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("CS Differential at 15 min") && columns[i + 1].InnerText != "-")
                                        {
                                            player.CSDifferenceAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Gold Differential at 15 min") && columns[i + 1].InnerText != "-")
                                        {
                                            player.GoldDifferenceAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("XP Differential at 15 min") && columns[i + 1].InnerText != "-")
                                        {
                                            player.XPDifferenceAt15 = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("First Blood Participation") && columns[i + 1].InnerText != "-")
                                        {
                                            player.FirstBloodParticipationPercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("First Blood Victim") && columns[i + 1].InnerText != "-")
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
                                        if (columns[i].InnerText.Contains("Damage Per Minute") && columns[i + 1].InnerText != "-")
                                        {
                                            player.DamagePerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Damage%") && columns[i + 1].InnerText != "-")
                                        {
                                            player.DamagePercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("K+A Per Minute") && columns[i + 1].InnerText != "-")
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
                                        if (columns[i].InnerText.Contains("Pentakills") && columns[i + 1].InnerText != "-")
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
                                        if (columns[i].InnerText.Contains("Vision score Per Minute") && columns[i + 1].InnerText != "-")
                                        {
                                            player.VisionScorePerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Equals("Ward Per Minute: ") && columns[i + 1].InnerText != "-")
                                        {
                                            player.WardPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Vision Ward Per Minute") && columns[i + 1].InnerText != "-")
                                        {
                                            player.VisionWardsPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                        if (columns[i].InnerText.Contains("Ward Cleared Per Minute") && columns[i + 1].InnerText != "-")
                                        {
                                            player.WardsClearedPerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                            continue;
                                        }
                                    }
                                }));
                            }
                            if (table.InnerText.Contains("Champion") && !table.InnerText.Contains("Score"))
                            {
                                var columns = table.SelectNodes(".//td");
                                tasks.Add(Task.Factory.StartNew(() =>
                                {
                                    for (int i = 0; i < columns.Count - 5; i += 4)
                                    {
                                        ChampionStat stat = new ChampionStat();
                                        stat.ChampionName = columns[i].InnerText;
                                        stat.GamesPlayed = int.Parse(columns[i + 1].InnerText);
                                        stat.WinratePercent = float.Parse(columns[i + 2].InnerText.Replace(".", ",").Replace("%", "").Replace("&nbsp;\n", "").Trim());
                                        stat.WinratePercent = Math.Round(stat.WinratePercent, 2);
                                        if(columns[i+3].InnerText == "-")
                                        {
                                            continue;
                                        }
                                        stat.KDA = float.Parse(columns[i + 3].InnerText.Replace(".", ",").Trim());
                                        stat.KDA = Math.Round(stat.KDA, 2);
                                        if (ChampionStats.ContainsKey(player.Nickname))
                                        {
                                            ChampionStats[player.Nickname].Add(stat);
                                        }
                                        else
                                        {
                                            ChampionStats.Add(player.Nickname, new List<ChampionStat>() { stat });
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
        public Dictionary<string,List<ChampionStat>> GetChampionStats()
        {
            return ChampionStats;          
        }

    }
}
