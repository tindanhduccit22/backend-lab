using System;
using System.Collections.Generic;
using final.Models;
using Microsoft.EntityFrameworkCore;

namespace final.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<PetCategory> PetCategories { get; set; }

    public virtual DbSet<PurchaseItem> PurchaseItems { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserRole> userRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=PetStoreManagementSystem;User Id=sa;Password=123456;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.PetId).HasName("PK__Pets__48E5386212229896");

            entity.Property(e => e.PetBreed).HasMaxLength(100);
            entity.Property(e => e.PetCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PetName).HasMaxLength(100);
            entity.Property(e => e.PetPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PetQuantity).HasDefaultValue(0);

            entity.HasOne(d => d.PetCategory).WithMany(p => p.Pets)
                .HasForeignKey(d => d.PetCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pets__PetCreated__4CA06362");
        });

        modelBuilder.Entity<PetCategory>(entity =>
        {
            entity.HasKey(e => e.PetCategoryId).HasName("PK__PetCateg__BDEB8BB44CCE5ECC");

            entity.HasIndex(e => e.PetCategoryName, "UQ__PetCateg__4C13080DB205514D").IsUnique();

            entity.Property(e => e.PetCategoryCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PetCategoryDescription).HasMaxLength(255);
            entity.Property(e => e.PetCategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<PurchaseItem>(entity =>
        {
            entity.HasKey(e => e.PurchaseItemId).HasName("PK__Purchase__B48BB68762F90E0A");

            entity.Property(e => e.PurchaseItemCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PurchaseItemUnitPirce).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Pet).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseI__PetId__5812160E");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseI__Purch__571DF1D5");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderField).HasName("PK__Purchase__2FB2D42B23BA0CE0");

            entity.Property(e => e.PurchaseOrdeStatus).HasMaxLength(20);
            entity.Property(e => e.PurchaseOrderCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PurchaseOrderOrderDate).HasColumnType("datetime");
            entity.Property(e => e.PurchaseOrderTotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Purch__534D60F1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C4A0D1C07");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053418446756").IsUnique();

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Fullname).HasMaxLength(200);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
