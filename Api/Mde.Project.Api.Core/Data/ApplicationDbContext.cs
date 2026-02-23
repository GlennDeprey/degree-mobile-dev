using Mde.Project.Api.Core.Entities.Products;
using Mde.Project.Api.Core.Entities.Reports;
using Mde.Project.Api.Core.Entities.Statistics;
using Mde.Project.Api.Core.Entities.Users;
using Mde.Project.Api.Core.Entities.Warehouses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mde.Project.Api.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        // Products
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductTax> ProductTaxes { get; set; }
        public DbSet<Brand> Brands { get; set; }

        // Warehouses
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public DbSet<WarehouseLocationInfo> WarehouseLocationInfos { get; set; }
        public DbSet<WarehouseGoogleInfo> WarehouseGoogleInfos { get; set; }
        public DbSet<WarehousePhoto> WarehousePhotos { get; set; }

        // Reports
        public DbSet<Report> Reports { get; set; }

        // Statistics
        public DbSet<WarehouseStats> WarehouseStats { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        override protected async void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Products
            modelBuilder.Entity<Brand>()
                .Property(b => b.Name)
                .IsRequired();

            modelBuilder.Entity<ProductCategory>()
                .Property(pt => pt.Name)
                .IsRequired();

            modelBuilder.Entity<ProductCategory>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Image)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.SalesPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Barcode)
                .IsRequired()
                .HasMaxLength(13);

            modelBuilder.Entity<ProductTax>()
                .Property(pt => pt.Name)
                .IsRequired();

            modelBuilder.Entity<ProductTax>()
                .Property(pt => pt.TaxRate)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasOne(w => w.SalesTax)
                .WithMany()
                .HasForeignKey(w => w.SalesTaxId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Product>()
                .HasOne(w => w.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(w => w.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Product>()
                .HasOne(w => w.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(w => w.BrandId)
                .OnDelete(DeleteBehavior.SetNull);

            // Warehouses
            modelBuilder.Entity<Warehouse>()
                .Property(p => p.LocationInfoId)
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .Property(p => p.ShortName)
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .Property(p => p.Phone)
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .Property(p => p.Earnings)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<WarehouseLocationInfo>()
                .Property(p => p.Address)
                .IsRequired();

            modelBuilder.Entity<WarehouseLocationInfo>()
                .Property(p => p.City)
                .IsRequired();

            modelBuilder.Entity<WarehouseLocationInfo>()
                .Property(p => p.Country)
                .IsRequired();

            modelBuilder.Entity<WarehouseLocationInfo>()
                .Property(p => p.PostalCode)
                .IsRequired();

            modelBuilder.Entity<WarehouseLocationInfo>()
                .Property(p => p.State)
                .IsRequired();

            modelBuilder.Entity<WarehouseGoogleInfo>()
                .Property(p => p.GoogleAddress)
                .IsRequired();

            modelBuilder.Entity<WarehouseGoogleInfo>()
                .Property(p => p.GoogleAddressId)
                .IsRequired();

            modelBuilder.Entity<WarehouseItem>()
                .Property(p => p.ProductId)
                .IsRequired();

            modelBuilder.Entity<WarehouseItem>()
                .Property(p => p.WarehouseId)
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .HasMany(w => w.WarehouseItems)
                .WithOne(wi => wi.Warehouse)
                .HasForeignKey(wi => wi.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.LocationInfo)
                .WithOne()
                .HasForeignKey<Warehouse>(w => w.LocationInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.GoogleInfo)
                .WithOne()
                .HasForeignKey<Warehouse>(wi => wi.GoogleInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WarehouseGoogleInfo>()
                .HasMany(w => w.GooglePhotoUris)
                .WithOne()
                .HasForeignKey(wi => wi.GoogleInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WarehouseItem>()
                .Property(p => p.Quantity)
                .IsRequired();

            modelBuilder.Entity<WarehouseItem>()
                .Property(p => p.WarehouseId)
                .IsRequired();

            modelBuilder.Entity<WarehouseItem>()
                .Property(p => p.ProductId)
                .IsRequired();

            modelBuilder.Entity<Report>()
                .Property(p => p.WarehouseId)
                .IsRequired();

            modelBuilder.Entity<Report>()
                .Property(p => p.ProductId)
                .IsRequired();

            modelBuilder.Entity<Report>()
                .Property(p => p.Description)
                .IsRequired();

            modelBuilder.Entity<WarehouseStats>()
                .Property(p => p.WarehouseId)
                .IsRequired();

            var seeder = new ApplicationSeeder(modelBuilder);
            seeder.Seed();
        }
    }
}
