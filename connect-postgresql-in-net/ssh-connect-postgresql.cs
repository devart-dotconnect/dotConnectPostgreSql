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
      // Configure SSH tunnel
      connection.SshOptions.Host = "sshServer";
      connection.SshOptions.User = "sshUser";
      connection.SshOptions.Password = "sshPassword";
      connection.SshOptions.AuthenticationType = SshAuthenticationType.Password;

      try
      {
        connection.Open();
        Console.WriteLine("SSH connection to PostgreSQL successful!");
      }
      catch (PgSqlException ex)
      {
        Console.WriteLine($"PostgreSQL SSH Error: {ex.Message}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"General Error: {ex.Message}");
      }
    }
  }
}