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
        private string _connCache = "";
        private static IModel _modelChannel;

        #region 构造函数
        public RabbitMqConsumer(Action<RabbitMqQueue> config, string optionStr, string cacheN = "")
        {
            RabbitMqueue = new RabbitMqQueue();
            config?.Invoke(RabbitMqueue);

            _connCache = cacheN;
            _connection = Connect(optionStr, cacheN);
        }
        #endregion

        protected override IModel Build(IModel channel, RabbitMqQueue mg)
        {
            ExchangeWithQueueDeclare += channel.WithExchange;
            ExchangeWithQueueDeclare += channel.WithQueueDeclare;
            ExchangeWithQueueDeclare += channel.WithQueueBind;

            return ExchangeWithQueueDeclare.Invoke(mg);
        }

        /// <summary>
        /// 消费者消费消息
        /// </summary>
        /// <param name="action"></param>
        public override void AMQPC(Action<IModel, RabbitMqQueue> action)
        {
            if (action != null)
            {
                if (_modelChannel == null) _modelChannel = _connection.CreateModel();
                var channel = Build(_modelChannel, this.RabbitMqueue);
                action?.Invoke(channel, this.RabbitMqueue);
            }
        }

        /// <summary>
        /// 关闭链接
        /// </summary>
        public void Disponse()
        {
            if (_connection != null)
            {
                if (_connection.IsOpen)
                {
                    _connection.Close();
                    _modelChannel.Close();
                    _modelChannel = null;
                }
            }

            Disponse(_connCache);
        }

        #region 私有方法
        #endregion
    }
}
