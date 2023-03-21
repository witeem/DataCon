using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Handels.RabbitMQ
{
    public class RabbitMqOptions
    {
        public RabbitMqConnOptions Connector { get; set; }

        public List<RabbitMqHostOptions> Hosts { get; set; }
    }

    public class RabbitMqConnOptions
    { 
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool ConsumersAsunc { get; set; }

        public string VirtualHost { get; set; }
    }

    public class RabbitMqHostOptions
    { 
        public string Host { get; set; }

        public int Port { get; set; }
    }
}
