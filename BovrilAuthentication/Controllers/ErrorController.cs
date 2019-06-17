using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BovrilAuthentication.Controllers
{
	public class ErrorController : Controller
	{
		ILogger<ErrorController> log;

		public ErrorController(ILogger<ErrorController> log)
		{
			this.log = log;
		}

		public IActionResult Index()
		{
			Exception ex = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

			Guid guid = Guid.NewGuid();
			if (ex != null)
			{
				log.LogWarning(guid.ToString());
				log.LogError(ex, guid.ToString());
			}

			return View("Error", guid.ToString());
		}
	}
}
