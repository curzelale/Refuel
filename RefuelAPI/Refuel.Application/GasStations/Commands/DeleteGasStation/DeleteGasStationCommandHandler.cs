using Refuel.Application.Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;

namespace Refuel.Application.GasStations.Commands.DeleteGasStation;

public class DeleteGasStationCommandHandler : IRequestHandler<DeleteGasStationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGasStationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HandleAsync(DeleteGasStationCommand request, CancellationToken cancellationToken = default)
    {
        var gasStation = await _unitOfWork.Repository<GasStation>().GetByIdAsync(request.Id)
                         ?? throw new KeyNotFoundException($"GasStation with id '{request.Id}' was not found.");

        _unitOfWork.Repository<GasStation>().Delete(gasStation);
        await _unitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}