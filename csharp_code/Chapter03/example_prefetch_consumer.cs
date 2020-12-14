using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Samples.Chapter02;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter03
{
    class ExamplePrefetchConsumer
    {
        public static void TaxiSubscribe(RabbitMqConnection rabbitMqConnection, string queue)
        {
            var channel = rabbitMqConnection.Channel;

            // this configures prefetch count
            channel.BasicQos(0, 1, false);

            TaxiFanoutSubscribe(channel, queueName: queue);
        }

        private static void TaxiFanoutSubscribe(IModel channel, string queueName)
        {
            // Declare a queue for a given taxi
            channel.QueueDeclare(queueName, durable: true, 
                                    exclusive: false, 
                                    autoDelete: false);

            // Declare a direct exchange, taxi-direct
            channel.ExchangeDeclare(Chapter3Constants.FanoutExchange,
                ExchangeType.Fanout,
                durable: true,
                autoDelete: true);

            // Bind the queue to the exchange
            channel.QueueBind(queue: queueName,
                exchange: Chapter3Constants.FanoutExchange,
                routingKey: string.Empty);

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

                channel.BasicAck(args.DeliveryTag, false);
            };

            var consumerTag = channel
                .BasicConsume(
                    queue: queueName,
                    autoAck: false, // manual_ack: true
                    consumer: consumer,
                    consumerTag: $"{nameof(ExamplePrefetchConsumer)}-{Guid.NewGuid()}",
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
