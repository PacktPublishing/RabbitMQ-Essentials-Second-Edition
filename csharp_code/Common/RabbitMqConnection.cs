using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ.Samples.Common
{
    public class RabbitMqConnection
    {
        public IConnection Connection { get; private set; }
        public IModel Channel { get; private set; }

        public RabbitMqConnection(IConnection connection, IModel channel)
        {
            Connection = connection;
            Channel = channel;
        }
    }
}
