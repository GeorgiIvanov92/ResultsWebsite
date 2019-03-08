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
                GamesOfLegends lol = new GamesOfLegends();
                lol.GetLinks();
                var teams=lol.GetTeams();
                if (teams.Count > 0)
                {
                    dbContext.Team.RemoveRange(teamsFromDb);
                    dbContext.Team.AddRange(teams);
                    dbContext.SaveChanges();
                }
                Console.WriteLine($"Finished Getting Teams at {DateTime.Now.ToShortTimeString()}." +
                    $" {teams.Count} teams added to Db. " +
                    $"Sampling Teams again in {TeamsSamplePeriodInMinutes.Hours} hours and {TeamsSamplePeriodInMinutes.Minutes} minutes");
                Thread.Sleep(TeamsSamplePeriodInMinutes);
            }
        }
    }
}
