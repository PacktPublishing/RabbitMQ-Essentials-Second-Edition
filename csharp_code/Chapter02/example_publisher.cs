using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter02
{
    class ExamplePublisher
    {
        public static void OrderTaxi(RabbitMqConnection rabbitMqConnectionChannel)
        {
            var channel = rabbitMqConnectionChannel.Channel;

            // the actual message
            var payload = "example-message";

            // prepare the payload to be published
            var messageBodyBytes = Encoding.UTF8.GetBytes(payload);
            var props = channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.ContentEncoding = "UTF-8";
            props.Persistent = true;
            props.MessageId = Guid.NewGuid().ToString();

            // publish the message to the exchange
            channel.BasicPublish(exchange: Chapter2Constants.DirectExchange,
                routingKey: Chapter2Constants.QueueTaxi1,
                basicProperties: props, 
                body: messageBodyBytes);
        }
    }
}
