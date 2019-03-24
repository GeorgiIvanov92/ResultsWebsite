using System;
using Tracker;
using Xunit;

namespace TrackerTests
{
    public class UnitTest1
    {
        [Fact]
        public void XEsportsLiveScoreResultEvents()
        {
            EsportsLiveScore.GetNewLinks();
            var results = EsportsLiveScore.GetResultEvents();
            foreach(var res in results)
            {
                Assert.False(res.GameDate == null 
                    || res.GameDate > DateTime.Now.AddHours(6)
                    ,$"{res.LeagueName} - {res.HomeTeam} vs {res.AwayTeam}. The specific failed event date: {res.GameDate}");
            }
        }
    }
}
