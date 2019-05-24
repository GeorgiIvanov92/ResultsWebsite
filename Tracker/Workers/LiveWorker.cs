using System;
using System.Collections.Generic;
using System.Text;
using Tracker.RabbitMQ;
using Tracker.Sites;
using Tracker.TransportObject;

namespace Tracker.Workers
{
    public static class LiveWorker
    {
        public static void LiveWorkerInit()
        {
            _1zPlay.GetActiveGameIds();
            while (true)
            {
                               
            }
        }
    }
}
