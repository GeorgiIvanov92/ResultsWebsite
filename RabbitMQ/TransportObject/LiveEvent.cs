using System;
using System.Collections.Generic;
using System.Text;
using static RabbitMQ.TrackerEssentials.Communication.Sports;

namespace RabbitMQ.TransportObject
{
    [System.Serializable]
    public class LiveEvent
    {
        public SportEnum Sport { get; set; }
        public LiveTeam HomeTeam { get; set; }
        public LiveTeam AwayTeam { get; set; }
        public string LeagueName { get; set; }
        public int FirstTower { get; set; }
        public int FirstBlood { get; set; }
        public int GameTime { get; set; }
        public int BestOf { get; set; }
        public int MapNumber { get; set; }
        
    }
}
