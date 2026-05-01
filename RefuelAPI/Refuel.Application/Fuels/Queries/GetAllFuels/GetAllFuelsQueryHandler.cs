using Refuel.Application.Fuels.Dtos;
using Mediator;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Fuels.Queries.GetAllFuels;

public class GetAllFuelsQueryHandler : IRequestHandler<GetAllFuelsQuery, IEnumerable<FuelDto>>
{
    private readonly IRepository<Fuel> _repository;

    public GetAllFuelsQueryHandler(IRepository<Fuel> repository)
    {
        _repository = repository;
    }

    public async ValueTask<IEnumerable<FuelDto>> Handle(GetAllFuelsQuery request,
        CancellationToken cancellationToken = default)
    {
        var fuels = await _repository.GetAllAsync();
        return fuels.Select(f => new FuelDto(f.Id, f.Name));
    }
}
