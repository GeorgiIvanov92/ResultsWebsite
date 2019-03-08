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
        public static List<T> FilterAlreadySentEvents<T>(TrackerDBContext db, List<T> rows)
        {
            if(rows is List<Results>)
            {
                List<Results> results = new List<Results>();
                rows.ForEach(ev => results.Add(ev as Results));
                List<Results> filteredResults = new List<Results>();
                var dbResults = db.Results.ToList();
                foreach (var result in results)
                {
                    bool eligableEvent = true;
                    foreach (var dbResult in dbResults)
                    {
                        if (result.LeagueName == dbResult.LeagueName
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
                return filteredResults as List<T>;
            }

            else if(rows is List<Prelive>)
            {
                List<Prelive> preliveEvents = new List<Prelive>();
                rows.ForEach(ev => preliveEvents.Add(ev as Prelive));
                List<Prelive> filteredPreliveEvents = new List<Prelive>();
                var dbPreliveEvents = db.Prelive.ToList();
                foreach (var prelive in preliveEvents)
                {
                    bool eligableEvent = true;
                    foreach (var dbPrelive in dbPreliveEvents)
                    {
                        if (prelive.LeagueName == dbPrelive.LeagueName
                            && prelive.HomeTeam == dbPrelive.HomeTeam
                            && prelive.AwayTeam == dbPrelive.AwayTeam
                            && prelive.GameDate.ToString() == dbPrelive.GameDate.ToString())
                        {
                            eligableEvent = false;
                            break;
                        }

                    }
                    if (eligableEvent)
                    {
                        filteredPreliveEvents.Add(prelive);
                    }
                }
                return filteredPreliveEvents as List<T>;
            }            
            return rows;
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
