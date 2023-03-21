using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Handels.RabbitMQ
{
    public class RabbitMqConsumer : RabbitMqCore, IRabbitMqConsumer
    {
        private readonly IConnection _connection;
        private event Func<RabbitMqQueue, IModel> ExchangeWithQueueDeclare;
        public RabbitMqConsumer(string config) 
        {
            _connection = Connect(config);
        }

        public RabbitMqConsumer(Action<RabbitMqQueue> config, string optionStr)
        {
            RabbitMqueue = new RabbitMqQueue();
            config?.Invoke(RabbitMqueue);
            _connection = Connect(optionStr);
        }

        public override void AMQPC(Action<IModel, RabbitMqQueue> action)
        {
            if (action != null)
            {
                var channel = Build(_connection.CreateModel(), this.RabbitMqueue);
                action?.Invoke(channel, this.RabbitMqueue);
            }
        }

        public override IModel Build(IModel channel, RabbitMqQueue mg)
        {
            ExchangeWithQueueDeclare += channel.WithExchange;
            ExchangeWithQueueDeclare += channel.WithQueueDeclare;
            ExchangeWithQueueDeclare += channel.WithQueueBind;

            return ExchangeWithQueueDeclare.Invoke(mg);
        }
    }
}
