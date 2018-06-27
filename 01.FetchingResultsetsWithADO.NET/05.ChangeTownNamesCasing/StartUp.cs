namespace ChangeTownNamesCasing
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using VillainNames;

    public class StartUp
    {
        public static void Main()
        {
            Console.Write("Enter the country: ");
            var searchedCountry = Console.ReadLine();

            var connection = new SqlConnection(Configuration.ConnectionString);
            connection.Open();

            using (connection)
            {
                var command = new SqlCommand("SELECT t.Name " +
                                             "FROM Countries c " +
                                             "INNER JOIN Towns AS t " +
                                             "ON t.CountryId = c.id " +
                                             "WHERE c.Name = @searchedCountry", connection);

                command.Parameters.AddWithValue("@searchedCountry", searchedCountry);
                var reader = command.ExecuteReader();

                var towns = new List<string>();
                while (reader.Read())
                {
                    var town = (string) reader["Name"];
                    towns.Add(town.ToUpper());
                }

                if (towns.Count != 0)
                {
                    Console.WriteLine($"{towns.Count} town names were affected.");
                    Console.WriteLine($"[{string.Join(", ", towns)}]");
                }
                else
                {
                    Console.WriteLine("No town names were affected.");
                }
            }
        }
    }
}