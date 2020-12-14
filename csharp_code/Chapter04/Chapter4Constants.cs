using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Samples.Chapter04
{
    public class Chapter4Constants
    {
        public const string QueueTaxiInbox1 = "taxi-inbox.1";
        public const string QueueTaxiInbox2 = "taxi-inbox.2";
        public const string QueueDeadLetter = "taxi-dlq";
        public const string QueueDelayed = "work.later";
        public const string QueueDestination = "work.now";
        public const string QueueTaxiInbox100 = "taxi-inbox.100";
        public const string FanoutExchange = "taxi-fanout";
        public const string FanoutExchangeDlx = "taxi-dlx";
    }
}
