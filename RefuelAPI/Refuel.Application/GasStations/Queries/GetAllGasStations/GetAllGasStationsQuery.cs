using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;

namespace Refuel.Application.GasStations.Queries.GetAllGasStations;

public record GetAllGasStationsQuery : IRequest<IEnumerable<GasStationDto>>;