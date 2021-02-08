using System;
using System.Data.SqlClient;

namespace Villain_Names
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

            using (connection)
            {
                var queryText = @" SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                FROM Villains AS v
                                JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                                GROUP BY v.Id, v.Name
                                HAVING COUNT(mv.VillainId) > 3
                                ORDER BY COUNT(mv.VillainId)";
                SqlCommand command = new SqlCommand(queryText, connection);
                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} - {reader["MinionsCount"]}");
                    }
                }
            }
        }
    }
}
