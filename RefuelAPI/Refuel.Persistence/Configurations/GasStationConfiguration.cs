using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Refuel.Domain.Entities;

namespace Refuel.Persistence.Configurations;

public class GasStationConfiguration : IEntityTypeConfiguration<GasStation>
{
    public void Configure(EntityTypeBuilder<GasStation> builder)
    {
        builder.ToTable("gas_stations");
        builder.Property(prop => prop.Id).IsRequired().HasColumnName("id");
        builder.Property(prop => prop.Name).IsRequired().HasColumnName("name");
        builder.Property(prop => prop.Address).IsRequired().HasColumnName("address");
        builder.Property(prop => prop.Latitude).IsRequired().HasColumnName("latitude");
        builder.Property(prop => prop.Longitude).IsRequired().HasColumnName("longitude");

        builder.HasMany(gs => gs.Fuels)
            .WithMany()
            .UsingEntity(
                "gas_station_fuels",
                l => l.HasOne(typeof(Fuel)).WithMany().HasForeignKey("fuel_id"),
                r => r.HasOne(typeof(GasStation)).WithMany().HasForeignKey("gas_station_id"),
                j =>
                {
                    j.HasKey("fuel_id", "gas_station_id");
                });
    }
}