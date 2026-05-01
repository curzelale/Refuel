using Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.GasStations.Commands.DeleteGasStation;

public class DeleteGasStationCommandHandler : IRequestHandler<DeleteGasStationCommand, bool>
{
    private readonly IRepository<GasStation> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGasStationCommandHandler(IRepository<GasStation> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<bool> Handle(DeleteGasStationCommand request, CancellationToken cancellationToken = default)
    {
        var gasStation = await _repository.GetByIdAsync(request.Id)
                         ?? throw new KeyNotFoundException($"GasStation with id '{request.Id}' was not found.");

        _repository.Delete(gasStation);
        await _unitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}