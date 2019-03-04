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
        public static void TeamsWorkerInit(TimeSpan PreliveSamplePeriod)
        {
            while (true)
            {
                TrackerDBContext dbContext = new TrackerDBContext();
                GamesOfLegends lol = new GamesOfLegends();
                lol.GetLinks();
                lol.GetTeams();
                //dbContext.Prelive.AddRange(filteredEvents);
                //dbContext.SaveChanges();
                Console.WriteLine($"Finished Getting Prelive Events at {DateTime.Now.ToShortTimeString()}." +
                    $"  events added to Db. " +
                    $"Sampling Prelive again in {PreliveSamplePeriod.Hours} hours and {PreliveSamplePeriod.Minutes} minutes");
                Thread.Sleep(PreliveSamplePeriod);
            }
        }
    }
}
