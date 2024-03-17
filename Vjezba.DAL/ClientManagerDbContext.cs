using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Vjezba.Model;

namespace Vjezba.DAL
{
    public class ClientManagerDbContext : IdentityDbContext<AppUser>
    {
        public ClientManagerDbContext(DbContextOptions<ClientManagerDbContext> options)
            : base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Vehicle>   Vehicles { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<UserVehicle> UserVehicles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderMaterial> OrderMaterials { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<MaterialOffer> MaterialOffers { get; set; }
        public DbSet<ServiceOffer> ServiceOffers { get; set; }
        public DbSet<OfferStatus> OfferStatuses { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>()
        .Property(m => m.MaterialPrice)
        .HasColumnType("decimal(18,2)"); 

            modelBuilder.Entity<Offer>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Service>()
                .Property(s => s.ServicePrice)
                .HasColumnType("decimal(18,2)");

            // Ostatak OnModelCreating metode...

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OfferStatus>().HasData(new OfferStatus { ID = 1, OfferStatusName = "Prihvaćena" });
            modelBuilder.Entity<OfferStatus>().HasData(new OfferStatus { ID = 2, OfferStatusName = "Odbijena" });
            modelBuilder.Entity<OfferStatus>().HasData(new OfferStatus { ID = 3, OfferStatusName = "Na čekanju" });
            modelBuilder.Entity<OrderStatus>().HasData(new OrderStatus { ID = 1, Name = "Pristigla" });
            modelBuilder.Entity<OrderStatus>().HasData(new OrderStatus { ID = 2, Name = "Na čekanju" });
        }

    }
}
