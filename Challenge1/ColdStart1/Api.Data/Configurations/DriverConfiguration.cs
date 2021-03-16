using ColdStart1App.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(x => x.ImageUri)
                .HasMaxLength(2000)
                .HasDefaultValue("https://coldstartsa.blob.core.windows.net/web/assets/Driver1.png");
        }
    }
}
