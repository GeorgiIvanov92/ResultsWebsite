using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using RabbitMQ.RabbitMQ;
using RabbitMQ.TransportObject;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Cache.CachedObjects;

namespace WebApi.SignalR
{
    public class LiveEventHub : Hub
    {
        private IMemoryCache _cache;
        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            SendCachedLiveEvents();
        }
        protected IHubContext<LiveEventHub> _context;
        public LiveEventHub(IHubContext<LiveEventHub> context, IMemoryCache cache)
        {
            this._context = context;
            _cache = cache;
        }
        public async void SendEvent(object sender, RabbitMQMessageReceiver.LiveEventArgs e)
        {
            string key = CreateLiveGameKey(e.LiveEvent);
            var live = _cache.Get("Live") as Live;
            if (live.LiveEvents.ContainsKey(key))
            {
                live.LiveEvents[key] = e.LiveEvent;
            }
            else
            {
                live.LiveEvents.TryAdd(key, e.LiveEvent);
            }
            _cache.Set("Live", live);

            await _context.Clients.All.SendAsync(((int)e.LiveEvent.Sport).ToString(), e.LiveEvent);
        }
        public async void RemoveLiveEvent(LiveEvent ev)
        {
            ev.shouldRemoveEvent = true;
            await _context.Clients.All.SendAsync(((int)ev.Sport).ToString(), ev);
        }
        public string CreateLiveGameKey(LiveEvent liveEvent)
        {
            if (liveEvent != null)
            {
                return $"{liveEvent.HomeTeam.TeamName} vs {liveEvent.AwayTeam.TeamName}";
            }
            return string.Empty;
        }
        private async void SendCachedLiveEvents()
        {
            var live = _cache.Get("Live") as Live;
            foreach(var ev in live.LiveEvents)
            {
               await _context.Clients.All.SendAsync(((int)ev.Value.Sport).ToString(), ev.Value);
            }
        }

    }
}
