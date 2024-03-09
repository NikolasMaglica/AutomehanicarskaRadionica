﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        .HasColumnType("decimal(18,2)");  // Prilagodi preciznost i skaliranje prema potrebama

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

            modelBuilder.Entity<City>().HasData(new City { ID = 1, Name = "Zagreb" });
            modelBuilder.Entity<City>().HasData(new City { ID = 2, Name = "Velika Gorica" });
            modelBuilder.Entity<City>().HasData(new City { ID = 3, Name = "Vrbovsko" });
        }

    }
}