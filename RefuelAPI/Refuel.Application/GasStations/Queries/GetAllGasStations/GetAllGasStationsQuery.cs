using Refuel.Application.GasStations.Dtos;
using Mediator;

namespace Refuel.Application.GasStations.Queries.GetAllGasStations;

public record GetAllGasStationsQuery : IRequest<IEnumerable<GasStationDto>>;