using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Refuel.Domain.Entities;

namespace Refuel.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");
        builder.Property(prop => prop.Id).IsRequired().HasColumnName("id");
        builder.Property(prop => prop.Brand).IsRequired().HasColumnName("brand");
        builder.Property(prop => prop.Model).IsRequired().HasColumnName("model");
        builder.Property(prop => prop.Owner).IsRequired().HasColumnName("owner");
        builder.Property(prop => prop.LicencesPlate).HasColumnName("licences_plate");
        builder.Property(prop => prop.Nickname).HasColumnName("nickname");

        builder.HasMany(v => v.Fuels)
            .WithMany()
            .UsingEntity(
                "vehicle_fuels",
                l => l.HasOne(typeof(Fuel)).WithMany().HasForeignKey("fuel_id"),
                r => r.HasOne(typeof(Vehicle)).WithMany().HasForeignKey("vehicle_id"),
                j =>
                {
                    j.HasKey("fuel_id", "vehicle_id");
                });
    }
}