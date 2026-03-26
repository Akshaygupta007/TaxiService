using Microsoft.EntityFrameworkCore;
using TaxiService.Entities;

namespace TaxiService.DataDb
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<CabType> CabTypes { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<DriverVehicle> DriverVehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Driver>()
                .HasOne(d => d.Vehicle)
                .WithOne(v => v.Driver)
                .HasForeignKey<Driver>(d => d.VehicleId);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.CabType)
                .WithOne()
                .HasForeignKey<Vehicle>(v => v.CabTypeID); // Fixed: Specify the correct entity type for HasForeignKey

            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.VehicleNumber)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Driver>()
                .HasIndex(d => d.LicenseNumber)
                .IsUnique();

            modelBuilder.Entity<CabType>()
                .HasIndex(c => c.CabTypeName)
                .IsUnique();

            modelBuilder.Entity<Driver>()
                .HasIndex(d => d.PhoneNumber)
                .IsUnique();

            SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed CabTypes
            modelBuilder.Entity<CabType>().HasData(
                new CabType { CabTypeID = 1, CabTypeName = "Economy",  BaseFare = 30, FarePerKm = 10, CreatedAt = DateTime.UtcNow },
                new CabType { CabTypeID = 2, CabTypeName = "Premium", BaseFare = 50, FarePerKm = 15, CreatedAt = DateTime.UtcNow },
                new CabType { CabTypeID = 3, CabTypeName = "SUV", BaseFare = 80, FarePerKm = 20, CreatedAt = DateTime.UtcNow }
            );
        }

    }

    }
