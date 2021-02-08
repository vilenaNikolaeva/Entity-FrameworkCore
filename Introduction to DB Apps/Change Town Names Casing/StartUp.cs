using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Change_Town_Names_Casing
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
            var country = Console.ReadLine();
            SqlConnection connection = new SqlConnection(String.Format(connectionString, DB_Name));
            connection.Open();

            using (connection)
            {
                var command =new SqlCommand($@"
                    UPDATE Towns
                    SET Name = UPPER(Name)
                    WHERE CountryCode = (
                    SELECT c.Id FROM Countries AS c 
                    WHERE c.Name = '{country}')", connection);
                var townAffected = command.ExecuteNonQuery();


                if (townAffected==0)
                {
                    Console.WriteLine("No town names were affected.");
                    connection.Close();
                    return;
                }
                else
                {
                    Console.WriteLine($"{townAffected} towns names was affected");
                     command = new SqlCommand($@"
                        SELECT [Name] FROM Towns
                        WHERE CountryCode= (SELECT c.Id FROM Countries AS c
                        WHERE c.Name='{country}')", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    using(reader)
                    {
                        var nameList = new List<string>();
                        while (reader.Read())
                        {
                            var townName =(string)reader["Name"];
                            nameList.Add(townName);
                        }
                        Console.WriteLine($"[{(string.Join(", ",nameList))}]");
                    }
                }
            }
        }
    }
}
