using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Increase_Minion_Age
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
            var minionsIds = Console.ReadLine().Split().Select(int.Parse).ToArray();

            var connection = new SqlConnection((String.Format(connectionString, "master")));
            connection.Open();

            var minionsId = new List<int>();
            SqlCommand command;
            SqlDataReader reader;

            using (connection)
            {
                 command = new SqlCommand($@"
                    SELECT * FROM Minions WHERE Id IN 
                    ({String.Join(", ", minionsIds)})", connection);
                 reader = command.ExecuteReader();

                if (reader == null)
                {
                    reader.Close();
                    connection.Close();
                    return;
                }
                while (reader.Read())
                {
                    minionsId.Add((int)reader["Id"]);
                }
                reader.Close();

                for (int i = 0; i < minionsId.Count; i++)
                {
                    var id = minionsId[i];
                    command = new SqlCommand($@"
                     UPDATE Minions
                    SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                    WHERE Id = {id}", connection);

                    command = new SqlCommand($@"
                    SELECT Name, Age 
                    FROM Minions", connection);
                    var result = command.ExecuteReader();

                    while (result.Read())
                    {
                        Console.WriteLine($"{(string)result["Name"]} {(int)result["Age"]}");
                    }
                }
            }//80%
        }
    }
}
