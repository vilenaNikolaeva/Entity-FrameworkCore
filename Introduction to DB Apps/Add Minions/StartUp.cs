using System;
using System.Data.SqlClient;

namespace Add_Minions
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

            var minionInfo = Console.ReadLine().Split();
            var minionName = minionInfo[1];
            var minionAge = int.Parse(minionInfo[2]);
            var minionTown = minionInfo[3];

            var villianInfo = Console.ReadLine().Split();
            var villianName = villianInfo[1];

            connection.Open();
            using (connection)
            {
                var command = new SqlCommand($"SELECT Id FROM Towns WHERE Name ='{minionTown}'", connection);
                if (command.ExecuteScalar() == null)
                {
                    command = new SqlCommand($"INSERT INTO Towns (Name) VALUES ('{minionTown}')", connection);
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Town {minionTown} was added to the database.");
                }

                command = new SqlCommand($"SELECT Id FROM Villains WHERE Name = '{villianName}'", connection);
                if (command.ExecuteScalar()==null)
                {
                    command = new SqlCommand($"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES ('{villianName}', 4)", connection);
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Villian {villianName} was added to the database.");
                }

                command = new SqlCommand($"SELECT Id FROM Towns WHERE Name= '{minionTown}'", connection);
                var townId =(int)command.ExecuteScalar();

                command = new SqlCommand($"INSERT INTO Minions (Name, Age, TownId) VALUES ('{minionName}',{minionAge},{townId})", connection);
                command.ExecuteScalar();

                var minionID = new SqlCommand($"SELECT Id FROM Minions WHERE Name= '{minionName}'", connection);
                var mID=(int)minionID.ExecuteScalar();
                var villianID = new SqlCommand($"SELECT Id FROM Villains WHERE Name= '{villianName}'", connection);
                var vID=(int)villianID.ExecuteScalar();

                command = new SqlCommand($"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES ({mID},{vID})", connection);
                command.ExecuteNonQuery();
                Console.WriteLine($"Successfully added {minionName} to the minion of {villianName}");

            }
        }
       
    }
}
