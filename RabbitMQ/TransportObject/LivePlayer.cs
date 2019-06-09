using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.TransportObject
{
    [System.Serializable]
    public class LivePlayer
    {
        public string Nickname { get; set; }
        public string ChampionName { get; set; }
        public string ChampionImageUrl { get; set; }
    }
}
