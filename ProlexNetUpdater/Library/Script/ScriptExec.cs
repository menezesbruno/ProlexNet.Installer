using ProlexNetUpdater.Models;
using System;
using System.Data.SqlClient;
using System.IO;

namespace ProlexNetUpdater.Library.Script
{
    public static class ScriptExec
    {
        public static void Run(string file, Scripts item)
        {
            var script = ReadFile(file);
            var version = item.Version;
            var computerName = Environment.GetEnvironmentVariable("COMPUTERNAME");

            string queryString = $"SELECT TOP(1) Version FROM [dbo].[dbVersion] WHERE Version < {version} ORDER BY Version DESC";
            string connectionString = $"Server={computerName}\\SQLEXPRESS;Database=ProlexNet;User Id=prolexnet;Password=Admin@13;MultipleActiveResultSets=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var scriptExecution = new SqlCommand(script, connection);
                    scriptExecution.BeginExecuteNonQuery();
                }

                reader.Close();
            }
        }

        public static string GetState()
        {
            var computerName = Environment.GetEnvironmentVariable("COMPUTERNAME");

            string queryString = $"SELECT TOP(1) Address_StateAcronym FROM Office";
            string connectionString = $"Server={computerName}\\SQLEXPRESS; Database=ProlexNet; user id=prolexnet; password=Admin@13; MultipleActiveResultSets=true";
            var result = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = (string)reader[0];
                }

                reader.Close();
            }

            return result;
        }

        public static string ReadFile(string file)
        {
            var readFile = File.ReadAllText(file);
            return readFile;
        }
    }
}