using Devart.Data.PostgreSql;

class Program
{
  static void Main()
  {
    var builder = new PgSqlConnectionStringBuilder
    {
      Host = "127.0.0.1",
      Port = 5432,
      UserId = "TestUser",
      Password = "TestPassword",
      Database = "sakila",
      Schema = "public",
      LicenseKey = "**********"
    };

    using (PgSqlConnection connection = new PgSqlConnection(builder.ConnectionString))
    {
      // Configure proxy settings
      connection.ProxyOptions.Host = "10.0.0.1";
      connection.ProxyOptions.Port = 808;
      connection.ProxyOptions.User = "ProxyUser";
      connection.ProxyOptions.Password = "ProxyPassword";

      try
      {
        connection.Open();
        Console.WriteLine("Proxy connection to PostgreSQL successful!");
      }
      catch (PgSqlException ex)
      {
        Console.WriteLine($"PostgreSQL Proxy Error: {ex.Message}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"General Error: {ex.Message}");
      }
    }
  }
}