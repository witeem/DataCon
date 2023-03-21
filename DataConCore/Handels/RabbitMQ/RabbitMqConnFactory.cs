using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Handels.RabbitMQ
{
    public class RabbitMqConnFactory
    {

        public static IConnection GetConnector(RabbitMqOptions options)
        {
            return options.Hosts.Count == 1 ? GetConnection(options) : GetClusterConnection(options);
        }

        #region 私有方法
        private static IConnection GetConnection(RabbitMqOptions options)
        {
            var host = options.Hosts.FirstOrDefault();
            var factory = new ConnectionFactory
            {
                HostName = host.Host,
                Port = host.Port,
                UserName = options.Connector.UserName,
                Password = options.Connector.Password,
                DispatchConsumersAsync = options.Connector.ConsumersAsunc,
                AutomaticRecoveryEnabled = true,
                // VirtualHost = options.Connector.VirtualHost
            };

            return factory.CreateConnection();
        }

        private static IConnection GetClusterConnection(RabbitMqOptions options)
        {
            var factory = new ConnectionFactory
            {
                UserName = options.Connector.UserName, // 账户
                Password = options.Connector.Password, // 密码
                DispatchConsumersAsync = options.Connector.ConsumersAsunc, // 支持异步发送消息
                VirtualHost = options.Connector.VirtualHost   // 虚拟主机
            };
            var list = options.Hosts.Select(s => new AmqpTcpEndpoint
            {
                HostName = s.Host,
                Port = s.Port,
            });

            return factory.CreateConnection(list.ToList());
        }
        #endregion
    }
}
