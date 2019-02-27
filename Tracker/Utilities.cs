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
        public static List<Results> FilterAlreadySentResults(TrackerDBContext db, List<Results> results)
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

        public static List<Prelive> FilterAlreadySentPreliveEvents(TrackerDBContext db, List<Prelive> preliveEvents)
        {
            List<Prelive> filteredPreliveEvents = new List<Prelive>();
            var dbPreliveEvents = db.Prelive.ToList();
            foreach (var result in preliveEvents)
            {
                bool eligableEvent = true;
                foreach (var dbPrelive in dbPreliveEvents)
                {
                    if (result.LeagueName == dbPrelive.LeagueName
                        && result.HomeTeam == dbPrelive.HomeTeam
                        && result.AwayTeam == dbPrelive.AwayTeam
                        && result.GameDate.ToString() == dbPrelive.GameDate.ToString())
                    {
                        eligableEvent = false;
                        break;
                    }

                }
                if (eligableEvent)
                {
                    filteredPreliveEvents.Add(result);
                }
            }
            return filteredPreliveEvents;
        }

        public static void RemoveUnwatedEventsFromDb(ref TrackerDBContext dbContext)
        {
            var resultsToRemove = new List<Results>();
            foreach (var res in dbContext.Results)
            {
                if (res.GameDate < DateTime.UtcNow.AddDays(-30))
                {
                    resultsToRemove.Add(res);
                }
            }
            dbContext.RemoveRange(resultsToRemove);
            var preliveEventsToRemove = new List<Prelive>();
            foreach(var pre in dbContext.Prelive)
            {
                if (pre.GameDate < DateTime.UtcNow)
                {
                    preliveEventsToRemove.Add(pre);
                }
            }
            dbContext.RemoveRange(preliveEventsToRemove);
        }
    }
}
