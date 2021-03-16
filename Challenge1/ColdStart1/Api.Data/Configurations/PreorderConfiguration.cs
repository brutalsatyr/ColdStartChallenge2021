using ColdStart1App.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Data.Configurations
{
    public class PreorderConfiguration : IEntityTypeConfiguration<Preorder>
    {
        public void Configure(EntityTypeBuilder<Preorder> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.User)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("new");
            builder.Property(x => x.FullAddress)
                .HasMaxLength(2000);
            builder.Property(x => x.LastPosition)
                .HasMaxLength(500);
            builder.Property(x => x.Date)
                .IsRequired()
                .ValueGeneratedOnAdd();
            builder.HasOne(x => x.Driver)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.DriverId);
            builder.HasOne(x => x.Icecream)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.IcecreamId).IsRequired();

        }
    }
}
