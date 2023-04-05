using System;
using SqlSugar;
using System.Security.Principal;

namespace DataConGateway.Common;

public class OcelotRouteEntity
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }

    public string DownstreamPathTemplate { get; set; }
    public string DownstreamScheme { get; set; }
    public string Key { get; set; }
    public int Priority { get; set; }
    public string RequestIdKey { get; set; }
    public string ServiceName { get; set; }
    public string UpstreamHost { get; set; }
    public string UpstreamHttpMethod { get; set; }
    public string UpstreamPathTemplate { get; set; }
    public string LoadBalancerOptions { get; set; }
    public string QoSOptions { get; set; }
    public DateTime CreateTime { get; set; }
    public int IsDel { get; set; }
}

