using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Tracker.Models;

namespace Tracker.Workers
{
    public static class PreliveWorker
    {
        public static void PreliveWorkerInit(TimeSpan PreliveSamplePeriod)
        {
            while (true)
            {
                while (EsportsLiveScore.LinksForLeagues==null || EsportsLiveScore.LinksForLeagues.Count == 0)
                {
                    Thread.Sleep(30000);
                }
                TrackerDBContext dbContext = new TrackerDBContext();
                var preliveEvents =EsportsLiveScore.GetPreliveEvents();
                var filteredEvents = Utilities.FilterAlreadySentEvents(dbContext, preliveEvents);
                dbContext.Prelive.AddRange(filteredEvents);
                dbContext.SaveChanges();
                Console.WriteLine($"Finished Getting Prelive Events at {DateTime.Now.ToShortTimeString()}." +
                    $" {filteredEvents.Count} events added to Db. " +
                    $"Sampling Prelive again in {PreliveSamplePeriod.Hours} hours and {PreliveSamplePeriod.Minutes} minutes");
                Thread.Sleep(PreliveSamplePeriod);
            }
        }
    }
}
