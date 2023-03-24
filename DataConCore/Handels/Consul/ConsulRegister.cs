using Consul;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Handels.Consul
{
    public class ConsulRegister
    {
        private static ConsulClient client;
        private static object flag = new object();

        public static ConsulClient Init(ConsulOptions options)
        {
            if (client == null)
            {
                lock (flag)
                {
                    if (client == null)
                    {
                        client = new ConsulClient(c =>
                        {
                            c.Address = new Uri($"{options.ConsulPath}:{options.ConsulPort}");
                            c.Datacenter = string.IsNullOrEmpty(options.ConsulCenter) ? "dc1" : options.ConsulCenter;
                        });
                    }
                }
            }

            client.Agent.ServiceRegister(new AgentServiceRegistration
            {
                ID = $"service:{options.ServicePath}:{options.ServicePort}", //服务ID，一个服务是唯一的
                Name = options.ServiceName,
                Address = options.ServicePath,
                Port = options.ServicePort,
                Tags = options.Tags,
                Check = new AgentServiceCheck()
                {
                    // GRPC = $"{ip}:{port}", //gRPC注册特有
                    // GRPCUseTLS = false,//支持http
                    Interval = TimeSpan.FromSeconds(options.HealthzInterval),
                    HTTP = $"http://{options.ServicePath}:{options.ServicePort}/healthz",
                    Timeout = TimeSpan.FromSeconds(5), //超时时间
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10) //多久检查一次心跳
                }
            });

            return client;
        }
    }
}
