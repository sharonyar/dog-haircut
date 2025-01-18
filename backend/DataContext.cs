using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
}
