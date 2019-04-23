using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BovrilAuthentication.Controllers
{
	public class DiscordController : Controller
	{
		AuthenticationProperties properties = new AuthenticationProperties()
		{
			IsPersistent = true,
			RedirectUri = "/Discord/Done"
		};

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult SignIn()
		{
			return Challenge(properties, "Discord");
		}

		public IActionResult Done()
		{
			return View();
		}
	}
}
