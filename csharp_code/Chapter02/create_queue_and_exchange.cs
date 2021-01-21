using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter02
{
    class CreateQueueAndExchange
    {
        public static RabbitMqConnection Start()
        {
            // Create a connection
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(Constants.RabbitmqUri);

            // or set it manually
            /*
            factory.UserName = "cc-dev";
            factory.Password = "taxi123";
            factory.VirtualHost = "cc-dev-vhost";
            */

            var conn = factory.CreateConnection();
            
            // Create a channel
            var channel = conn.CreateModel();

            // Declare a queue for a given taxi
            var queue = channel
                    .QueueDeclare(Chapter2Constants.QueueTaxi1, 
                                                durable: true, 
                                                autoDelete:false, 
                                                exclusive:false);
            
            // Declare a direct exchange, taxi-direct
            channel.ExchangeDeclare(Chapter2Constants.DirectExchange, 
                                    ExchangeType.Direct, 
                                    durable: true, 
                                    autoDelete: true);

            // Bind the queue to the exchange
            channel.QueueBind(queue: Chapter2Constants.QueueTaxi1, 
                                exchange: Chapter2Constants.DirectExchange, 
                                routingKey: Chapter2Constants.QueueTaxi1);

            return new RabbitMqConnection(connection: conn, channel: channel);
        }
    }
}
