using Microsoft.EntityFrameworkCore;
using PolicyTracker.API.Models;

namespace PolicyTracker.API.Data;

public class PolicyTrackerDbContext : DbContext
{
    public PolicyTrackerDbContext(DbContextOptions<PolicyTrackerDbContext> options)
        : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<Claim> Claims => Set<Claim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(e =>
        {
            e.ToTable("Customers");
            e.Property(c => c.SSN).HasMaxLength(11);
            e.Property(c => c.State).HasMaxLength(2);
        });

        modelBuilder.Entity<Policy>(e =>
        {
            e.ToTable("Policies");
            e.HasOne(p => p.Customer)
             .WithMany(c => c.Policies)
             .HasForeignKey(p => p.CustomerId);
            e.Property(p => p.PremiumAmount).HasColumnType("decimal(10,2)");
        });

        modelBuilder.Entity<Claim>(e =>
        {
            e.ToTable("Claims");
            e.HasOne(cl => cl.Policy)
             .WithMany(p => p.Claims)
             .HasForeignKey(cl => cl.PolicyId);
            e.Property(cl => cl.Amount).HasColumnType("decimal(10,2)");
        });
    }
}