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
                while (!EsportsLiveScore.HaveGatheredLinks)
                {

                }
                EsportsLiveScore.WriteTeamIconsToDisk();
                Console.WriteLine($"Finished Saving Team Logos at {DateTime.Now.ToShortTimeString()}. Sampling images again in {ImageSamplePeriodInMinutes.Hours} hours {ImageSamplePeriodInMinutes.Minutes} minutes");
                Thread.Sleep(ImageSamplePeriodInMinutes);
            }
        }
    }
}
