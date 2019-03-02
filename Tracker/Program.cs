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
        private static TimeSpan PreliveTrackerTimeSpan = new TimeSpan(0, int.Parse(ConfigurationManager.AppSettings["PreliveSamplePeriodInMinutes"]), 0);
        private static Task ResultsTask = new Task(() => ResultsWorker.ResultsWorkerInit(ResultsTrackerTimeSpan));
        private static Task ImagesTask = new Task(() => ImagesWorker.ImagesWorkerInit(ImagesTrackerTimeSpan));
        private static Task PreliveTask = new Task(() => PreliveWorker.PreliveWorkerInit(PreliveTrackerTimeSpan));
        static void Main(string[] args)
        {            
            ResultsTask.Start();
            ImagesTask.Start();
            PreliveTask.Start();
            while (true)
            {

            }
            
        }

    }
}
