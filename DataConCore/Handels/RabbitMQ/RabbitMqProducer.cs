using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Handels.RabbitMQ
{
    public class RabbitMqProducer : RabbitMqCore, IRabbitMqProducer
    {
        private event Func<RabbitMqQueue, IModel> ExchangeWithQueueDeclare;
        protected IConnection _connection { get; set; }

        #region 构造函数
        public RabbitMqProducer(Action<RabbitMqQueue> config, string optionStr, string cacheN = "")
        {
            RabbitMqueue = new RabbitMqQueue();
            config?.Invoke(RabbitMqueue);
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
        /// 自定义发送
        /// </summary>
        /// <param name="action"></param>
        public override void AMQP(Action<IModel, RabbitMqQueue> action)
        {
            if (action != null)
            {
                using (var channel = Build(_connection.CreateModel(), this.RabbitMqueue))
                {
                    action?.Invoke(channel, this.RabbitMqueue);
                }
            }
        }

        /// <summary>
        /// 简单队列发送
        /// </summary>
        /// <param name="mqProducer">生产者</param>
        /// <param name="msg">消息</param>
        /// <param name="ifComfig">是否开启确认模式</param>
        /// <param name="mandatory">是否强制投递 true：投递失败将消息返回生产者； false：投递失败将消息丢失</param>
        /// <param name="properties">属性</param>
        public void BasicPublish(string msg, bool ifComfig, bool mandatory = false, Action successFunc = null, Action falseFunc = null)
        {
            AMQP((channel, mg) =>
            {
                if (string.IsNullOrEmpty(RabbitMqueue.RouteKey)) RabbitMqueue.RouteKey = mg.Name;
                var body = Encoding.UTF8.GetBytes(msg);
                if (ifComfig) channel.ConfirmSelect();
                channel.BasicPublish(RabbitMqueue.Exchange, RabbitMqueue.RouteKey, mandatory, null, body);
                if (ifComfig)
                {
                    if (channel.WaitForConfirms())
                    {
                        successFunc?.Invoke();
                    }
                    else
                    {
                        falseFunc?.Invoke();
                    }
                }
            });
        }

        /// <summary>
        /// 主题模式发送
        /// <param name="msg">消息</param>
        /// <param name="ifComfig">是否开启确认模式</param>
        /// <param name="mandatory">是否强制投递 true：投递失败将消息返回生产者； false：投递失败将消息丢失</param>
        /// <param name="properties">属性</param>
        public void TopicSend(string msg, bool ifComfig, bool mandatory = false, Action successFunc = null, Action falseFunc = null)
        {
            if (!string.IsNullOrEmpty(RabbitMqueue.RouteKey) && RabbitMqueue.ExchangeType == ExchangeType.Topic)
            {
                List<string> routeKeys = RabbitMqueue.RouteKey.Split(';').Distinct().ToList();
                AMQP((channel, mg) =>
                {
                    var body = Encoding.UTF8.GetBytes(msg);
                    if (ifComfig) channel.ConfirmSelect();
                    foreach (var item in routeKeys)
                    {
                        channel.BasicPublish(RabbitMqueue.Exchange, item, mandatory, null, body);
                    }
                    if (ifComfig)
                    {
                        if (channel.WaitForConfirms())
                        {
                            successFunc?.Invoke();
                        }
                        else
                        {
                            falseFunc?.Invoke();
                        }
                    }
                });
            }
            else
            {
                Console.WriteLine("主题模式必须设置路由键，路由键不能为空");
            }
        }
    }
}
