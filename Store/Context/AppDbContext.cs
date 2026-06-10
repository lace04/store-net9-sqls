using Microsoft.EntityFrameworkCore;
using Store.Entitites;

namespace Store.Context
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      // Configure relationships and constraints if needed

      // Configure Category entity and seed initial data
      modelBuilder.Entity<Category>(e =>
      {
        e.HasKey("CategoryId");
        e.Property("CategoryId").ValueGeneratedOnAdd();
        e.HasData(
          new Category { CategoryId = 1, Name = "Electronics", Description = "Electronic devices and accessories" },
          new Category { CategoryId = 2, Name = "Books", Description = "Books and publications" },
          new Category { CategoryId = 3, Name = "Clothing", Description = "Apparel and fashion items" }
        );
      });


      // Configure Product entity and its relationship with Category, delete is restricted to prevent cascading deletes
      modelBuilder.Entity<Product>(e =>
      {
        e.HasKey("ProductId");
        e.Property("ProductId").ValueGeneratedOnAdd();
        e.Property("Price").HasColumnType("decimal(10,2)");
        e.HasOne(e => e.Category)
          .WithMany(p => p.Products)
          .HasForeignKey(p => p.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);
      });

      // Configure User entity and ensure email is unique
      modelBuilder.Entity<User>(e =>
      {
        e.HasKey("UserId");
        e.Property("UserId").ValueGeneratedOnAdd();
        e.HasIndex(u => u.Email).IsUnique();
      });

      // Configure Order entity, its relationship with User, and restrict cascade deletes to prevent accidental data loss
      modelBuilder.Entity<Order>(e =>
      {
        e.HasKey("OrderId");
        e.Property("OrderId").ValueGeneratedOnAdd();
        e.Property("TotalAmount").HasColumnType("decimal(10,2)");
        e.HasOne(o => o.User)
          .WithMany(u => u.Orders)
          .HasForeignKey(o => o.UserId)
        .OnDelete(DeleteBehavior.Restrict);
      });

      // Configure OrderItem entity, its relationships with Order and Product, and ensure that deleting an order or product does not cascade delete the order items to maintain data integrity
      modelBuilder.Entity<OrderItem>(e =>
        {
          e.HasKey("OrderItemId");
          e.Property("OrderItemId").ValueGeneratedOnAdd();
          e.Property(oi => oi.Price).HasColumnType("decimal(10,2)");
          e.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
          .OnDelete(DeleteBehavior.Restrict);
          e.HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
          .OnDelete(DeleteBehavior.Restrict);
        });
    }
  }
}
