using System;
using System.Configuration;
using System.Threading.Tasks;
using Tracker.Workers;

namespace Tracker
{
    class Program
    {
        private static TimeSpan ResultsTrackerTimeSpan = new TimeSpan(0, int.Parse(ConfigurationManager.AppSettings["ResultsSamplePeriodInMinutes"]), 0);
        private static TimeSpan ImagesTrackerTimeSpan = new TimeSpan(0, int.Parse(ConfigurationManager.AppSettings["ImagesSamplePeriodInMinutes"]), 0);
        private static Task ResultsTask = new Task(() => ResultsWorker.TrackerWorkerInit(ResultsTrackerTimeSpan));
        private static Task ImagesTask = new Task(() => ImagesWorker.ImagesWorkerInit(ImagesTrackerTimeSpan));
        static void Main(string[] args)
        {            
            ResultsTask.Start();
            ImagesTask.Start();
            while (true)
            {
                
            }
            
        }

    }
}
