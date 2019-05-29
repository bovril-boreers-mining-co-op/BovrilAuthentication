using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

		public void ReplaceEve(ulong eveId, ulong discordId)
		{
			using (MySqlConnection connection = GetConnection())
			{
				connection.Open();

				MySqlCommand cmd = new MySqlCommand($"update users set eve_id = {eveId} where discord_id = {discordId} limit 1;", connection);
				cmd.ExecuteNonQuery();
			}
		}

		public bool EveUserExists(ulong eveId)
		{
			string result;
			using (MySqlConnection connection = GetConnection())
			{
				connection.Open();

				MySqlCommand cmd = new MySqlCommand($"select exists(select * from users where eve_id = {eveId} limit 1);", connection);
				result = cmd.ExecuteScalar().ToString();
			}

			return int.Parse(result) == 1;
		}

		public bool DiscordUserExists(ulong discordId)
		{
			string result;
			using (MySqlConnection connection = GetConnection())
			{
				connection.Open();

				MySqlCommand cmd = new MySqlCommand($"select exists(select * from users where discord_id = {discordId} limit 1);", connection);
				result = cmd.ExecuteScalar().ToString();
			}

			return int.Parse(result) == 1;
		}

		public bool UserExists(ulong eveId, ulong discordId)
		{
			return EveUserExists(eveId) && DiscordUserExists(discordId);
		}
	}
}