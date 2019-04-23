using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BovrilAuthentication.Models
{
	public class HomeModel
	{
		public bool EveAuthed { get; }

		public bool DiscordAuthed { get; }

		public HomeModel(bool eveAuthed, bool discordAuthed)
		{
			EveAuthed = eveAuthed;
			DiscordAuthed = discordAuthed;
		}
	}
}
