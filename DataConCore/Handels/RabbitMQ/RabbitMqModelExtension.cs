using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Handels.RabbitMQ
{
    public static class RabbitMqModelExtension
    {
        /// <summary>
        /// 声明一个交换机
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mg"></param>
        /// <returns></returns>
        public static IModel WithExchange(this IModel channel, RabbitMqQueue mg)
        {
            if (!string.IsNullOrEmpty(mg.Exchange) && !string.IsNullOrEmpty(mg.ExchangeType))
                channel.ExchangeDeclare(mg.Exchange, mg.ExchangeType, true, false);

            return channel;
        }

        /// <summary>
        /// 声明一个队列
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mg"></param>
        /// <returns></returns>
        public static IModel WithQueueDeclare(this IModel channel, RabbitMqQueue mg)
        {
            var queues = mg.Name.Split(",");
            foreach (var name in queues)
            {
                channel.QueueDeclare(name, false, false, false, mg.Args);
            }
            return channel;
        }

        /// <summary>
        /// 队列绑定routingKey
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mg"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IModel WithQueueBind(this IModel channel, RabbitMqQueue mg)
        {
            if (!string.IsNullOrEmpty(mg.Exchange))
            {
                var queues = mg.Name.Split(";");
                var routes = mg.RouteKey.Split(";");

                var isNeedRouteKey = mg.ExchangeType == ExchangeType.Direct || mg.Exchange == ExchangeType.Topic;
                if (isNeedRouteKey && queues.Length != routes.Length)
                {
                    throw new ArgumentException("路由数量和路由键数量不相等.");
                }

                for (int i = 0, len = queues.Length; i < len; i++)
                {
                    if (isNeedRouteKey)
                    {
                        channel.QueueBind(queue: queues[i], exchange: mg.Exchange, routingKey: routes[i]);
                    }
                    else
                    {
                        channel.QueueBind(queue: queues[i], exchange: mg.Exchange, routingKey: "");
                    }
                }
            }
            return channel;
        }
    }
}
