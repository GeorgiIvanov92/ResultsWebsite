using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Tracker.Sites;
using Tracker.Sites.Zplay;

namespace Tracker.Workers
{
    public static class LiveWorker
    {
        public static void LiveWorkerInit(TimeSpan timeSpan)
        {
            SocketConnection socketConnection = new SocketConnection();          
            while (true)
            {             
                try
                {
                    if (!socketConnection.isSocketActive)
                    {
                        socketConnection = new SocketConnection();
                    }
                    //ZPlay.GetActiveGameIds();
                    //ZPlay.SendActiveGames();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
