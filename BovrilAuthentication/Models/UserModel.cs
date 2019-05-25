using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BovrilAuthentication.Models
{
	public class UserModel
	{
		public ulong EveID { get; set; }

		public ulong DiscordID { get; set; }

		public bool Set
		{
			get
			{
				return eveSet && discordSet;
			}
		}

		[JsonProperty]
		bool eveSet = false;
		[JsonProperty]
		bool discordSet = false;

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
