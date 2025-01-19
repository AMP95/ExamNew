using Microsoft.EntityFrameworkCore;
using Models;
using Models.Main;

namespace DAL
{
    public class Context : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractTemplate> Templates { get; set; }

        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasIndex(u => new { u.TruckModel, u.TruckNumber, u.TrailerModel, u.TrailerNumber }).IsUnique(true);
            modelBuilder.Entity<Contract>().HasIndex(u => new { u.Number, u.CreationDate }).IsUnique(true);
            modelBuilder.Entity<ContractTemplate>().HasIndex(u => u.Name).IsUnique(true);
            modelBuilder.Entity<Models.Sub.File>().HasIndex(u => u.Path).IsUnique(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
