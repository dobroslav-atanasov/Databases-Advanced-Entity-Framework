namespace IncreaseAgeStoredProcedure
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using VillainNames;

    public class StartUp
    {
        public static void Main()
        {
            int[] ids = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            SqlConnection connection = new SqlConnection(Configuration.ConnectionString);
            connection.Open();

            using (connection)
            {
                foreach (int id in ids)
                {
                    SqlCommand command = new SqlCommand("EXEC usp_GetOlder @id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }

                SqlCommand selectCommand = new SqlCommand("SELECT * FROM Minions", connection);
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Name"]} - {reader["Age"]} old years old");
                }
            }
        }
    }
}