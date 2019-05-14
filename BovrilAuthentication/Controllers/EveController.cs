using AspNet.Security.OAuth.EVEOnline;
using BovrilAuthentication.Extensions;
using BovrilAuthentication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BovrilAuthentication.Controllers
{
	public class EveController : Controller
	{
		AuthenticationProperties properties = new AuthenticationProperties()
		{
			IsPersistent = true,
			RedirectUri = "Eve/Done"
		};

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult SignIn()
		{
			return Challenge(properties, "EVEOnline");
		}

		public IActionResult Done()
		{
			UserModel model = TempData.Get<UserModel>("UserModel");
			uint.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out uint id);
			model.SetEveID(id);
			TempData.Put("UserModel", model);

			return RedirectToAction("Index", "Discord");
		}
	}
}
