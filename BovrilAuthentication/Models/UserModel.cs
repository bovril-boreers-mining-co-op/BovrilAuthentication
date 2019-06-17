using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BovrilAuthentication.Models
{
	public class UserModel
	{
		public ulong EveID { get; set; }

		public ulong DiscordID { get; set; }

		[JsonIgnore]
		public bool Set
		{
			get
			{
				return eveSet && discordSet;
			}
		}

		public bool eveSet { get; set; } = false;
		public bool discordSet { get; set; } = false;

		public void SetEveID(ulong id)
		{
			EveID = id;
			eveSet = true;
		}

		public void SetDiscordID(ulong id)
		{
			DiscordID = id;
			discordSet = true;
		}
	}
}
