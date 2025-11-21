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