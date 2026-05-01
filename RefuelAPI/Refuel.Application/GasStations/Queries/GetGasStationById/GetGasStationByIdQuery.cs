using Refuel.Application.GasStations.Dtos;
using Mediator;

namespace Refuel.Application.GasStations.Queries.GetGasStationById;

public record GetGasStationByIdQuery(Guid Id) : IRequest<GasStationDto?>;