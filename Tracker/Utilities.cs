using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tracker.Models;

namespace Tracker
{
    public static class Utilities
    {
        public static List<Results> FilterAlreadySentEvents(TrackerDBContext db, List<Results> results)
        {
            List<Results> filteredResults = new List<Results>();
            var dbResults = db.Results.ToList();
            foreach(var result in results)
            {
                bool eligableEvent = true;
                foreach(var dbResult in dbResults)
                {
                    if(result.LeagueName != dbResult.LeagueName 
                        || result.HomeTeam != dbResult.HomeTeam
                        || result.AwayTeam != dbResult.AwayTeam
                        || result.GameDate.ToString() != dbResult.GameDate.ToString())
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
    }
}
