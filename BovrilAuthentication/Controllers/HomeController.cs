using EveOpenApi;
using EveOpenApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BovrilAuthentication.Controllers
{
	public class HomeController : Controller
	{
		EveWebLogin login;

		public HomeController(ILogin login)
		{
			this.login = login as EveWebLogin;
		}

		public IActionResult Index()
		{
			(string authUrl, string state) = login.GetAuthUrl((Scope)"");

			ViewData["AuthURL"] = authUrl;
			ViewData["Chars"] = login.GetUsers().Count;
			return View();
		}

		public async Task<IActionResult> Callback([FromQuery]string code)
		{
			await login.AddToken((Scope)"", code);
			ViewData["Chars"] = login.GetUsers().Count;
			return View("Index");
		}

	}
}
