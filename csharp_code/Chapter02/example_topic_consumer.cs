using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter02
{
    public class ExampleTopicConsumer
    {
        public static void TaxiTopicSubscribe(RabbitMqConnection rabbitMqConnection)
        {
            var channel = rabbitMqConnection.Channel;

            // Creates a queue named 'taxi.3' with 2 bindings in the topic exchange
            // First binding receives all messages with routing key 'taxi.eco.*'
            // second binding receives all messages with routing key 'taxi'
            TaxiTopicSubscribe(channel, queueName: "taxi.3", "taxi.eco.*");
        }

        private static void TaxiTopicSubscribe(IModel channel, string queueName, string type)
        {
            // Declare a queue for a given taxi
            channel.QueueDeclare(queueName, durable: true);

            // Bind the queue to the exchange
            channel.QueueBind(queue: queueName,
                exchange: Chapter2Constants.TopicExchange,
                routingKey: type);

            // Bind the queue to the exchange to make sure the taxi will get any order
            channel.QueueBind(queue: queueName,
                exchange: Chapter2Constants.TopicExchange,
                routingKey: "taxi");

            Consume(channel, queueName);
        }


        private static void Consume(IModel channel, string queueName)
        {
            // create a consumer
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);

                ProcessOder(message);
            };

            var consumerTag = channel
                .BasicConsume(
                    queue: queueName,
                    autoAck: true, // manual_ack: false
                    consumer: consumer,
                    consumerTag: nameof(ExampleTopicConsumer),
                    noLocal: false,
                    exclusive: false,
                    arguments: null);
        }

        private static void ProcessOder(string message)
        {
            Console.WriteLine("Handling taxi order");
            Console.WriteLine(message);
            Thread.Sleep(500);

            Console.WriteLine("Processing done");
        }
    }
}
