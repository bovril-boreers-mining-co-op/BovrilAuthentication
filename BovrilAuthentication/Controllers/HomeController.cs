using BovrilAuthentication.Extensions;
using BovrilAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BovrilAuthentication.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			TempData.Put("UserModel", new UserModel());
			return RedirectToAction("Index", "Eve");
		}

		public IActionResult Error()
		{
			return View();
		}

		public async Task<IActionResult> AddUser()
		{
			MySqlContext database = HttpContext.RequestServices.GetService(typeof(MySqlContext)) as MySqlContext;
			UserModel user = GetUserMode();
			TempData.Put("UserModel", user);

			if (database.UserExists(user.EveID, user.DiscordID))
				return View("Exists");

			if (database.DiscordUserExists(user.DiscordID))
				return View("ExistsDiscord");

			if (database.EveUserExists(user.EveID))
				return View("ExistsEve");

			database.AddUser(user.EveID, user.DiscordID);

			await SendMessage();
			return RedirectToAction("Done");
		}

		public IActionResult ReplaceUser()
		{
			MySqlContext database = HttpContext.RequestServices.GetService(typeof(MySqlContext)) as MySqlContext;
			UserModel user = GetUserMode();

			if (!database.DiscordUserExists(user.DiscordID))
				return RedirectToAction("AddUser");

			database.ReplaceEve(user.EveID, user.DiscordID);
			return RedirectToAction("Done");
		}

		public IActionResult Done()
		{
			return View("Done");
		}

		UserModel GetUserMode()
		{
			UserModel user = TempData.Get<UserModel>("UserModel");

			if (user is null)
				throw new NullReferenceException("User model not initialized");

			if (!user.Set)
				throw new Exception($"User model credentials not set. {user.DiscordID} {user.EveID}");

			return user;
		}

		async Task SendMessage()
		{
			IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback, 55766);

			Socket client = new Socket(IPAddress.Loopback.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

			await client.ConnectAsync(localEndPoint);

			Memory<byte> data = Encoding.ASCII.GetBytes("Namecheck");

			await client.SendAsync(data, SocketFlags.None);
			client.Shutdown(SocketShutdown.Both);
			client.Close();
		}
	}
}