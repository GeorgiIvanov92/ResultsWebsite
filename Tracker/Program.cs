using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tracker.Models;
using Tracker.Workers;

namespace Tracker
{
    class Program
    {
        private static TimeSpan TrackerSamplePeriodInMinutes = new TimeSpan(0, int.Parse(ConfigurationManager.AppSettings["ResultsSamplePeriodInMinutes"]), 0);
        private static Task TrackerTask = new Task(() => TrackerWorker.TrackerWorkerInit(TrackerSamplePeriodInMinutes));
        Program()
        {
            
        }
        
        static void Main(string[] args)
        {            
            TrackerTask.Start();
            while (true)
            {
                
            }
            
        }

    }
}
