using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Tracker.Models;
using Tracker.Sites;

namespace Tracker.Workers
{
    public static class ResultsWorker
    {
        public static void ResultsWorkerInit(TimeSpan ResultsSamplePeriod)
        {
            while (true)
            {                
                TrackerDBContext dbContext = new TrackerDBContext();
                EsportsLiveScore.GetNewLinks();
                var results = EsportsLiveScore.GetResultEvents();
                var filteredResults = Utilities.FilterAlreadySentResults(dbContext, results);
                dbContext.Results.AddRange(filteredResults);
                Utilities.RemoveUnwatedEventsFromDb(ref dbContext);
                dbContext.SaveChanges();
                Console.WriteLine($"Finished Getting Results at {DateTime.Now.ToShortTimeString()}. New Results Added to db: {filteredResults.Count}. Sampling Results again in {ResultsSamplePeriod.Hours} hours and {ResultsSamplePeriod.Minutes} minutes");
                Thread.Sleep(ResultsSamplePeriod);
            }
        }
    }
}
