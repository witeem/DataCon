using System;
using Ocelot.Configuration.File;

namespace DataConGateway.Common;

public class OcelotRedis
{
	public OcelotGlobalConfig GlobalConfiguration { get; set; }

	public List<FileRoute> Routes { get; set; }


}

public class OcelotGlobalConfig
{
	public bool ReRouteIsCaseSensitive { get; set; } = false;

	public string RequestIdKey { get; set; } = "OcRequestId";

	public string BaseUrl { get; set; } = "http://localhost:5000";

	public FileHttpHandlerOptions HttpHandlerOptions { get; set; }

    public FileLoadBalancerOptions LoadBalancerOptions { get; set; }

	public FileQoSOptions QoSOptions { get; set; }

    public FileServiceDiscoveryProvider ServiceDiscoveryProvider { get; set; }
}

public class OcelotRoutes
{
	public string DownstreamPathTemplate { get; set; }

	public string DownstreamScheme { get; set; }

	public string UpstreamPathTemplate { get; set; }

	public List<string> UpstreamHttpMethod { get; set; } = new List<string> { "Get", "Post" };

	public bool UseServiceDiscovery { get; set; }

	public string ServiceName { get; set; }

	public int Priority { get; set; } = 0;

	public string RequestIdKey { get; set; }


    public FileLoadBalancerOptions LoadBalancerOptions { get; set; }

	public FileQoSOptions QoSOptions { get; set; }
}

public class OcelotBalancerOptions
{
	public string Type { get; set; } = "RoundRobin";
}