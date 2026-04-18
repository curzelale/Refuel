using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Refuel.Domain.Entities;

namespace Refuel.Persistence.Configurations;

public class FuelConfiguration: IEntityTypeConfiguration<Fuel>
{
    public void Configure(EntityTypeBuilder<Fuel> builder)
    {
        builder.ToTable("fuels");
        builder.Property(prop => prop.Id).IsRequired().HasColumnName("id");
        builder.Property(prop => prop.Name).IsRequired().HasColumnName("name");
    }
}