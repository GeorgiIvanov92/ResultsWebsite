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
                        teams.Add(teamFromDb);
                    }
                }
                if (teams.Count > 0)
                {                   
                    dbContext.Team.RemoveRange(teamsFromDb);
                    dbContext.Team.AddRange(teams);
                    dbContext.SaveChanges();
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
                            players.Add(playerFromDb);
                        }
                    }
                    dbContext.Player.RemoveRange(playersFromDb);
                    dbContext.Player.AddRange(players);

                    var champStats = lol.GetChampionStats(players);
                    var champsStatsFromDb = dbContext.ChampionStat.ToList();
                    dbContext.RemoveRange(champsStatsFromDb);
                    dbContext.AddRange(champStats);
                    dbContext.SaveChanges();


                    Console.WriteLine($"Finished Getting Players at {DateTime.Now.ToShortTimeString()}." +
                   $" {players.Count} players added to Db.");
                }
                Console.WriteLine($"Sampling Teams and Players again in {TeamsSamplePeriodInMinutes.Hours} hours and {TeamsSamplePeriodInMinutes.Minutes} minutes");
                Thread.Sleep(TeamsSamplePeriodInMinutes);
            }
        }
    }
}
