using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Print_All_MInion_Names
{
    class StartUp
    {
        private static string connectionString =
           "Server=(localdb)\\MSSQLLocalDB;" +
           "Database={0};" +
           "Integrated Security=true";
        private const string DB_Name = "MinionsDB";

        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(String.Format(connectionString, DB_Name));
            connection.Open();
            var minionNames = new List<string>();
            var result = new List<string>();

            using (connection)
            {
                var command = new SqlCommand($@"
                 SELECT [Name] FROM Minions", connection);
                var reader = command.ExecuteReader();

                if (reader==null)
                {
                    reader.Close();
                    command.Clone();
                    return;
                }
                else
                {
                    while (reader.Read())
                    {
                        minionNames.Add((string)reader["Name"]);
                    }
                }

            }
            while (minionNames.Any())
            {
                result.Add(minionNames[0]);
                minionNames.RemoveAt(0);

                if (minionNames.Count!=0)
                {
                    result.Add(minionNames[minionNames.Count - 1]);
                    minionNames.RemoveAt(minionNames.Count - 1);
                }
            }

            result.ForEach(n => Console.WriteLine(n));
        }
    }
}
