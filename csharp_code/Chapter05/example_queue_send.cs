using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter05
{
    class ExampleQueueSend
    {
        public static void PublishSubscribeWithHeaders()
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


                var queueTaxiInformationWithHeaders = channel
                    .QueueDeclare(Chapter5Constants.QueueTaxiInformationWithHeaders,
                        durable: true,
                        autoDelete: false,
                        exclusive: false);

                //  Declare an exchange
                channel.ExchangeDeclare(Chapter5Constants.TaxiHeaderExchange,
                    ExchangeType.Headers,
                    durable: true,
                    autoDelete: true);

                // Bind the queue
                channel.QueueBind(
                    queue: Chapter5Constants.QueueTaxiInformationWithHeaders,
                    exchange: Chapter5Constants.TaxiHeaderExchange,
                    routingKey: string.Empty,
                    arguments: new Dictionary<string, object>()
                    {
                        { "x-match", "all" },
                        { "system", "taxi" },
                        { "version", "0.1b" },
                    });

                var connectionChannelDetails =
                    new RabbitMqConnection(connection: conn, channel: channel);

                // publish a location
                PublishCoordinatesWithHeaders(connectionChannelDetails);
        }

        public static void PublishCoordinatesWithHeaders(RabbitMqConnection rabbitMqConnectionChannel)
        {
            var channel = rabbitMqConnectionChannel.Channel;

            var rand = new Random();

            var coordinates = new Coordinates()
            {
                Latitude = GenerateDouble(rand, 0, 100),
                Longitude = GenerateDouble(rand, 0, 100)
            };

            // the actual message
            var payload = JsonConvert.SerializeObject(coordinates);

            // prepare the payload to be published
            var messageBodyBytes = Encoding.UTF8.GetBytes(payload);
            var props = channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.ContentEncoding = "UTF-8";
            props.Persistent = true;
            props.MessageId = Guid.NewGuid().ToString();
            props.Headers = new Dictionary<string, object>()
            {
                { "system", "taxi" },
                { "version", "0.1b" },
            };

            // publish the message to the exchange
            channel.BasicPublish(exchange: Chapter5Constants.TaxiHeaderExchange,
                routingKey: string.Empty,
                basicProperties: props,
                body: messageBodyBytes);
        }

        private static double GenerateDouble(Random rand, double minValue, double maxValue)
        {
            return rand.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}
