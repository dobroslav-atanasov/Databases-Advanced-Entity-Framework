namespace MinionNames
{
    using System;
    using System.Data.SqlClient;
    using VillainNames;

    public class StartUp
    {
        public static void Main()
        {
            Console.Write("Enter Villain ID: ");
            var villainId = int.Parse(Console.ReadLine());

            SqlConnection connection = new SqlConnection(Configuration.ConnectionString);
            connection.Open();

            using (connection)
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Villains AS v WHERE v.Id = @villainId", connection);
                command.Parameters.AddWithValue("@villainId", villainId);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        Console.WriteLine($"Villain: {reader["Name"]}");
                    reader.Close();

                    SqlCommand minionsCommand = new SqlCommand("SELECT m.Name, m.Age " +
                                                               "FROM Villains AS v " +
                                                               "INNER JOIN MinionsVillains AS mv " +
                                                               "ON mv.VillainId = v.Id " +
                                                               "INNER JOIN Minions AS m " +
                                                               "ON m.Id = mv.MinionId " +
                                                               "WHERE v.Id = @villainId " +
                                                               "ORDER BY m.Name ", connection);

                    minionsCommand.Parameters.AddWithValue("@villainId", villainId);
                    SqlDataReader minionReader = minionsCommand.ExecuteReader();
                    if (minionReader.HasRows)
                    {
                        var index = 1;
                        while (minionReader.Read())
                        {
                            Console.WriteLine($"{index}. {minionReader["Name"]} {minionReader["Age"]}");
                            index++;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"(no minions)");
                    }
                }
                else
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                }
            }
        }
    }
}