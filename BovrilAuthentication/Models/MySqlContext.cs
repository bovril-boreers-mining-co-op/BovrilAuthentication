using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BovrilAuthentication.Models
{
	public class MySqlContext
	{
		public string ConnectionString { get; }

		public MySqlContext(string connectionString)
		{
			ConnectionString = connectionString;
		}

		MySqlConnection GetConnection()
		{
			return new MySqlConnection(ConnectionString);
		}

		public void AddUser(ulong eveId, ulong discordId)
		{
			using (MySqlConnection connection = GetConnection())
			{
				connection.Open();

				MySqlCommand cmd = new MySqlCommand($"insert into users(eve_id, discord_id) values({eveId}, {discordId});", connection);
				cmd.ExecuteNonQuery();
			}
		}

		//public bool UserExists(ulong eveId, ulong discordId)
		//{

		//}
	}
}