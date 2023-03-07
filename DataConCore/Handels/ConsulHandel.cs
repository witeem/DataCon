using System;
using Consul;
using DataConCore.Handels.HandelDto;

namespace DataConCore.Handels
{
	public static class ConsulHandel
	{
		public static void ConsulRegist(ConsulSetting setting)
		{
			ConsulClient client = new ConsulClient(c =>
			{
				c.Address = new Uri("http://localhost:8500/");
				c.Datacenter = "dc1";
			});

			client.Agent.ServiceRegister(new AgentServiceRegistration
			{
				ID = Guid.NewGuid().ToString(),
				Name =  setting.ServerName,
				Address = setting.Ip,
				Port = setting.Port,
				Tags = setting.Tags,
				Check = new AgentServiceCheck()
				{
					Interval = TimeSpan.FromSeconds(12),
                    HTTP = $"http://{setting.Ip}:{setting.Port}/healthz",
                    // HTTP = "http://127.0.0.1:5726/healthz",
                    Timeout = TimeSpan.FromSeconds(5),
					DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(30)
				}
			});
		}

		public static List<string> GetConsulServers(string serverName, string method)
		{
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500/");
                c.Datacenter = "dc1";
            });

			List<string> urls = new List<string>();
			var response = client.Agent.Services().Result.Response;
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

		public static List<string> GetConsulServersWithWeight(string serverName, string method)
		{
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500/");
                c.Datacenter = "dc1";
            });

            List<string> urls = new List<string>();
            var response = client.Agent.Services().Result.Response;
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

