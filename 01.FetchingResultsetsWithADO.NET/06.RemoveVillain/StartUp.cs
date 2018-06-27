namespace RemoveVillain
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
            int villainId = int.Parse(Console.ReadLine());

            using (connection)
            {
                SqlCommand nameCommand = new SqlCommand("SELECT * FROM Villains WHERE Id = @villainId", connection);
                nameCommand.Parameters.AddWithValue("@villainId", villainId);
                SqlDataReader nameReader = nameCommand.ExecuteReader();
                string villainName = string.Empty;
                using (nameReader)
                {
                    while (nameReader.Read())
                    {
                        villainName = (string)nameReader["Name"];
                    }
                }

                if (string.IsNullOrWhiteSpace(villainName))
                {
                    throw new ArgumentException("No such villain was found.");
                }

                SqlCommand mvcommand = new SqlCommand("DELETE FROM MinionsVillains WHERE VillainId = @villainId",
                    connection);
                mvcommand.Parameters.AddWithValue("@villainId", villainId);
                int releasedMinions = mvcommand.ExecuteNonQuery();

                SqlCommand villainCommand =
                    new SqlCommand("DELETE FROM Villians WHERE Id = @villainId", connection);
                villainCommand.Parameters.AddWithValue("@villainId", villainCommand);
                villainCommand.ExecuteNonQuery();

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{releasedMinions} minions were released.");
            }
        }
    }
}
