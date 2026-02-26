using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Shop.Entities;

public partial class ShopDBContext : DbContext
{
    public ShopDBContext()
    {
    }

    public ShopDBContext(DbContextOptions<ShopDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderAudit> OrderAudits { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=ShopDBTest;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B4584D357");

            entity.ToTable("Categories", "catalog");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E060CD67C3").IsUnique();

            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D89916EC2F");

            entity.ToTable("Customers", "sales");

            entity.HasIndex(e => e.RegistrationDate, "NCI_Customers_RegistrationDate");

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D10534828ED2E8").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Customers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Customers__UserI__55009F39");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF4A07C9DF");

            entity.ToTable("Orders", "sales");

            entity.HasIndex(e => e.CustomerId, "NCI_Orders_CustomerId");

            entity.HasIndex(e => e.Status, "NCI_Orders_Status");

            entity.Property(e => e.OrderNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComputedColumnSql("('ORD-'+right('000000'+CONVERT([varchar](6),[OrderId]),(6)))", true)
                .HasColumnName("OrderNO");
            entity.Property(e => e.PaidAt).HasColumnType("datetime");
            entity.Property(e => e.PlacedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.ToTalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Customer__57DD0BE4");
        });

        modelBuilder.Entity<OrderAudit>(entity =>
        {
            entity.HasKey(e => e.OrderAuditId).HasName("PK__OrderAud__DFA84451F0CE3AB1");

            entity.ToTable("OrderAudit", "OPS");

            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NewStatis).HasMaxLength(20);
            entity.Property(e => e.OldStatus).HasMaxLength(20);
            entity.Property(e => e.Operations).HasMaxLength(20);

            entity.HasOne(d => d.ChangedByUser).WithMany(p => p.OrderAudits)
                .HasForeignKey(d => d.ChangedByUserId)
                .HasConstraintName("FK__OrderAudi__Chang__65370702");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderAudits)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderAudi__Order__634EBE90");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED0681E1ED6438");

            entity.ToTable("OrderItems", "sales");

            entity.HasIndex(e => e.OrderId, "NCI_OrderItems_OrderId");

            entity.HasIndex(e => e.ProductId, "NCI_OrderItems_ProductId");

            entity.Property(e => e.TotalPrice)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", true)
                .HasColumnType("decimal(21, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderItem__Order__5D95E53A");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Produ__5E8A0973");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD961E1C9E");

            entity.ToTable("Products", "catalog");

            entity.HasIndex(e => e.IsActive, "NCI_Product_IsActive");

            entity.HasIndex(e => e.UnitPrice, "NCI_Product_UnitPrice");

            entity.HasIndex(e => e.ProductName, "UQ__Products__DD5A978A2F73D8F0").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasMany(d => d.Categories).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK__ProductCa__Categ__503BEA1C"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK__ProductCa__Produ__4F47C5E3"),
                    j =>
                    {
                        j.HasKey("ProductId", "CategoryId").HasName("PK__ProductC__159C556D3ECE2068");
                        j.ToTable("ProductCategories", "catalog");
                        j.HasIndex(new[] { "CategoryId" }, "NCI_ProductCategories_CategoryId");
                    });
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Students", "academy");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C9D19B223");

            entity.ToTable("Users", "auth");

            entity.HasIndex(e => e.IsActive, "NCI_User_IsActive");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A429FB3B").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(64);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserProf__1788CC4CD2E1E971");

            entity.ToTable("UserProfiles", "auth");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PersonalN).HasMaxLength(11);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(30)
                .HasColumnName("PhoneNO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
