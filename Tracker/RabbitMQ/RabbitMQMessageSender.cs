using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Tracker.Models;

namespace Tracker.RabbitMQ
{
    public static class RabbitMQMessageSender
    {
        static ConnectionFactory Factory;
        static IConnection Connection;
        static IModel Channel;
        static RabbitMQMessageSender()
        {
            Factory = new ConnectionFactory() { HostName = "localhost" };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
            Channel.QueueDeclare(queue: "task_queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            Channel.BasicConsume(queue: "task_queue",
                                 autoAck: true,
                                 consumer: consumer);
        }
        public static void Send(string messageBody)
        {
            var message = messageBody;
            var body = Encoding.UTF8.GetBytes(message);            
            body = ObjectToByteArray(new Prelive() { HomeTeam="SKT"});
            var properties = Channel.CreateBasicProperties();
            properties.Persistent = true;

            Channel.BasicPublish(exchange: "",
                                 routingKey: "task_queue",
                                 basicProperties: properties,
                                 body: body);
        }
        private static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        
    }   
}
