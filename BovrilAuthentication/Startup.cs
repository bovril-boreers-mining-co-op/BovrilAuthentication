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
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

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

			/*services.AddAuthentication(x =>
			{
				x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
			.AddCookie()
			.AddEVEOnline(x =>
			{
				x.ClientId = config["EveConfig:ClientID"];
				x.ClientSecret = config["EveConfig:ClientSecret"];
				//x.CallbackPath = new PathString(config["EveConfig:CallbackURL"]);
			})
			.AddDiscord(x =>
			{
				x.ClientId = config["Discord:AppID"];
				x.ClientSecret = config["Discord:AppSecret"];
				x.Scope.Add(config["Discord:Scope"]);
				//x.CallbackPath = new PathString(config["Discord:CallbackURL"]);
			});*/

			services.AddAuthentication(x =>
			{
				x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
			.AddCookie()
			.AddOAuth("EVEOnline", x =>
			{
				x.ClientId = config["EveConfig:ClientID"];
				x.ClientSecret = config["EveConfig:ClientSecret"];
				x.CallbackPath = new PathString("/signin-eveonline");

				x.AuthorizationEndpoint = "https://login.eveonline.com/oauth/authorize";
				x.TokenEndpoint = "https://login.eveonline.com/oauth/token";
				x.UserInformationEndpoint = "https://login.eveonline.com/oauth/verify";

				x.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "CharacterID");
				x.ClaimActions.MapJsonKey(ClaimTypes.Name, "CharacterName");
				x.ClaimActions.MapJsonKey(ClaimTypes.Expiration, "ExpiresOn");
				x.ClaimActions.MapJsonKey("urn:eveonline:scopes", "Scopes");

				x.Events = new OAuthEvents
				{
					OnCreatingTicket = async context =>
					{
						var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
						request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

						var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
						response.EnsureSuccessStatusCode();

						var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
						context.RunClaimActions(user);
					}
				};
			})
			.AddOAuth("Discord", x =>
			{
				x.ClientId = config["Discord:AppID"];
				x.ClientSecret = config["Discord:AppSecret"];
				x.Scope.Add(config["Discord:Scope"]);
				x.CallbackPath = new PathString("/signin-discord");

				x.AuthorizationEndpoint = "https://discordapp.com/api/oauth2/authorize";
				x.TokenEndpoint = "https://discordapp.com/api/oauth2/token";
				x.UserInformationEndpoint = "https://discordapp.com/api/users/@me";

				x.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
				x.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
				x.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

				x.Events = new OAuthEvents
				{
					OnCreatingTicket = async context =>
					{
						var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
						request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

						var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
						response.EnsureSuccessStatusCode();

						var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
						context.RunClaimActions(user);
					}
				};
			});

			services.AddLogging();
			services.AddMvc().AddJsonOptions(x => x.JsonSerializerOptions.WriteIndented = false);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
		{
			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				logger.AddFile(config["Log:Folder"]);
			}

			app.UseHsts();
			app.UseAuthentication();

			app.UseRouting();

			app.UseStaticFiles();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
