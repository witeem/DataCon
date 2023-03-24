using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConCore.Handels.Consul
{
    public class ConsulOptions
    {
        /// <summary>
        /// Consul服务地址
        /// </summary>
        public string ConsulPath { get; set; }

        /// <summary>
        /// Consul服务端口
        /// </summary>
        public string ConsulPort { get; set; }

        /// <summary>
        /// Consul服务数据中心
        /// </summary>
        public string ConsulCenter { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServicePath { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServicePort { get; set; }

        /// <summary>
        /// 多久检查一次心跳 (s)
        /// </summary>
        public int HealthzInterval { get; set; }

        /// <summary>
        /// 配置标签
        /// </summary>
        public string[] Tags { get; set; }
    }
}
