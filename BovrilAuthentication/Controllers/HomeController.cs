using BovrilAuthentication.Extensions;
using BovrilAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BovrilAuthentication.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			TempData.Put("UserModel", new UserModel("Hei"));

			ViewData["EveAuthUrl"] = "/Eve";
			ViewData["DiscordAuthUrl"] = "/Discord";
			return RedirectToAction("Index", "Eve");
		}

		public IActionResult Done()
		{
			MySqlContext database = HttpContext.RequestServices.GetService(typeof(MySqlContext)) as MySqlContext;
			UserModel user = TempData.Get<UserModel>("UserModel");

			database.AddUser(user.EveID, user.DiscordID);
			return View(user);
		}
	}
}