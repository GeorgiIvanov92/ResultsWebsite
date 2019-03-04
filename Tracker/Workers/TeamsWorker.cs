using System;
using System.Collections.Generic;
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
                GamesOfLegends lol = new GamesOfLegends();
                lol.GetLinks();
                var teams=lol.GetTeams();
                dbContext.Team.AddRange(teams);
                dbContext.SaveChanges();
                Console.WriteLine($"Finished Getting Teams at {DateTime.Now.ToShortTimeString()}." +
                    $" {teams.Count} teams added to Db. " +
                    $"Sampling Teams again in {TeamsSamplePeriodInMinutes.Hours} hours and {TeamsSamplePeriodInMinutes.Minutes} minutes");
                Thread.Sleep(TeamsSamplePeriodInMinutes);
            }
        }
    }
}
