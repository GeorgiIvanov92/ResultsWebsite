using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Tracker.Models;

namespace Tracker.Workers
{
    public static class ImagesWorker
    {
        public static void ImagesWorkerInit(TimeSpan ImageSamplePeriodInMinutes)
        {
            while (true)
            {
                EsportsLiveScore.WriteTeamIconsToDisk();
                Thread.Sleep(ImageSamplePeriodInMinutes);
            }
        }
    }
}
