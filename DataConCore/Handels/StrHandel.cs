using System;
using Newtonsoft.Json;
namespace DataConCore.Handels;

public static class StrHandel
{
	public static T ToObject<T>(this string value)
	{
		if (string.IsNullOrEmpty(value)) return default(T);
		return JsonConvert.DeserializeObject<T>(value);
	}
}

