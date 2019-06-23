using Microsoft.AspNetCore.SignalR;
using RabbitMQ.RabbitMQ;
using RabbitMQ.TransportObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.SignalR
{
    public class LiveEventHub : Hub
    {
        protected IHubContext<LiveEventHub> _context;
        public LiveEventHub(IHubContext<LiveEventHub> context)
        {
            this._context = context; 
        }
        public async void SendEvent(object sender, RabbitMQMessageReceiver.LiveEventArgs e)
        {           
            
            await _context.Clients.All.SendAsync("ReceiveMessage", e.LiveEvent);
        }
    }
}
