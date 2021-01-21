using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter02
{
    public class ExampleTopicDeclare
    {
        public static void DeclareTopic(RabbitMqConnection rabbitMqConnection)
        {
            var channel = rabbitMqConnection.Channel;

            // Declare a direct exchange, taxi-direct
            channel.ExchangeDeclare(Chapter2Constants.TopicExchange,
                ExchangeType.Topic,
                durable: true,
                autoDelete: true);
        }
    }
}
