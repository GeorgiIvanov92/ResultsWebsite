using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.TransportObject
{
    [System.Serializable]
    public class LiveEvent
    {
        public TrackerEssentials.Communication.Sports.SportEnum Sport { get; set; }
        public LiveTeam HomeTeam { get; set; }
        public LiveTeam AwayTeam { get; set; }
        public int GameTime { get; set; }
        public int BestOf { get; set; }
        public int MapNumber { get; set; }
        
    }
}
