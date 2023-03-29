using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConGateway.Common
{
    public class OcelotEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        public string GatewayName { get; set; }

        public string BaseUrl { get; set; }

        public string DownstreamScheme { get; set; }

        public string RequestIdKey { get; set; }

        public string HttpHandlerOptions { get; set; }

        public string LoadBalancerOptions { get; set; }

        public string QoSOptions { get; set; }

        public string ServiceDiscoveryProvider { get; set; }

        public DateTime CreateTime { get; set; }

        public int IsDel { get; set; }
    }
}
