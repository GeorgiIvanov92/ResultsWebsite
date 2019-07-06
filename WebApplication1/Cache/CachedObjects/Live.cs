using RabbitMQ.TransportObject;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Cache.CachedObjects
{
    public class Live
    {
        public ConcurrentDictionary<string,LiveEvent> LiveEvents = new ConcurrentDictionary<string,LiveEvent>();
    }
}
