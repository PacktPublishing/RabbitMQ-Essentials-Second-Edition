using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RabbitMQ.Samples.Chapter02
{
    public class Chapter2Samples
    {
        public static void Run()
        {
            var connectionChannelDetails = CreateQueueAndExchange.Start();

            // DIRECT

            ExamplePublisher.OrderTaxi(connectionChannelDetails);
            ExampleConsumer.TaxiSubscribe(connectionChannelDetails);

            // TOPICS

            ExampleTopicDeclare.DeclareTopic(connectionChannelDetails);
            ExampleTopicConsumer.TaxiTopicSubscribe(connectionChannelDetails);
            ExampleTopicPublisher.OrderTopicTaxis(connectionChannelDetails);

            Thread.Sleep(5000);
            // close channel
            connectionChannelDetails.Channel.Close();

            // close connection
            connectionChannelDetails.Connection.Close();
        }
    }
}
