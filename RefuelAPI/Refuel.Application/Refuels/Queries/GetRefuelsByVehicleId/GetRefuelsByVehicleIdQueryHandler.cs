using Mediator;
using Refuel.Application.Refuels.Dtos;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Refuels.Queries.GetRefuelsByVehicleId;

public class GetRefuelsByVehicleIdQueryHandler : IRequestHandler<GetRefuelsByVehicleIdQuery, IEnumerable<RefuelDto>>
{
    private readonly IRefuelRepository _repository;

    public GetRefuelsByVehicleIdQueryHandler(IRefuelRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<IEnumerable<RefuelDto>> Handle(GetRefuelsByVehicleIdQuery request, CancellationToken cancellationToken)
    {
        var refuels = await _repository.GetByVehicleIdAsync(request.VehicleId);
        return refuels.Select(RefuelMapper.ToDto);
    }
}
