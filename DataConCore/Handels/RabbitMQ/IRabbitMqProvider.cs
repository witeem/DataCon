using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Handels.RabbitMQ
{
    public interface IRabbitMqProvider
    {
        IRabbitMqProducer RegisterProducer(Action<RabbitMqQueue> config, string cacheN = "");

        IRabbitMqConsumer RegisterConsumer(Action<RabbitMqQueue> config, string cacheN = "");
    }
}
