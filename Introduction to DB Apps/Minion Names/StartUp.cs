using System;
using System.Data.SqlClient;

namespace Minion_Names
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
            var villianID = int.Parse(Console.ReadLine());
            SqlConnection connection = new SqlConnection(String.Format(connectionString, DB_Name));
            connection.Open();

            using (connection)
            {
                var command = new SqlCommand($"SELECT Name FROM Villains WHERE Id = {villianID}",connection);
                var villianName = (string)command.ExecuteScalar();

                if (villianName==null)
                {
                    Console.WriteLine($"No villain with ID {villianID} exists in the database.");
                    return;
                }
                Console.WriteLine($"Villian: {villianName}");
                command = new SqlCommand($@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                        JOIN Minions As m ON mv.MinionId = m.Id
                                        WHERE mv.VillainId = {villianID}
                                        ORDER BY m.Name" ,connection);

                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("(no minions)");
                        reader.Close();
                        connection.Close();
                        return;
                    }

                    while (reader.Read())
                    {
                        var minionName = reader["Name"];
                        var rowNumber = reader["RowNum"];
                        var minionAge = reader["Age"];
                        Console.WriteLine($"{rowNumber}. {minionName} {minionAge}");
                    }
                    
                }
            }
        }
    }
}
