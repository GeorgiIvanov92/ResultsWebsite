using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Tracker.RabbitMQ;
using Tracker.Sites;
using Tracker.TransportObject;

namespace Tracker.Workers
{
    public static class LiveWorker
    {
        public static void LiveWorkerInit()
        {            
            while (true)
            {
                _1zPlay.GetActiveGameIds();
                _1zPlay.SendActiveGames();
                Thread.Sleep(5000);
            }
        }
    }
}
