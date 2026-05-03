using Mediator;
using Refuel.Application.Refuels.Dtos;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Refuels.Queries.GetRefuelById;

public class GetRefuelByIdQueryHandler : IRequestHandler<GetRefuelByIdQuery, RefuelDto?>
{
    private readonly IRefuelRepository _repository;

    public GetRefuelByIdQueryHandler(IRefuelRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<RefuelDto?> Handle(GetRefuelByIdQuery request, CancellationToken cancellationToken)
    {
        var refuel = await _repository.GetByIdAsync(request.Id);
        return refuel is null ? null : RefuelMapper.ToDto(refuel);
    }
}
