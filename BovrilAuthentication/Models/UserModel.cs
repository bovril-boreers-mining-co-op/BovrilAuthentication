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

		public string Test { get; set; }

		public UserModel(string test)
		{
			Test = test;
		}

		public void SetEveID(ulong id)
		{
			EveID = id;
		}

		public void SetDiscordID(ulong id)
		{
			DiscordID = id;
		}
	}
}
