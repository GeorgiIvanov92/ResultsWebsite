using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tracker.Models;

namespace Tracker
{
    public static class Utilities
    {
        public static Dictionary<int, string> Sport = new Dictionary<int, string>()
        {
            {1,"leagueoflegends" },
            {2,"csgo" },
            {3,"dota" }
        };
        public static List<Results> FilterAlreadySentEvents(TrackerDBContext db, List<Results> results)
        {
            List<Results> filteredResults = new List<Results>();
            var dbResults = db.Results.ToList();
            foreach(var result in results)
            {
                bool eligableEvent = true;
                foreach(var dbResult in dbResults)
                {
                    if(result.LeagueName == dbResult.LeagueName 
                        && result.HomeTeam == dbResult.HomeTeam
                        && result.AwayTeam == dbResult.AwayTeam
                        && result.GameDate.ToString() == dbResult.GameDate.ToString())
                    {
                        eligableEvent = false;
                        break;
                    }
                   
                }
                if (eligableEvent)
                {
                    filteredResults.Add(result);
                }
            }
            return filteredResults;
        }
        public static List<Results> UnwantedEventsFromDb(TrackerDBContext dbContext)
        {
            var eventsToRemove = new List<Results>();
            foreach (var res in dbContext.Results)
            {
                if (res.GameDate < DateTime.UtcNow.AddDays(-30))
                {
                    eventsToRemove.Add(res);
                }
            }
            return eventsToRemove;
        }
    }
}
