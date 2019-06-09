using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Models;
using WebApi.Models;
using WebApplication1.Models;

namespace WebApi.TransportObjects
{
    public class Sport
    {
        public ConcurrentDictionary<string, HashSet<Results>> ResultsEvents = new ConcurrentDictionary<string,HashSet<Results>>();
        public ConcurrentDictionary<string, HashSet<Prelive>> PreliveEvents = new ConcurrentDictionary<string, HashSet<Prelive>>();
        public ConcurrentDictionary<string, string> TeamLogos = new ConcurrentDictionary<string, string>();
        public ConcurrentDictionary<string, HashSet<Team>> TeamsInLeague = new ConcurrentDictionary<string, HashSet<Team>>();
        public List<Player> Players = new List<Player>();
        public List<ChampionStat> ChampionStats = new List<ChampionStat>(); 

        public DateTime LastUpdate = DateTime.UtcNow;
    }
}
