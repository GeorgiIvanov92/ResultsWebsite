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
                try
                {
                    ZPlay.GetActiveGameIds();
                    ZPlay.SendActiveGames();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(timeSpan);
            }
        }
    }
}
