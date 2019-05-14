using AspNet.Security.OAuth.Discord;
using AspNet.Security.OAuth.EVEOnline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using BovrilAuthentication.Models;
using Microsoft.Extensions.Logging;

namespace BovrilAuthentication
{
	public class Startup
	{
		IConfiguration config;

		public Startup(IConfiguration config)
		{
			this.config = config;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			string conString = config.GetConnectionString("MySql");

			services.Add(new ServiceDescriptor(
				typeof(MySqlContext),
				new MySqlContext(conString)
			));

			services.AddAuthentication(x =>
			{
				x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
			.AddCookie()
			.AddEVEOnline(x =>
			{
				x.ClientId = config["EveConfig:ClientID"];
				x.ClientSecret = config["EveConfig:ClientSecret"];
			})
			.AddDiscord(x =>
			{
				x.ClientId = config["Discord:AppID"];
				x.ClientSecret = config["Discord:AppSecret"];
				x.Scope.Add(config["Discord:Scope"]);
			});

			services.AddMvc().AddNewtonsoftJson();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();

			app.UseBrowserLink();
			app.UseStaticFiles();

			app.UseRouting(routes =>
			{
				routes.MapControllerRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}"
				);
				routes.MapRazorPages();
			});
		}
	}
}
