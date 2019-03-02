using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Workers
{
    public class TaskSceduler
    {
        private Dictionary<Task, TimeSpan> TasksDict;
        public TaskSceduler(Dictionary<Task,TimeSpan> tasks)
        {
            TasksDict = tasks;
        }
        public void ManageTasks()
        {
            foreach (var task in TasksDict)
            {
                if (task.Key.Status == TaskStatus.WaitingForActivation)
                {
                    task.Key.Start();
                }
            }
            RestartTasksIfNeeded();
        }
        public void RestartTasksIfNeeded()
        {
            while (true)
            {
                foreach(var task in TasksDict)
                {
                    if(task.Key.IsCompleted)
                    {
                        //Task.Delay(task.Value);
                    }
                }
            }

        }
    }
}
