using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Tracker.Models;
using Tracker.Sites;

namespace Tracker.Workers
{
    class TeamsWorker
    {
        public static void TeamsWorkerInit(TimeSpan TeamsSamplePeriodInMinutes)
        {
            while (true)
            {
                TrackerDBContext dbContext = new TrackerDBContext();
                var teamsFromDb = dbContext.Team.ToList();
                var playersFromDb = dbContext.Player.ToList();
                GamesOfLegends lol = new GamesOfLegends();
                List<Player> players = new List<Player>();
                lol.GetLinks();
                var teams=lol.GetTeams();
                foreach(var teamFromDb in teamsFromDb)
                {
                    bool shouldAddTeam = true;
                    foreach(var team in teams)
                    {
                        if(team.Name == teamFromDb.Name)
                        {
                            shouldAddTeam = false;
                        }
                    }
                    if (shouldAddTeam)
                    {
                        teamFromDb.Id = null;
                        teams.Add(teamFromDb);
                    }
                }
                if (teams.Count > 0)
                {                   
                    dbContext.Team.RemoveRange(teamsFromDb);
                    dbContext.Team.AddRange(teams);
                    Console.WriteLine($"Finished Getting Teams at {DateTime.Now.ToShortTimeString()}." +
                   $" {teams.Count} teams added to Db.");

                    players = lol.GetPlayers(teams);
                    foreach (var playerFromDb in playersFromDb)
                    {
                        bool shouldAddPlayer = true;
                        foreach (var player in players)
                        {
                            if (player.Nickname == playerFromDb.Nickname)
                            {
                                shouldAddPlayer = false;
                            }
                        }
                        if (shouldAddPlayer)
                        {
                            playerFromDb.PlayerId = null;
                            players.Add(playerFromDb);
                        }
                    }
                    dbContext.Player.RemoveRange(playersFromDb);
                    var champsStatsFromDb = dbContext.ChampionStat.ToList();
                    dbContext.RemoveRange(champsStatsFromDb);
                    dbContext.SaveChanges();
                    dbContext.Player.AddRange(players);
                    dbContext.SaveChanges();

                    var champStats = lol.GetChampionStats();
                    List<ChampionStat> stats = new List<ChampionStat>();
                    dbContext.SaveChanges();
                    foreach (var champ in champStats) 
                    {
                        foreach (var stat in champ.Value)
                        {
                            foreach (var player in players)
                            {
                                if (champ.Key == player.Nickname)
                                {
                                    stat.PlayerId = player.PlayerId;
                                    stats.Add(stat);
                                }
                            }
                        }
                    }                   
                    dbContext.AddRange(stats);
                    dbContext.SaveChanges();


                     Console.WriteLine($"Finished Getting Players and Champion stats at {DateTime.Now.ToShortTimeString()}." +
                   $" {players.Count} players added to Db.");
                }
                Console.WriteLine($"Sampling Teams and Players again in {TeamsSamplePeriodInMinutes.Hours} hours and {TeamsSamplePeriodInMinutes.Minutes} minutes");
                Thread.Sleep(TeamsSamplePeriodInMinutes);
            }
        }
    }
}
