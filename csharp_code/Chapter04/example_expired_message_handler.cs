using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Samples.Chapter03;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter04
{
    class ExampleExpiredMessageHandler
    {
        public static RabbitMqConnection DeclareQueuesAndFanoutExchanges()
        {

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
            // If queues have been already declared without ttl then the followings will throw exception
            // the solution is to either delete the queues and re-run them or simple use the set_q_ttl_dlx.sh
            // to declare ttl through policy
            var queue1 = channel
                .QueueDeclare(Chapter4Constants.QueueTaxiInbox1,
                    durable: true,
                    autoDelete: false,
                    exclusive: false
                    ,arguments: new Dictionary<string, object>()
                    {
                        { "x-message-ttl", 604800000 },
                        { "x-dead-letter-exchange", "taxi-dlx"}
                    });

            var queue2 = channel
                .QueueDeclare(Chapter4Constants.QueueTaxiInbox2,
                    durable: true,
                    autoDelete: false,
                    exclusive: false
                    ,arguments: new Dictionary<string, object>()
                    {
                        { "x-message-ttl", 604800000 },
                        { "x-dead-letter-exchange", "taxi-dlx"}
                    });

            //  Declare a fanout exchange
            channel.ExchangeDeclare(Chapter4Constants.FanoutExchange,
                ExchangeType.Fanout,
                durable: true,
                autoDelete: true);

            // Bind the queue

            channel.QueueBind(queue: Chapter4Constants.QueueTaxiInbox1,
                exchange: Chapter4Constants.FanoutExchange,
                routingKey: string.Empty);

            channel.QueueBind(queue: Chapter4Constants.QueueTaxiInbox2,
                exchange: Chapter4Constants.FanoutExchange,
                routingKey: string.Empty);

            // Declare a dead letter queue
            var deadLetterQueue = channel
                .QueueDeclare(Chapter4Constants.QueueDeadLetter,
                    durable: true,
                    autoDelete: false,
                    exclusive: false);

            // Declare dead letter fanout exchange
            channel.ExchangeDeclare(Chapter4Constants.FanoutExchangeDlx,
                ExchangeType.Fanout,
                durable: true,
                autoDelete: true);

            // Bind taxi-dlx to taxi-dlq
            channel.QueueBind(queue: Chapter4Constants.QueueDeadLetter,
                exchange: Chapter4Constants.FanoutExchangeDlx,
                routingKey: string.Empty);

            PublishMessage(channel, "Hello! This is an information message!", string.Empty);

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
            channel.BasicPublish(exchange: Chapter4Constants.FanoutExchange,
                routingKey: routingKey,
                basicProperties: props,
                body: messageBodyBytes);
        }
    }
}
