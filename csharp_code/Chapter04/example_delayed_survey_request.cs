using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Samples.Chapter03;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter04
{
    class ExampleDelayedSurveyRequest
    {
        public static void RunDelayedMessage(RabbitMqConnection rabbitMqConnection)
        {
            var channel = rabbitMqConnection.Channel;

            SubscribeDestinationQueue(channel);
            PublishDelayedMessage(channel);
        }

        // Define publish method
        private static void PublishDelayedMessage(IModel channel)
        {
            // Declare queue
            // x-dead-letter-exchange: empty => this will end up to the default exchange => queue based on msg routing key
            channel
                .QueueDeclare(Chapter4Constants.QueueDelayed,
                    durable: true,
                    autoDelete: false,
                    exclusive: false,
                    arguments: new Dictionary<string, object>()
                    {
                        { "x-message-ttl", 120000 },
                        { "x-dead-letter-exchange", "" },
                        { "x-dead-letter-routing-key", Chapter4Constants.QueueDestination }
                    });

            PublishMessage(channel, $"{DateTime.Now}: Published the message", routingKey: Chapter4Constants.QueueDelayed );
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
            channel.BasicPublish(exchange: "", // default exchange => queue from routing key
                routingKey: routingKey,
                basicProperties: props,
                body: messageBodyBytes);
        }

        // Define subscribe method
        private static void SubscribeDestinationQueue(IModel channel)
        {
            // Declare a queue for a given taxi
            channel.QueueDeclare(Chapter4Constants.QueueDestination, 
                durable: true,
                exclusive: false,
                autoDelete: false);

            Consume(channel, Chapter4Constants.QueueDestination);
        }

        private static void Consume(IModel channel, string queueName)
        {
            // create a consumer
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"{DateTime.Now}: Got the message");

                channel.BasicAck(args.DeliveryTag, false);
            };

            var consumerTag = channel
                .BasicConsume(
                    queue: queueName,
                    autoAck: false, // manual_ack: true
                    consumer: consumer,
                    consumerTag: $"{nameof(ExampleDelayedSurveyRequest)}-{Guid.NewGuid()}",
                    noLocal: false,
                    exclusive: false,
                    arguments: null);
        }
    }
}
