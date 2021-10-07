using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WTAssignment.Models;
#nullable disable

namespace WTAssignment.Data
{
    public partial class AmazonOrdersContext : DbContext
    {
        public AmazonOrdersContext()
        {
        }

        public AmazonOrdersContext(DbContextOptions<AmazonOrdersContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemCategory> ItemCategories { get; set; }
        public virtual DbSet<ItemMarkupHistory> ItemMarkupHistories { get; set; }
        public virtual DbSet<ItemsInOrder> ItemsInOrders { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-CDBQUR0\\SQLEXPRESS;Initial Catalog=AmazonOrders;Persist Security Info=False;User ID=sa;Password=123456;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.AddressId).HasColumnName("addressID");

                entity.Property(e => e.AddressLine)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("addressLine");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("country");

                entity.Property(e => e.Postcode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("postcode");

                entity.Property(e => e.Region)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("region");

                entity.Property(e => e.Suburb)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("suburb");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.AddressId).HasColumnName("addressID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.MainPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("mainPhoneNumber");

                entity.Property(e => e.SecondaryPhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("secondaryPhoneNumber");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("fk_address");
            });

            modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.HasKey(e => e.OrderNumber)
                    .HasName("pk_orderNumber");

                entity.Property(e => e.OrderNumber).HasColumnName("orderNumber");

                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.DatePaid)
                    .HasColumnType("date")
                    .HasColumnName("datePaid");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("orderDate");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("fk_customerID");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ItemId).HasColumnName("itemID");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.ItemCost)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("itemCost");

                entity.Property(e => e.ItemDescription)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("itemDescription");

                entity.Property(e => e.ItemImage)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("itemImage");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("itemName");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_itemCategory");
            });

            modelBuilder.Entity<ItemCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("pk_itemCategories");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("categoryName");

                entity.Property(e => e.ParentCategoryId).HasColumnName("parentCategoryID");

                entity.HasOne(d => d.ParentCategory)
                    .WithMany(p => p.InverseParentCategory)
                    .HasForeignKey(d => d.ParentCategoryId)
                    .HasConstraintName("fk_parentCategory");
            });

            modelBuilder.Entity<ItemMarkupHistory>(entity =>
            {
                entity.HasKey(e => new { e.ItemId, e.StartDate });

                entity.ToTable("ItemMarkupHistory");

                entity.Property(e => e.ItemId).HasColumnName("itemID");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("endDate");

                entity.Property(e => e.Markup)
                    .HasColumnType("decimal(4, 1)")
                    .HasColumnName("markup")
                    .HasDefaultValueSql("((1.3))");

                entity.Property(e => e.Sale)
                    .IsRequired()
                    .HasColumnName("sale")
                    .HasDefaultValueSql("('False')");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemMarkupHistories)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemMarkupHistory_Items");
            });

            modelBuilder.Entity<ItemsInOrder>(entity =>
            {
                entity.HasKey(e => new { e.OrderNumber, e.ItemId })
                    .HasName("pk_itemsInOrder");

                entity.ToTable("ItemsInOrder");

                entity.Property(e => e.OrderNumber).HasColumnName("orderNumber");

                entity.Property(e => e.ItemId).HasColumnName("itemID");

                entity.Property(e => e.NumberOf).HasColumnName("numberOf");

                entity.Property(e => e.TotalItemCost)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("totalItemCost");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemsInOrders)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_items");

                entity.HasOne(d => d.OrderNumberNavigation)
                    .WithMany(p => p.ItemsInOrders)
                    .HasForeignKey(d => d.OrderNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_orderNumber");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.ReviewId).HasColumnName("reviewID");

                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.ItemId).HasColumnName("itemID");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.ReviewDate)
                    .HasColumnType("date")
                    .HasColumnName("reviewDate");

                entity.Property(e => e.ReviewDescription)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("reviewDescription");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_customer_review");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("fk_item_review");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
