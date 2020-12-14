using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Samples.Chapter04
{
    public class Chapter4Samples
    {
        public static void Run()
        {
            var connectionDetails = ExampleExpiredMessageHandler.DeclareQueuesAndFanoutExchanges();
            ExampleDelayedSurveyRequest.RunDelayedMessage(connectionDetails);
            ExampleBackOfficeSender.TestMandatoryDelivery(connectionDetails);
        }
    }
}
