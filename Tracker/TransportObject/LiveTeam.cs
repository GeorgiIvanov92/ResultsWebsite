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
        public int GoldLead { get; set; }
        public int Kills { get; set; }
        public int WinsInSeries { get; set; }
    }
}
