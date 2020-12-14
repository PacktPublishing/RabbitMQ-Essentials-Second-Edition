using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Samples.Chapter03
{
    public class Chapter3Samples
    {
        public static void Run()
        {
            // creates connection / exchange / queues / channel
            var connectionChannelDetails =  ExampleFanoutPublish.Publish();

            // subscribe to first fanout queue
            ExamplePrefetchConsumer.TaxiSubscribe(connectionChannelDetails, Chapter3Constants.QueueTaxiInbox1);
            ExamplePrefetchConsumer.TaxiSubscribe(connectionChannelDetails, Chapter3Constants.QueueTaxiInbox2);
        }
    }
}
