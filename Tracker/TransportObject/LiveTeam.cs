using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.TransportObject
{
    [System.Serializable]
    public class LiveTeam
    {
        public string TeamName { get; set; }
        public bool IsLeadingInGold { get; set; }
        public int Gold { get; set; }
        public int Kills { get; set; }
        public int WinsInSeries { get; set; }
        public List<LivePlayer> Players { get; set;}
    }
}
