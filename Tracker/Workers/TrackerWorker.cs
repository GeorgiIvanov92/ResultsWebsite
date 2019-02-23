using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Tracker.Models;

namespace Tracker.Workers
{
    public static class TrackerWorker
    {
        public static void TrackerWorkerInit(TimeSpan TrackerSamplePeriodInMinutes)
        {
            while (true)
            {
                TrackerDBContext dbContext = new TrackerDBContext();
                EsportsLiveScore.GetNewLinks();
                var results = EsportsLiveScore.GetResultEvents();
                var filteredResults = Utilities.FilterAlreadySentEvents(dbContext, results);
                dbContext.Results.AddRange(filteredResults);
                dbContext.RemoveRange(Utilities.UnwantedEventsFromDb(dbContext));
                dbContext.SaveChanges();
                Thread.Sleep(TrackerSamplePeriodInMinutes);
            }
        }
    }
}
