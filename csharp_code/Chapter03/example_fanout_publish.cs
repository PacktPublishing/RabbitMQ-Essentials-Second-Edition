using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Samples.Chapter02;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter03
{
    class ExampleFanoutPublish
    {
        public static RabbitMqConnection Publish()
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

            // Declare a queue taxis
            var queue1 = channel
                .QueueDeclare(Chapter3Constants.QueueTaxiInbox1,
                    durable: true,
                    autoDelete: false,
                    exclusive: false);

            var queue2 = channel
                .QueueDeclare(Chapter3Constants.QueueTaxiInbox2,
                    durable: true,
                    autoDelete: false,
                    exclusive: false);

            //  Declare a fanout exchange
            channel.ExchangeDeclare(Chapter3Constants.FanoutExchange,
                ExchangeType.Fanout,
                durable: true,
                autoDelete: true);

            // Bind the queue

            channel.QueueBind(queue: Chapter3Constants.QueueTaxiInbox1,
                exchange: Chapter3Constants.FanoutExchange,
                routingKey: string.Empty);

            channel.QueueBind(queue: Chapter3Constants.QueueTaxiInbox2,
                exchange: Chapter3Constants.FanoutExchange,
                routingKey: string.Empty);

            PublishMessage(channel, "Hello everybody! This is an information message from the crew!", string.Empty);

            return new RabbitMqConnection(connection: conn, channel: channel);
        }

        private static void PublishMessage(IModel channel, string payload, string routingKey)
        {
            // prepare the payload to be published
            var messageBodyBytes = Encoding.UTF8.GetBytes(payload);
            var props = channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.ContentEncoding = "UTF-8";
            props.Persistent = true;
            props.MessageId = Guid.NewGuid().ToString();

            // publish the message to the exchange
            channel.BasicPublish(exchange: Chapter3Constants.FanoutExchange,
                routingKey: routingKey,
                basicProperties: props,
                body: messageBodyBytes);
        }
    }
}
