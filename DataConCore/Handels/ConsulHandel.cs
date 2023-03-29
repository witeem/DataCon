using Consul;

namespace DataConCore.Handels
{
    public static class ConsulHandel
	{
		public static List<string> GetConsulServers(string consulService, string datacenter, string serverName, string method)
		{
			var consulClient = new ConsulClient(c =>
			{
				c.Address = new Uri(consulService);
				c.Datacenter = datacenter;
			});

			List<string> urls = new List<string>();
			var response = consulClient.Agent.Services().Result.Response;
			if (response != null)
			{
				var servers = response.Where(m => m.Value.Service.Equals(serverName, StringComparison.OrdinalIgnoreCase)).ToArray();
				foreach (var item in servers)
				{
					urls.Add($"http://{item.Value.Address}:{item.Value.Port}/{method}");
                }
			}

			return urls;
        }

		public static List<string> GetConsulServersWithWeight(string consulService, string datacenter, string serverName, string method)
		{
			var consulClient = new ConsulClient(c =>
			{
				c.Address = new Uri(consulService);
				c.Datacenter = datacenter;
			});

			List<string> urls = new List<string>();
            var response = consulClient.Agent.Services().Result.Response;
            if (response != null)
            {
                var servers = response.Where(m => m.Value.Service.Equals(serverName, StringComparison.OrdinalIgnoreCase)).ToArray();
                foreach (var item in servers)
                {
					if (item.Value.Tags?[0] != null)
					{
						int itemCount = int.Parse(item.Value.Tags[0]);
						for (int i = 0; i < itemCount; i++)
						{
                            urls.Add($"http://{item.Value.Address}:{item.Value.Port}/{method}");
                        }
					}                   
                }
            }

            return urls;
        }
	}
}

