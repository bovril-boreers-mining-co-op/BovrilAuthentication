using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BovrilAuthentication
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseKestrel(options =>
					{
						// Configure the Url and ports to bind to
						// This overrides calls to UseUrls and the ASPNETCORE_URLS environment variable, but will be 
						// overridden if you call UseIisIntegration() and host behind IIS/IIS Express
						options.Listen(IPAddress.Loopback, 5000);
						options.Listen(IPAddress.Loopback, 5001, listenOptions =>
						{
							listenOptions.UseHttps("localhost.pfx", "testpassword");
						});
					});
					webBuilder.UseStartup<Startup>();
				});
	}
}
