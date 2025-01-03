using Microsoft.EntityFrameworkCore;
using Models;
using Models.Sub;

namespace DAL
{
    public class Context : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<RoutePoint> RoutePoints { get; set; }

        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasIndex(u => u.Name).IsUnique(true);
            modelBuilder.Entity<Truck>().HasIndex(u => new { u.Model, u.Number }).IsUnique(true);
            modelBuilder.Entity<Trailer>().HasIndex(u => new { u.Model, u.Number }).IsUnique(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
