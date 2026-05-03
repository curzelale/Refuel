using Refuel.Application.Fuels.Dtos;
using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Vehicles.Dtos;

namespace Refuel.Application.Refuels.Dtos;

internal static class RefuelMapper
{
    internal static RefuelDto ToDto(Domain.Entities.Refuel refuel)
    {
        var vehicleDto = refuel.Vehicle is null
            ? null
            : new VehicleDto(
                refuel.Vehicle.Id, refuel.Vehicle.Brand, refuel.Vehicle.Model, refuel.Vehicle.Owner,
                refuel.Vehicle.Fuels.Select(f => new FuelDto(f.Id, f.Name)),
                refuel.Vehicle.Nickname, refuel.Vehicle.LicencesPlate);

        var gasStationDto = refuel.GasStation is null
            ? null
            : new GasStationDto(
                refuel.GasStation.Id, refuel.GasStation.Name, refuel.GasStation.Address,
                refuel.GasStation.Latitude, refuel.GasStation.Longitude,
                Enumerable.Empty<FuelDto>());

        var fuelDto = refuel.Fuel is null
            ? null
            : new FuelDto(refuel.Fuel.Id, refuel.Fuel.Name);

        return new RefuelDto(
            refuel.Id,
            refuel.VehicleId,
            refuel.GasStationId,
            refuel.FuelId,
            refuel.Quantity,
            refuel.TotalPrice,
            refuel.Date,
            refuel.OdometerKm,
            refuel.Note,
            vehicleDto,
            gasStationDto,
            fuelDto);
    }
}
