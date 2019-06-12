using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Tracker.Sites;

namespace Tracker.Workers
{
    public static class LiveWorker
    {
        public static void LiveWorkerInit(TimeSpan timeSpan)
        {            
            while (true)
            {
                ZPlay.GetActiveGameIds();
                ZPlay.SendActiveGames();
                Thread.Sleep(timeSpan);
            }
        }
    }
}
