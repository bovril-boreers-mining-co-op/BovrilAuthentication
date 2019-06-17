using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BovrilAuthentication.Extensions
{
	public static class TempDataExtensions
	{
		public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
		{
			tempData[key] = JsonSerializer.ToString(value);
		}

		public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
		{
			tempData.TryGetValue(key, out object o);
			return o is null ? null : JsonSerializer.Parse<T>((string)o);
		}
	}
}
