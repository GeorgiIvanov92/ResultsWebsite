using System;
using System.Collections.Generic;
using System.Text;
using Tracker.RabbitMQ;
using Tracker.TransportObject;

namespace Tracker.Workers
{
    public static class LiveWorker
    {
        public static void LiveWorkerInit()
        {
            LiveEvent live = new LiveEvent();
            live.HomeTeam = new LiveTeam();
            live.HomeTeam.TeamName = "SKT";
            live.MapNumber = 1;
            live.Sport = TrackerEssentials.Communication.Sports.SportEnum.LeagueOfLegends;
            RabbitMQMessageSender.Send(live);
            while (true)
            {
                               
            }
        }
    }
}
