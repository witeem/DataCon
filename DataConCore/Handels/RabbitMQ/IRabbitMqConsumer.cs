using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Handels.RabbitMQ
{
    public interface IRabbitMqConsumer
    {
        void AMQPC(Action<IModel, RabbitMqQueue> action);
    }
}
