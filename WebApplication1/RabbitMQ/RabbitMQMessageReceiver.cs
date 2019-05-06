using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Tracker.RabbitMQ
{
    public class RabbitMQMessageReceiver
    {
        private ConnectionFactory Factory;
        private IConnection Connection;
        private IModel Channel;
        private EventingBasicConsumer Consumer;
        delegate void asd(BasicDeliverEventArgs ea);
        public RabbitMQMessageReceiver()
        {
            Factory = new ConnectionFactory() { HostName = "localhost" };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
            Channel.QueueDeclare(queue: "task_queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Consumer = new EventingBasicConsumer(Channel);
            Consumer.Received += MessageReceived;
            Channel.BasicConsume(queue: "task_queue",
                                 autoAck: true,
                                 consumer: Consumer);
        }       
        public void MessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var asd = Encoding.UTF8.GetString(e.Body);
        }
    }
}
