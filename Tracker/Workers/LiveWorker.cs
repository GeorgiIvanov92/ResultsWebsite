using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Tracker.Sites;

namespace Tracker.Workers
{
    public static class LiveWorker
    {
        public static void LiveWorkerInit()
        {            
            while (true)
            {
                ZPlay.GetActiveGameIds();
                ZPlay.SendActiveGames();
                Thread.Sleep(5000);
            }
        }
    }
}
