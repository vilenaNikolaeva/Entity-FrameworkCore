using System;
using System.Data.SqlClient;

namespace Remove_Villain
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
            var villainsId = int.Parse(Console.ReadLine());
            SqlConnection connection = new SqlConnection(String.Format(connectionString, DB_Name));
            connection.Open();

            using (connection)
            {
                var command = new SqlCommand($@"
                SELECT Name FROM Villains WHERE Id = {villainsId}", connection);
                var result =command.ExecuteScalar();

                if (result==null)
                {
                    Console.WriteLine("No such villain was found.");
                    connection.Close();
                    return;
                }
                else
                {
                    command = new SqlCommand($@" 
                    DELETE * FROM Villians
                        WHERE Id={villainsId}", connection);
                    Console.WriteLine($"{result} was deleted.");


                    command = new SqlCommand($@"
                     SELECT COUNT(*) FROM MinionsVillains 
                        WHERE VillainId = {villainsId}", connection);
                    result =(int)command.ExecuteScalar();

                    Console.WriteLine($"{result} minions were released.");

                    command = new SqlCommand($@"
                    DELETE  FROM MinionsVillains 
                        WHERE VillainId = {villainsId}", connection);
                }
            }
        }
    }
}
