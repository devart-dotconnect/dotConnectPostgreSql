# How to connect to PostgreSQL in .NET with C#

Based on [https://www.devart.com/dotconnect/postgresql/connect-postgresql-in-net.html](https://www.devart.com/dotconnect/postgresql/connect-postgresql-in-net.html)

This tutorial covers multiple ways to connect to PostgreSQL databases in .NET using C#. Whether you're implementing standard ADO.NET connections or working with EF Core, this guide will walk you through secure and flexible options using [dotConnect for PostgreSQL](https://www.devart.com/dotconnect/postgresql/)—a powerful data provider with built-in support for SSL, SSH, and proxy configurations.

## Connect to PostgreSQL using C#

Get started by establishing a basic connection to your PostgreSQL database using dotConnect for PostgreSQL and ADO.NET. This section shows how to write a connection string, open the connection, and execute simple SQL commands in your .NET app.

```cs
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
```

## Connect to PostgreSQL using the SSL/TLS connection

Encrypt your data in transit by enabling SSL/TLS in your PostgreSQL connection. This section demonstrates how to configure your connection string to use SSL and validate certificates for secure communication between your application and the database server.

```cs
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
```

## Connect to PostgreSQL using the SSH connection

If your PostgreSQL server is only accessible via an internal network, SSH tunneling offers a secure path. Learn how to configure dotConnect for PostgreSQL to tunnel your database connection through an SSH server, improving security and bypassing network restrictions.

```cs
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
```

## Connect to PostgreSQL using the Proxy connection

This section guides you through setting up a connection to PostgreSQL using a proxy server. dotConnect supports HTTP, SOCKS4, and SOCKS5 proxies, making it easy to connect in restricted or monitored environments.

```cs
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
```

## Connect to PostgreSQL with EF Core

Use EF Core to streamline data access in your .NET applications. This section explains how to configure dotConnect for PostgreSQL with EF Core and build a data model, enabling clean, maintainable code through object-relational mapping.

```cs
using System;
using Microsoft.EntityFrameworkCore;
using Devart.Data.PostgreSql.EFCore;

public class Actor
{
  public int ActorId { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
}

public class SakilaContext : DbContext
{
  public DbSet<Actor> Actors { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UsePostgreSql(
      "Host=127.0.0.1;Port=5432;UserId=TestUser;Password=TestPassword;Database=sakila;Schema=public;LicenseKey=**********",
      PostgreSqlServerVersion.LatestSupportedServerVersion
    );
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Actor>().ToTable("actor"); // ensure correct table mapping
    modelBuilder.Entity<Actor>().Property(a => a.ActorId).HasColumnName("actor_id");
    modelBuilder.Entity<Actor>().Property(a => a.FirstName).HasColumnName("first_name");
    modelBuilder.Entity<Actor>().Property(a => a.LastName).HasColumnName("last_name");
  }
}

class Program
{
  static void Main()
  {
    using (var context = new SakilaContext())
    {
      try
      {
        // Ensure the database is created (optional for read-only usage)
        context.Database.EnsureCreated();
        Console.WriteLine("Connected to PostgreSQL database successfully!");

        // Query data from the Actors table
        var actors = context.Actors;
        foreach (var actor in actors)
        {
          Console.WriteLine($"Actor ID: {actor.ActorId}, Name: {actor.FirstName} {actor.LastName}");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error: {ex.Message}");
      }
    }
  }
}
```