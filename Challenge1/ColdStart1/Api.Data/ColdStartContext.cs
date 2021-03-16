using Api.Data.Configurations;
using ColdStart1App.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Data
{
    public class ColdStartContext : DbContext
    {

        public ColdStartContext(DbContextOptions<ColdStartContext> options) : base(options)
        {

        }

        public DbSet<CatalogItem> Icecreams { get; set; }
        public DbSet<Preorder> Orders { get; set; }
        public DbSet<Driver> Drivers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Preorder>(new PreorderConfiguration());
            modelBuilder.Entity<Preorder>().ToTable("Orders");
            modelBuilder.ApplyConfiguration<CatalogItem>(new CatalogItemConfiguration());
            modelBuilder.Entity<CatalogItem>().ToTable("Icecreams");
            modelBuilder.ApplyConfiguration<Driver>(new DriverConfiguration());
            modelBuilder.Entity<Driver>().ToTable("Drivers");
        }
    }
}
