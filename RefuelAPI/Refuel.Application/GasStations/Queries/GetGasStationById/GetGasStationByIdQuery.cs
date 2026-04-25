using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;

namespace Refuel.Application.GasStations.Queries.GetGasStationById;

public record GetGasStationByIdQuery(Guid Id) : IRequest<GasStationDto?>;