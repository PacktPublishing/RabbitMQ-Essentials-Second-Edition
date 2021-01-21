using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client.Events;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter02
{
    class ExampleConsumer
    {
        public static void TaxiSubscribe(RabbitMqConnection rabbitMqConnection)
        {
            var channel = rabbitMqConnection.Channel;

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
                    queue: Chapter2Constants.QueueTaxi1,
                    autoAck: true, // manual_ack: false
                    consumer: consumer,
                    consumerTag: nameof(ExampleConsumer),
                    noLocal: false,
                    exclusive:false,
                    arguments:null);
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
