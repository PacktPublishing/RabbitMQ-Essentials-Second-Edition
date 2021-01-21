using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Samples.Chapter02;
using RabbitMQ.Samples.Chapter03;
using RabbitMQ.Samples.Common;

namespace RabbitMQ.Samples.Chapter04
{
    public class ExampleBackOfficeSender
    {
        public static void TestMandatoryDelivery(RabbitMqConnection rabbitMqConnection)
        {
            var channel = rabbitMqConnection.Channel;

            channel.BasicReturn += (sender, args) =>
            {
                Console.WriteLine("A returned message!"); // "A returned message!"
            };

            var queueTaxi100 = channel
                .QueueDeclare(Chapter4Constants.QueueTaxiInbox100,
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

            // create a consumer
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(message); // "A message is consumed."
            };

            var consumerTag = channel
                .BasicConsume(
                    queue: Chapter4Constants.QueueTaxiInbox100,
                    autoAck: true, // manual_ack: false
                    consumer: consumer,
                    consumerTag: nameof(ExampleBackOfficeSender),
                    noLocal: false,
                    exclusive: false,
                    arguments: null);

            // This will be handle by the above consumer
            PublishMessage(rabbitMqConnection, 
                payload: "A message published to a queue that does exist, it should NOT be returned",
                routingKey: Chapter4Constants.QueueTaxiInbox100);

            // This will be handle by the return handler
            PublishMessage(rabbitMqConnection,
                payload: "A message published to a queue that does not exist, it should be returned",
                routingKey: "random-key"); // no queue has been declared for this key
        }

        public static void PublishMessage(RabbitMqConnection rabbitMqConnectionChannel,
            string payload,
            string routingKey)
        {
            var channel = rabbitMqConnectionChannel.Channel;

            // prepare the payload to be published
            var messageBodyBytes = Encoding.UTF8.GetBytes(payload);
            var props = channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.ContentEncoding = "UTF-8";
            props.Persistent = true;
            props.MessageId = Guid.NewGuid().ToString();
            
            // publish the message to the default exchange
            channel.BasicPublish(exchange: string.Empty,
                routingKey: routingKey,
                basicProperties: props,
                body: messageBodyBytes,
                mandatory:true); // make this publish mandatory
        }
    }
}
