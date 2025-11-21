using Devart.Data.PostgreSql;

class Program
{
  static void Main()
  {
    string connectionString = "" +
    "Host=127.0.0.1;" +
    "Port=5432;" +
    "User Id=TestUser;" +
    "Password=TestPassword;" +
    "Database=sakila;" +
    "Schema=public;" +
    "LicenseKey=**********";

    using (PgSqlConnection connection = new PgSqlConnection(connectionString))
    {
      try
      {
        connection.Open();
        Console.WriteLine("Connection to PostgreSQL successful!");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"PostgreSQL Error: {ex.Message}");
      }
    }
  }
}