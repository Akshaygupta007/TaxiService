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

            modelBuilder.Entity<DriverVehicle>()
                .HasKey(dv => new { dv.DriverID, dv.VehicleID });  // Composite PK

            modelBuilder.Entity<DriverVehicle>()
                .HasOne(dv => dv.Driver)
                .WithMany(d => d.Vehicles)
                .HasForeignKey(dv => dv.DriverID);

            modelBuilder.Entity<DriverVehicle>()
                .HasOne(dv => dv.Vehicle)
                .WithMany(v => v.Drivers)
                .HasForeignKey(dv => dv.VehicleID);

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

            modelBuilder.Entity<Driver>()
                .HasIndex(d => d.PhoneNumber)
                .IsUnique();
            
            modelBuilder.Entity<CabType>()
                .HasMany(e => e.Vehicles)
                .WithOne(v => v.CabType)
                .HasForeignKey(v => v.CabTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.Drivers)
                .WithOne(dv => dv.Vehicle)
                .HasForeignKey(dv => dv.VehicleID)
                .OnDelete(DeleteBehavior.Cascade);
            
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
