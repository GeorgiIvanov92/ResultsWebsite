using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.TransportObject
{
    [System.Serializable]
    public class LiveEvent
    {
        public TrackerEssentials.Communication.Sports.SportEnum Sport { get; set; }
        public string LeagueName { get; set; }
        public LiveTeam HomeTeam { get; set; }
        public LiveTeam AwayTeam { get; set; }
        public int GameTime { get; set; }
        public int BestOf { get; set; }
        public int MapNumber { get; set; }

        // 0 none / 1 home / 2 away
        public int FirstBlood { get; set; }
        public int FirstTower { get; set; }
        
    }
}
