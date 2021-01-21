using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter02
{
    public class ExampleTopicPublisher
    {
        public static void OrderTopicTaxis(RabbitMqConnection rabbitMqConnection)
        {
            var channel = rabbitMqConnection.Channel;
                
            OrderTaxi("taxi.eco.3", channel, "message for taxi.eco");
            OrderTaxi("taxi", channel, "message for taxi");
        }

        private static void OrderTaxi(string type, IModel channel, string payload)
        {
            // prepare the payload to be published
            var messageBodyBytes = Encoding.UTF8.GetBytes(payload);
            var props = channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.ContentEncoding = "UTF-8";
            props.Persistent = true;
            props.MessageId = Guid.NewGuid().ToString();

            // publish the message to the exchange
            channel.BasicPublish(exchange: Chapter2Constants.TopicExchange,
                routingKey: type,
                basicProperties: props,
                body: messageBodyBytes);
        }
    }
}
