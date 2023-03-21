using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DataConCore.Handels.RabbitMQ
{
    public class RabbitMqProvider : IRabbitMqProvider
    {
        private IRabbitMqProducer _mqProducer;
        private IRabbitMqConsumer _mqConsumer;
        private readonly RabbitMqOptions _connOptions;
        public RabbitMqProvider(IOptionsMonitor<RabbitMqOptions> connOptions)
        {
            _connOptions = connOptions.CurrentValue;
        }

        public IRabbitMqProducer RegisterProducer(Action<RabbitMqQueue> config)
        {
            _mqProducer = new RabbitMqProducer(config, JsonConvert.SerializeObject(_connOptions));
            return _mqProducer;
        }

        public IRabbitMqConsumer RegisterConsumer(Action<RabbitMqQueue> config)
        {
            _mqConsumer = new RabbitMqConsumer(config, JsonConvert.SerializeObject(_connOptions));
            return _mqConsumer;
        }
    }
}
