using Mediator;
using Refuel.Application.Refuels.Dtos;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Refuels.Queries.GetAllRefuels;

public class GetAllRefuelsQueryHandler : IRequestHandler<GetAllRefuelsQuery, IEnumerable<RefuelDto>>
{
    private readonly IRefuelRepository _repository;

    public GetAllRefuelsQueryHandler(IRefuelRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<IEnumerable<RefuelDto>> Handle(GetAllRefuelsQuery request, CancellationToken cancellationToken)
    {
        var refuels = await _repository.GetAllAsync();
        return refuels.Select(RefuelMapper.ToDto);
    }
}
