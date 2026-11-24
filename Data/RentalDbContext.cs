using Microsoft.EntityFrameworkCore;


namespace TP1.Data;

public class RentalDbContext : DbContext
{
    public RentalDbContext(
        DbContextOptions<RentalDbContext> options)
        : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Rental> Rentals => Set<Rental>();
}