using System;
using System.Linq;
using Tracker.Models;

namespace Tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            TrackerDBContext dbContext = new TrackerDBContext();
            EsportsLiveScore.GetNewLinks();
            var results = EsportsLiveScore.GetResultEvents();
            //dbContext.Results.Add();
            dbContext.SaveChanges();
            
        }
    }
}
