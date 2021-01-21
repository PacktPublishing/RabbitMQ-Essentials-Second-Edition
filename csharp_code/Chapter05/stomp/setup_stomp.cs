using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Samples.Chapter02;
using RabbitMQ.Samples.Chapter04;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter05.stomp
{
    class SetupStomp
    {
        public static RabbitMqConnection DeclareQueuesAndExchange()
        {

            var factory = new ConnectionFactory();
            factory.Uri = new Uri(Constants.RabbitmqUriWs);

            // or set it manually
            /*
            factory.UserName = "cc-dev";
            factory.Password = "taxi123";
            factory.VirtualHost = "cc-dev-ws";
            */

            var conn = factory.CreateConnection();

            // Create a channel
            var channel = conn.CreateModel();


            var queueTaxiInformation = channel
                .QueueDeclare(Chapter5Constants.QueueTaxiInformation,
                    durable: true,
                    autoDelete: false,
                    exclusive: false);

            //  Declare an exchange
            channel.ExchangeDeclare(Chapter5Constants.TaxiExchange,
                ExchangeType.Direct,
                durable: true,
                autoDelete: true);

            // Bind the queue
            channel.QueueBind(queue: Chapter5Constants.QueueTaxiInformation,
                exchange: Chapter5Constants.TaxiExchange,
                routingKey: Chapter5Constants.QueueTaxiInformation);

            var connectionChannelDetails = 
                new RabbitMqConnection(connection: conn, channel: channel);

            // publish a location
            PublishCoordinates(connectionChannelDetails);

            return connectionChannelDetails;
        }

        public static void PublishCoordinates(RabbitMqConnection rabbitMqConnectionChannel)
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

            // publish the message to the exchange
            channel.BasicPublish(exchange: Chapter5Constants.TaxiExchange,
                routingKey: Chapter5Constants.QueueTaxiInformation,
                basicProperties: props,
                body: messageBodyBytes);
        }

        private static double GenerateDouble(Random rand, double minValue, double maxValue)
        {
            return rand.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}
