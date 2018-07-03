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
            string connectionString = $"{computerName}\\SQLEXPRESS;Database=ProlexNet;User Id=prolexnet;Password=Admin@13;MultipleActiveResultSets=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    SqlCommand scriptExecution = new SqlCommand(script, connection);
                    scriptExecution.BeginExecuteNonQuery();
                }

                reader.Close();
            }
        }

        public static string ReadFile(string file)
        {
            var readFile = File.ReadAllText(file);
            return readFile;
        }
    }
}