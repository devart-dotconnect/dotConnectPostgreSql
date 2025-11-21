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
      // Set SSL options
      connection.SslOptions.CACert = @"\CACert.crt";
      connection.SslOptions.Cert = @"\Cert.crt";
      connection.SslOptions.Key = @"\Key.key";
      connection.SslOptions.SslMode = SslMode.Require;

      try
      {
        connection.Open();
        Console.WriteLine("SSL connection to PostgreSQL successful!");
      }
      catch (PgSqlException ex)
      {
        Console.WriteLine($"PostgreSQL SSL Error: {ex.Message}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"General Error: {ex.Message}");
      }
    }
  }
}