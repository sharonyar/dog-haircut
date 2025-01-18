using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;


public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    // ✅ Ensure both Users and Customers DbSet exist
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ✅ Seed a default user
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "testuser",
                PasswordHash = HashPassword("password123") // Hashed password123
            }
        );

    }

    // ✅ Add Hashing Method
    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}

