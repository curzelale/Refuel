using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Refuel.Domain.Entities;

namespace Refuel.Persistence.Configurations;

public class RefuelConfiguration : IEntityTypeConfiguration<Domain.Entities.Refuel>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Refuel> builder)
    {
        builder.ToTable("refuels");
        builder.Property(prop => prop.Id).IsRequired().HasColumnName("id");
        builder.Property(prop => prop.VehicleId).IsRequired().HasColumnName("vehicle_id");
        builder.Property(prop => prop.GasStationId).IsRequired().HasColumnName("gas_station_id");
        builder.Property(prop => prop.FuelId).IsRequired().HasColumnName("fuel_id");
        builder.Property(prop => prop.Quantity).IsRequired().HasColumnName("quantity");
        builder.Property(prop => prop.TotalPrice).IsRequired().HasColumnName("total_price");
        builder.Property(prop => prop.Date).IsRequired().HasColumnName("date");
        builder.Property(prop => prop.OdometerKm).IsRequired().HasColumnName("odometer_km");
        builder.Property(prop => prop.Note).HasColumnName("note");
    }
}