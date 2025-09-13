namespace CreditApproval.Data.Data;

public class CreditApprovalDbContext(DbContextOptions<CreditApprovalDbContext> options) : DbContext(options)
{
    public DbSet<CreditModel> Credits { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Store enums as string for better readability
        modelBuilder.Entity<CreditModel>().Property(prop => prop.CreditType).HasConversion<string>();
        modelBuilder.Entity<CreditModel>().Property(prop => prop.Status).HasConversion<string>();

        modelBuilder.Entity<User>().Property(prop => prop.Role).HasConversion<string>();

        base.OnModelCreating(modelBuilder);
    }
}
