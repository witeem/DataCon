using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Handels.RabbitMQ
{
    public interface IRabbitMqProducer
    {
        /// <summary>
        /// 自定义发送
        /// </summary>
        /// <param name="action"></param>
        void AMQP(Action<IModel, RabbitMqQueue> action);

        /// <summary>
        /// 简单队列发送
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ifComfig">是否开启确认模式</param>
        /// <param name="mandatory">是否强制投递 true：投递失败将消息返回生产者； false：投递失败将消息丢失</param>
        void BasicPublish(string msg, bool ifComfig, bool mandatory = false, Action successFunc = null, Action falseFunc = null);

        /// <summary>
        /// 主题模式发送
        /// <param name="msg">消息</param>
        /// <param name="ifComfig">是否开启确认模式</param>
        /// <param name="mandatory">是否强制投递 true：投递失败将消息返回生产者； false：投递失败将消息丢失</param>
        void TopicSend(string msg, bool ifComfig, bool mandatory = false, Action successFunc = null, Action falseFunc = null);
    }
}
