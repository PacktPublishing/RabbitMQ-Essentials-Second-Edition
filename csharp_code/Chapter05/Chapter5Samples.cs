using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Samples.Chapter05.stomp;

namespace RabbitMQ.Samples.Chapter05
{
    public class Chapter5Samples
    {
        public static void Run()
        {
            var connectionChannelDetails = SetupStomp.DeclareQueuesAndExchange();
            ExampleQueueSend.PublishSubscribeWithHeaders();
        }
    }
}
