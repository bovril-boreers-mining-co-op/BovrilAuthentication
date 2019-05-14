using AspNet.Security.OAuth.Discord;
using BovrilAuthentication.Extensions;
using BovrilAuthentication.Models;
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
			UserModel model = TempData.Get<UserModel>("UserModel");
			ulong.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out ulong id);
			model.SetDiscordID(id);
			TempData.Put("UserModel", model);

			return RedirectToAction("Done", "Home");
		}
	}
}
