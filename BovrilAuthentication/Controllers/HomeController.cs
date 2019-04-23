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
			ViewData["EveAuthUrl"] = "/Eve";
			ViewData["DiscordAuthUrl"] = "/Discord";
			//return View("Index", new HomeModel(false, false));
			return RedirectToAction("Index", "Eve");
		}
	}
}
