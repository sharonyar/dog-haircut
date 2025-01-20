using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    // ✅ Define both Users and Schedules tables
    public DbSet<User> Users { get; set; }
    public DbSet<Schedule> Schedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ✅ Setup foreign key relationship
        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade); // ✅ Delete appointments if user is removed

        // ✅ Seed a default user for testing
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "testuser",
                PasswordHash = HashPassword("password123"), // ✅ Store hashed password
                Name = "John Doe"
            }
        );
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();

        // ✅ Generate a random salt
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // ✅ Combine password and salt before hashing
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] combinedBytes = new byte[salt.Length + passwordBytes.Length];
        Buffer.BlockCopy(salt, 0, combinedBytes, 0, salt.Length);
        Buffer.BlockCopy(passwordBytes, 0, combinedBytes, salt.Length, passwordBytes.Length);

        // ✅ Compute Hash
        byte[] hashBytes = sha256.ComputeHash(combinedBytes);

        // ✅ Store salt + hash together in Base64 format
        byte[] hashWithSalt = new byte[salt.Length + hashBytes.Length];
        Buffer.BlockCopy(salt, 0, hashWithSalt, 0, salt.Length);
        Buffer.BlockCopy(hashBytes, 0, hashWithSalt, salt.Length, hashBytes.Length);

        return Convert.ToBase64String(hashWithSalt);
    }





}
