using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Data;

public partial class PBL3_Db_Context : DbContext
{
    public PBL3_Db_Context()
    {
    }

    public PBL3_Db_Context(DbContextOptions<PBL3_Db_Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<GoodsReceipt> GoodsReceipts { get; set; }

    public virtual DbSet<GoodsReceiptDetail> GoodsReceiptDetails { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD797AF0DDB4E");

            entity.ToTable("Cart");

            entity.HasIndex(e => e.UserId, "UQ__Cart__1788CCAD208F333C").IsUnique();

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Users");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B2A102B22CC");

            entity.HasIndex(e => new { e.CartId, e.ProductId }, "UQ_CartItems_Cart_Product").IsUnique();

            entity.Property(e => e.CartItemId).HasColumnName("CartItemID");
            entity.Property(e => e.AddedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Cart");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Products");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B14E590B9");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E047847E7F").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PK__Coupons__384AF1DA417EBE4B");

            entity.HasIndex(e => e.CouponCode, "UQ__Coupons__D3490800B67C3E2F").IsUnique();

            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.CouponCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DiscountType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DiscountValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Active");
        });

        modelBuilder.Entity<GoodsReceipt>(entity =>
        {
            entity.HasKey(e => e.ReceiptId).HasName("PK__GoodsRec__CC08C4001F2836C9");

            entity.Property(e => e.ReceiptId).HasColumnName("ReceiptID");
            entity.Property(e => e.ReceiptDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<GoodsReceiptDetail>(entity =>
        {
            entity.HasKey(e => e.ReceiptDetailId).HasName("PK__GoodsRec__82FADEDB7DC3101F");

            entity.Property(e => e.ReceiptDetailId).HasColumnName("ReceiptDetailID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReceiptId).HasColumnName("ReceiptID");

            entity.HasOne(d => d.Product).WithMany(p => p.GoodsReceiptDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GoodsReceiptDetails_Products");

            entity.HasOne(d => d.Receipt).WithMany(p => p.GoodsReceiptDetails)
                .HasForeignKey(d => d.ReceiptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GoodsReceiptDetails_Receipts");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF624D738A");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ShippingAddress).HasMaxLength(500);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Coupon).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CouponId)
                .HasConstraintName("FK_Orders_Coupons");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C1D1E9638");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PriceAtPurchase).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Products");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A58E82DFFCC");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TransactionID");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Orders");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED40077AEF");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.SellingPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Size)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC4D3C8916");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534124415B8").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Customer");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Active");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
