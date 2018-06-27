namespace AddMinion
{
    using System;
    using System.Data.SqlClient;
    using VillainNames;

    public class StartUp
    {
        public static void Main()
        {
            SqlConnection connection = new SqlConnection(Configuration.ConnectionString);

            connection.Open();

            string[] minionInput = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            string newMinionName = minionInput[1];
            int newMinionAge = int.Parse(minionInput[2]);
            string newMinionTown = minionInput[3];
            string[] villainInput = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            string villainName = villainInput[1];

            using (connection)
            {
                int townId = FindTown(connection, newMinionTown);
                int villainId = FindVillain(connection, villainName);

                if (townId == 0)
                {
                    InsertNewTown(connection, newMinionTown);
                    townId = FindTown(connection, newMinionTown);
                }

                if (villainId == 0)
                {
                    InsertNewVillain(connection, villainName);
                    villainId = FindVillain(connection, villainName);
                }

                InsertNewMinion(connection, townId, newMinionName, newMinionAge);
                int minionId = FindMinionId(connection, newMinionName);
                CreateConnectionBetweenVillainAndMinion(connection, minionId, villainId, newMinionName, villainName);
            }
        }

        private static void CreateConnectionBetweenVillainAndMinion(SqlConnection connection, int minionId,
            int villainId, string newMinionName, string villainName)
        {
            var command =
                new SqlCommand("INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)",
                    connection);
            command.Parameters.AddWithValue("@minionId", minionId);
            command.Parameters.AddWithValue("@villainId", villainId);
            command.ExecuteNonQuery();
            Console.WriteLine($"Successfully added {newMinionName} to be minion of {villainName}.");
        }

        private static int FindMinionId(SqlConnection connection, string newMinionName)
        {
            var minionCommand = new SqlCommand("SELECT * FROM Minions WHERE Name = @newMinionName", connection);
            minionCommand.Parameters.AddWithValue("@newMinionName", newMinionName);
            var minionReader = minionCommand.ExecuteReader();
            using (minionReader)
            {
                var minionId = 0;
                while (minionReader.Read())
                    minionId = (int) minionReader["Id"];
                return minionId;
            }
        }

        private static void InsertNewMinion(SqlConnection connection, int townId, string newMinionName,
            int newMinionAge)
        {
            var minionCommand =
                new SqlCommand(
                    "INSERT INTO Minions (Name, Age, TownId) VALUES (@newMinionName, @newMinionAge, @townId)",
                    connection);
            minionCommand.Parameters.AddWithValue("@townId", townId);
            minionCommand.Parameters.AddWithValue("@newMinionName", newMinionName);
            minionCommand.Parameters.AddWithValue("@newMinionAge", newMinionAge);
            minionCommand.ExecuteNonQuery();
        }

        private static void InsertNewVillain(SqlConnection connection, string villainName)
        {
            var villainCommand =
                new SqlCommand("INSERT INTO Villains (Name, EvilnessFactorId) VALUES (@villainName, 4)", connection);
            villainCommand.Parameters.AddWithValue("@villainName", villainName);
            villainCommand.ExecuteNonQuery();
            Console.WriteLine($"Villain {villainName} was added to the database.");
        }

        private static void InsertNewTown(SqlConnection connection, string newMinionTown)
        {
            var townCommand = new SqlCommand("INSERT INTO Towns (Name, CountryId) VALUES (@newMinionTown, 5)",
                connection);
            townCommand.Parameters.AddWithValue("@newMinionTown", newMinionTown);
            townCommand.ExecuteNonQuery();
            Console.WriteLine($"Town {newMinionTown} was added to the database.");
        }

        private static int FindVillain(SqlConnection connection, string villainName)
        {
            var villainCommand = new SqlCommand("SELECT * FROM Villains WHERE Name = @villainName", connection);
            villainCommand.Parameters.AddWithValue("@villainName", villainName);
            var villainReader = villainCommand.ExecuteReader();
            using (villainReader)
            {
                if (villainReader.HasRows)
                {
                    var villainId = 0;
                    while (villainReader.Read())
                        villainId = (int) villainReader["Id"];
                    return villainId;
                }
                return 0;
            }
        }

        private static int FindTown(SqlConnection connection, string newMinionTown)
        {
            var townCommand = new SqlCommand("SELECT * FROM Towns WHERE Name = @newMinionTown", connection);
            townCommand.Parameters.AddWithValue("@newMinionTown", newMinionTown);
            var townReader = townCommand.ExecuteReader();
            using (townReader)
            {
                if (townReader.HasRows)
                {
                    var townId = 0;
                    while (townReader.Read())
                        townId = (int) townReader["Id"];
                    return townId;
                }
                return 0;
            }
        }
    }
}