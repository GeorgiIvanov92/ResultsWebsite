using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Models;
using WebApplication1.Models;

namespace WebApi.Communication
{
    public class Sport
    {
        public ConcurrentDictionary<string, HashSet<Results>> ResultsEvents = new ConcurrentDictionary<string,HashSet<Results>>();
        public ConcurrentDictionary<string, HashSet<Prelive>> PreliveEvents = new ConcurrentDictionary<string, HashSet<Prelive>>();
        public ConcurrentDictionary<string, string> TeamLogos = new ConcurrentDictionary<string, string>();
    }
}
