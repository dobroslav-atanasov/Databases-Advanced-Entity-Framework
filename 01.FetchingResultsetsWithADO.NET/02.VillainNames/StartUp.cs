namespace VillainNames
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            SqlConnection connection = new SqlConnection(Configuration.ConnectionString);
            connection.Open();

            using (connection)
            {
                List<Minion> minions = new List<Minion>();

                SqlCommand command = new SqlCommand("SELECT v.Name AS [VillainName], m.Name AS[MinionName] " +
                                                    "FROM Villains v " +
                                                    "INNER JOIN MinionsVillains mv " +
                                                    "ON mv.VillainId = v.Id " +
                                                    "INNER JOIN Minions m " +
                                                    "ON m.Id = mv.MinionId", connection);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var minionName = (string) reader["MinionName"];
                    var villainName = (string) reader["VillainName"];
                    var minion = new Minion(minionName, villainName);
                    minions.Add(minion);
                }

                List<IGrouping<string, Minion>> groupMinions = minions
                    .GroupBy(m => m.VillainName)
                    .OrderByDescending(v => v.Count())
                    .ToList();

                foreach (IGrouping<string, Minion> group in groupMinions)
                    Console.WriteLine($"{group.Key} - {group.Count()}");
            }
        }
    }
}