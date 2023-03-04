using System;
namespace DataConCore.Handels
{
	public static class ApiHandel
	{
		public static string InvokeApi(string uri, HttpMethod method)
		{
			using (HttpClient client = new HttpClient())
			{
				HttpRequestMessage message = new HttpRequestMessage();
				message.Method = method;
				message.RequestUri = new Uri(uri);				
				var result = client.SendAsync(message).Result;
				string content = result.Content.ReadAsStringAsync().Result;
				return content;

            }
		}
	}
}

