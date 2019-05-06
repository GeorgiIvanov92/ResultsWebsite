using System;
using System.Collections.Generic;
using System.Text;
using Tracker.RabbitMQ;

namespace Tracker.Workers
{
    public static class LiveWorker
    {
        public static void LiveWorkerInit()
        {
            RabbitMQMessageSender.Send("zdr");
            while (true)
            {
                               
            }
        }
    }
}
