using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Good> Goods { get; set; }

    public virtual DbSet<GoodMutation> GoodMutations { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=inventory;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__category__D54EE9B440C98654");

            entity.ToTable("category");

            entity.HasIndex(e => e.CategoryName, "UQ__category__5189E2551EF99E22").IsUnique();

            entity.HasIndex(e => e.CategoryName, "idx_category_name");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("category_name");
            entity.Property(e => e.IsDelete)
                .HasDefaultValue(false)
                .HasColumnName("is_delete");
        });

        modelBuilder.Entity<Good>(entity =>
        {
            entity.HasKey(e => e.GoodId).HasName("PK__good__1405AEAA4CB61155");

            entity.ToTable("good");

            entity.HasIndex(e => e.GoodCode, "UQ__good__547B6BB04EB4D7FC").IsUnique();

            entity.HasIndex(e => e.GoodCode, "idx_good_code");

            entity.HasIndex(e => e.GoodName, "idx_good_name");

            entity.Property(e => e.GoodId).HasColumnName("good_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.GoodCode)
                .HasMaxLength(10)
                .HasColumnName("good_code");
            entity.Property(e => e.GoodName)
                .HasMaxLength(255)
                .HasColumnName("good_name");
            entity.Property(e => e.GoodStock).HasColumnName("good_stock");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Goods)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_category");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Goods)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_supplier");
        });

        modelBuilder.Entity<GoodMutation>(entity =>
        {
            entity.HasKey(e => e.MutationId).HasName("PK__good_mut__8CB62A6F2CDC21ED");

            entity.ToTable("good_mutation");

            entity.HasIndex(e => e.MutationDate, "idx_good_mutation_date");

            entity.Property(e => e.MutationId).HasColumnName("mutation_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.GoodId).HasColumnName("good_id");
            entity.Property(e => e.MutationDate)
                .HasColumnType("datetime")
                .HasColumnName("mutation_date");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasColumnName("status");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

            entity.HasOne(d => d.Category).WithMany(p => p.GoodMutations)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_category_good_mutation");

            entity.HasOne(d => d.Good).WithMany(p => p.GoodMutations)
                .HasForeignKey(d => d.GoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_good");

            entity.HasOne(d => d.Supplier).WithMany(p => p.GoodMutations)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_supplier_good_mutation");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__supplier__6EE594E84AEE0E4B");

            entity.ToTable("supplier");

            entity.HasIndex(e => e.SupplierName, "UQ__supplier__821868347F1B0E13").IsUnique();

            entity.HasIndex(e => e.SupplierName, "idx_supplier_name");

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.IsDelete)
                .HasDefaultValue(false)
                .HasColumnName("is_delete");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(255)
                .HasColumnName("supplier_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
