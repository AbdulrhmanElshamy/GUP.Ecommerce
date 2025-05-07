using GUP.Ecommerce.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace GUP.Ecommerce.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) :
    IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


     public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var cascadeFKs = builder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;


        builder.Entity<ApplicationUser>()
        .HasQueryFilter(c => !c.IsDisabled);

        builder.Entity<Product>()
          .HasOne(p => p.Category)
          .WithMany(c => c.Products)
          .HasForeignKey(p => p.CategoryId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProductStock>()
            .HasOne(ps => ps.Product)
            .WithMany(p => p.ProductStocks)
            .HasForeignKey(ps => ps.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Payment>()
            .HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);


        base.OnModelCreating(builder);

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();

        foreach (var entityEntry in entries)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId()!;

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}