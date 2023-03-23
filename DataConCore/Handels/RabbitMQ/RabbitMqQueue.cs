using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Handels.RabbitMQ
{
    public class RabbitMqQueue
    {
        public static RabbitMqQueue Configure(Action<RabbitMqQueue> config)
        {
            var msg = new RabbitMqQueue();
            config?.Invoke(msg);
            return msg;
        }

        /// <summary>
        /// 队列名称 个路由名称 通过;分割
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 路由key 多个路由KEY 通过;分割
        /// </summary>
        public string RouteKey { get; set; }

        /// <summary>
        /// 是否持久化
        /// </summary>
        public bool Durable { get; set; }

        /// <summary>
        /// 绑定参数
        /// </summary>
        public IDictionary<string, object> Args { get; set; }

        /// <summary>
        /// 路由交换机
        /// </summary>
        public string Exchange { get; set; } = "";

        /// <summary>
        /// 路由交换机 类型
        /// </summary>
        public string ExchangeType { get; set; }
    }
}
