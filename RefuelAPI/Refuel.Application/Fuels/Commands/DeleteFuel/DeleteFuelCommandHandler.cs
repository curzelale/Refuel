using Refuel.Application.Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Fuels.Commands.DeleteFuel;

public class DeleteFuelCommandHandler : IRequestHandler<DeleteFuelCommand, bool>
{
    private readonly IRepository<Fuel> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFuelCommandHandler(IRepository<Fuel> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HandleAsync(DeleteFuelCommand request, CancellationToken cancellationToken = default)
    {
        var fuel = await _repository.GetByIdAsync(request.Id)
                   ?? throw new KeyNotFoundException($"Fuel with id '{request.Id}' was not found.");

        _repository.Delete(fuel);
        await _unitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}
