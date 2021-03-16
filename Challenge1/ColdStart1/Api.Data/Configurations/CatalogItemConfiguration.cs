using ColdStart1App.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Data.Configurations
{
    public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(2000);
            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(2000);
        }
    }
}
