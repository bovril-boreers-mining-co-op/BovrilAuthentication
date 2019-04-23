using AspNet.Security.OAuth.EVEOnline;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
			return RedirectToAction("Index", "Discord");
		}
	}
}
