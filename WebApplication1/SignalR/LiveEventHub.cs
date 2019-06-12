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
        public void SendEvent(object sender, RabbitMQMessageReceiver.LiveEventArgs e)
        {
            Clients.All.SendAsync("ReceiveMessage",e.LiveEvent);
        }
    }
}
